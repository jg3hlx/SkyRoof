using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using OrbiNom.Properties;
using Serilog;
using VE3NEA;

namespace OrbiNom
{
  public enum CatMode { RxOnly, TxOnly, Split, Duplex }

  public class CatEngine : IDisposable
  {
    public RadioInfo RadioInfo;
    protected CatMode CatMode;
    protected SynchronizationContext syncContext = SynchronizationContext.Current!;
    protected EventWaitHandle? wakeupEvent;
    protected Thread? processingThread;
    protected bool stopping = false;
    private bool Ptt => ptt ??= ReadPtt();
    private bool? ptt;

    private long RequestedRxFrequency, RequestedTxFrequency;
    private long LastWrittenRxFrequency, LastWrittenTxFrequency;
    private Slicer.Mode? RequestedRxMode, RequestedTxMode;
    private Slicer.Mode? LastWrittenRxMode, LastWrittenTxMode;
    public long LastReadRxFrequency, LastReadTxFrequency;

    public readonly CatSettings Settings;
    private TcpClient? TcpClient;

    public event EventHandler? StatusChanged;
    public event EventHandler? RxTuned;
    public event EventHandler? TxTuned;

    public static RadioInfoList BuildRadioInfoList()
    {
      string path = Path.Combine(Utils.GetUserDataFolder(), "cat_info.json");
      //if (!File.Exists(path)) 
        File.WriteAllBytes(path, Resources.cat_info);
      return JsonConvert.DeserializeObject<RadioInfoList>(File.ReadAllText(path))!;
    }

    public CatEngine(CatSettings settings)
    {
      Settings = settings;
      RadioInfo = BuildRadioInfoList().First(r => r.radio == Settings.RadioModel);
    }


    private void LogFreqs(string msg)
    { 
      Log.Information($"{msg}  (RxReq={RequestedRxFrequency} RxWr={LastWrittenRxFrequency}  RxRd={LastReadRxFrequency})");
    }
    private long Round(double freq)
    {
      return 10 * (long)Math.Round(freq / 10);
    }

    //----------------------------------------------------------------------------------------------
    //                                      public methods
    //----------------------------------------------------------------------------------------------
    public virtual void Start(bool rx, bool tx, bool crossband)
    {
      // determmine required cat control mode
      if (rx && !tx) CatMode = CatMode.RxOnly;
      else if (!rx && tx) CatMode = CatMode.TxOnly;
      else if (crossband && RadioInfo.commands.setup_duplex != null) CatMode = CatMode.Duplex;
      else if (RadioInfo.commands.setup_split != null) CatMode = CatMode.Split;
      else { Log.Error($"Radio {RadioInfo.radio} does not support split mode"); return; }

      // connect to rigctld
      TcpClient = new();
      TcpClient.SendTimeout = 200;
      TcpClient.ReceiveTimeout = 1000;
      TcpClient.Connect(Settings.Host, Settings.Port);

      // set up required cat control mode
      switch (CatMode)
      {
        case CatMode.RxOnly:
        case CatMode.TxOnly:
          RunSetupSequence(RadioInfo.commands.setup_simplex);
          break;
        case CatMode.Split:
          RunSetupSequence(RadioInfo.commands.setup_split);
          break;
        case CatMode.Duplex:
          RunSetupSequence(RadioInfo.commands.setup_duplex);
          break;
      }

      StartThread();
    }

    protected virtual void Stop()
    {
      if (stopping) return;
      StopThread();
      TcpClient?.Dispose();
      TcpClient = null;
    }

    public void SetRxFrequency(double frequency)
    {
      frequency = Round(frequency);
      LogFreqs($"SetRxFrequency {frequency}");
      RequestedRxFrequency = (long)Math.Truncate(frequency);
    }

    public void SetTxFrequency(double frequency)
    {
      frequency = Round(frequency);
      RequestedTxFrequency = (long)Math.Truncate(frequency);
    }

    public void SetRxMode(Slicer.Mode mode)
    {
      RequestedRxMode = mode;
    }

    public void SetTxMode(Slicer.Mode mode)
    {
      RequestedTxMode = mode;
    }

    public bool IsRunning()
    {
      return TcpClient?.Connected ?? false;
    }

    public string GetStatusString()
    {
      return IsRunning() ? "Connected" : "Disconnected";
    }




    //----------------------------------------------------------------------------------------------
    //                                        thread 
    //----------------------------------------------------------------------------------------------
    private void StartThread()
    {
      wakeupEvent = new EventWaitHandle(false, EventResetMode.AutoReset);

      processingThread = new Thread(new ThreadStart(ThreadProcedure));
      processingThread.IsBackground = true;
      processingThread.Name = GetType().Name;
      processingThread.Start();
      processingThread.Priority = ThreadPriority.Highest;
    }

    private void StopThread()
    {
      stopping = true;
      wakeupEvent!.Set();
      processingThread!.Join();
      processingThread = null;
      wakeupEvent.Dispose();
      wakeupEvent = null;
    }

    protected virtual void ThreadProcedure()
    {
      while (!stopping)
      {
        try
        {
          if (stopping) break;

          ptt = null; // will read it only if needed

          TryReadRxFrequency();
          TryReadTxFrequency();
          TryWriteRxFrequency();
          TryWriteTxFrequency();
          TryWriteRxMode();
          TryWriteTxMode();

          Thread.Sleep(100);
        }
        catch (SocketException ex)
        {
          Log.Error(ex, $"Socket error in {GetType().Name}");

          // todo: connection lost, terminate thread
          // Stop();
          //StatusChanged?.Invoke(this, EventArgs.Empty);
          //break;
        }
        catch (Exception ex)
        {
          Log.Error(ex, $"Error in {GetType().Name}");
        }
      }
    }




    //----------------------------------------------------------------------------------------------
    //                                         read
    //----------------------------------------------------------------------------------------------
    private void TryReadRxFrequency()
    {
      if (HasCapability(RadioInfo.capabilities.read_main_frequency))
      {
        string command = RadioInfo.commands.read_main_frequency!;
        var reply = SendReadCommand(command);
        if (reply == null) return;
        if (!long.TryParse(reply, out long frequency)) BadReply(reply);
        frequency = Round(frequency);

        bool changed = LastReadRxFrequency != 0 &&
          IsDiff(frequency, LastReadRxFrequency) &&    // same freq as before
          IsDiff(frequency, LastWrittenRxFrequency) && // new freq was written
          IsDiff(frequency, LastWrittenTxFrequency);   // tx freq read by accident, ignore

        LastReadRxFrequency = frequency;
        if (changed) 
          OnRxFrequencyChanged();
      }
    }

    private void TryReadTxFrequency()
    {
      if (CatMode != CatMode.TxOnly) return;

      string command;
      if (CatMode == CatMode.TxOnly && HasCapability(RadioInfo.capabilities.read_main_frequency))
        command = RadioInfo.commands.read_main_frequency!;
      else if (CatMode != CatMode.TxOnly && HasCapability(RadioInfo.capabilities.read_split_frequency))
        command = RadioInfo.commands.read_split_frequency!;
      else
        return;

      var reply = SendReadCommand(command);
      if (reply == null) return;
      if (!long.TryParse(reply, out long frequency)) BadReply(reply);
      frequency = Round(frequency);

      bool changed = LastReadTxFrequency != 0 && IsDiff(frequency, LastReadTxFrequency);
      LastReadTxFrequency = frequency;
      if (changed) OnTxFrequencyChanged();
    }




    //----------------------------------------------------------------------------------------------
    //                                        write
    //----------------------------------------------------------------------------------------------
    private void TryWriteRxFrequency()
    {
      if (!NeedToWriteRxFrequency()) return;
      if (!HasCapability(RadioInfo.capabilities.set_main_frequency)) return;

      long freq = RequestedRxFrequency;
      string command = RadioInfo.commands.set_main_frequency!.Replace("{frequency}", $"{freq}");
      SendWriteCommand(command);
      LastWrittenRxFrequency = freq;
      //LastReadRxFrequency = RequestedRxFrequency;
      LogFreqs("Rx frequency written");
    }

    private void TryWriteTxFrequency()
    {
      if (!NeedToWriteTxFrequency()) return;

      // tx only, write main frequency
      string command;
      if (CatMode == CatMode.TxOnly && HasCapability(RadioInfo.capabilities.set_main_frequency))
        command = RadioInfo.commands.set_main_frequency!.Replace("{frequency}", $"{RequestedTxFrequency}");
      
      // split or duplex, write split frequency
      else if (CatMode != CatMode.TxOnly && HasCapability(RadioInfo.capabilities.set_split_frequency))
        command = RadioInfo.commands.set_split_frequency!.Replace("{frequency}", $"{RequestedTxFrequency}");

      // no capability
      else return;
     
      SendWriteCommand(command);
      LastWrittenTxFrequency = RequestedTxFrequency;
      //LastReadTxFrequency = RequestedTxFrequency;
    }

    private void TryWriteRxMode()
    {
      if (!NeedToWriteRxMode()) return;
      if (!HasCapability(RadioInfo.capabilities.set_main_mode)) return;
     
      string mode = $"{RequestedRxMode}".Replace("_D", "");
      string command = RadioInfo.commands.set_main_mode!.Replace("{mode}", mode);
      SendWriteCommand(command);
      LastWrittenRxMode = RequestedRxMode;
    }

    private void TryWriteTxMode()
    {
      if (!NeedToWriteTxMode()) return;

      string command;
      if (CatMode == CatMode.TxOnly && HasCapability(RadioInfo.capabilities.set_main_mode))
        command = RadioInfo.commands.set_main_mode!.Replace("{mode}", $"{RequestedTxMode}");
      else if (CatMode != CatMode.TxOnly && HasCapability(RadioInfo.capabilities.set_split_mode))
        command = RadioInfo.commands.set_split_mode!.Replace("{mode}", $"{RequestedTxMode}");
      else return;

      //SendWriteCommand($"V VFOA");
      var info = SendReadCommand(" \\get_rig_info");

      SendWriteCommand(command);

      LastWrittenTxMode = RequestedTxMode;
    }




    //----------------------------------------------------------------------------------------------
    //                                  check conditions
    //----------------------------------------------------------------------------------------------
    // this function reads Ptt only if capability depends on it, and reads it only once
    private bool HasCapability(string[] capability)
    {
      return
        (capability.Contains("when_receiving") && capability.Contains("when_transmitting"))
        ||
        (capability.Contains("when_receiving") && !Ptt)
        ||
        (capability.Contains("when_transmitting") && Ptt);
    }

    private bool NeedToWriteRxFrequency()
    {
      LogFreqs("NeedToWriteRxFreq?");
      if (CatMode == CatMode.TxOnly) return false;
      if (RequestedRxFrequency == 0) return false; // never assigned
      return IsDiff(RequestedRxFrequency, LastWrittenRxFrequency);
    }

    private bool NeedToWriteTxFrequency()
    {
      if (CatMode == CatMode.RxOnly) return false;
      if (RequestedTxFrequency == 0) return false; // never assigned
      return IsDiff(RequestedTxFrequency, LastWrittenTxFrequency);
    }

    private bool NeedToWriteRxMode()
    {
      if (CatMode == CatMode.TxOnly) return false;
      if (!RequestedRxMode.HasValue) return false;
      return RequestedRxMode != LastWrittenRxMode;
    }

    private bool NeedToWriteTxMode()
    {
      if (CatMode == CatMode.RxOnly) return false;
      if (!RequestedTxMode.HasValue) return false;
      return RequestedTxMode != LastWrittenTxMode;
    }

    private bool IsDiff(long freq1, long freq2)
    {
      return Math.Abs(freq1 - freq2) > 0;
    }




    //----------------------------------------------------------------------------------------------
    //                                       commands
    //----------------------------------------------------------------------------------------------
    private void RunSetupSequence(string[]? setup_simplex)
    {
      foreach (string cmd in setup_simplex!)
        SendWriteCommand(cmd);
    }

    private bool SendWriteCommand(string command)
    {
      var reply = SendCommand(command);
      if (reply == "RPRT 0\n") return true;

      else if (reply != null) BadReply(reply);
      return false;
    }

    private string? SendReadCommand(string command)
    {
      var reply = SendCommand(command);
      if (reply == null) return null;
      if (reply.EndsWith("\n")) 
          {
        reply = reply.Substring(0, reply.Length - 1);
        Log.Information($"Reply from rigctld: {reply}");

        return reply;
      }
      BadReply(reply);
      return null;
    }

    private string? SendCommand(string command)
    {
      try
      {
        Log.Information($"Sending command to rigctld: {command}");
        byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\n");
        TcpClient!.Client.Send(commandBytes);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Failed to send command to rigctld");
        return null;
      }

      try
      {
        byte[] replyBytes = new byte[1024];
        int replyByteCount = TcpClient.Client.Receive(replyBytes);
        return Encoding.ASCII.GetString(replyBytes, 0, replyByteCount);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Failed to receive reply from rigctld");
        return null;
      }
    }

    private void BadReply(string reply)
    {
      Log.Error($"Unexpected reply from rigctld: {reply}");
    }

    private bool ReadPtt()
    {
      var reply = SendReadCommand(RadioInfo.commands.read_ptt!);
      bool newPtt = reply == "1";

      //if (newPtt != ptt) LastReadRxFrequency = 0;
       return newPtt;
    }




    //----------------------------------------------------------------------------------------------
    //                                      events
    //----------------------------------------------------------------------------------------------
    protected void OnStatusChanged()
    {
      StatusChanged?.Invoke(this, EventArgs.Empty);
    }

    protected void OnRxFrequencyChanged()
    {
      LogFreqs($"OnRxFrequencyChanged");
      RxTuned?.Invoke(this, EventArgs.Empty);
    }

    protected void OnTxFrequencyChanged()
    {
      LogFreqs($"OnTxFrequencyChanged");

      TxTuned?.Invoke(this, EventArgs.Empty);
    }




    //----------------------------------------------------------------------------------------------
    //                                      IDisposable
    //----------------------------------------------------------------------------------------------
    public virtual void Dispose()
    {
      Stop();
    }
  }
}
