using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Speech.Synthesis.TtsEngine;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CSCore.Win32;
using Newtonsoft.Json;
using Serilog;
using SkyRoof.Properties;
using VE3NEA;

namespace SkyRoof
{
  public enum CatMode { RxOnly, TxOnly, Split, Duplex }

  public class CatControlEngine : ControlEngine
  {
    private RadioInfo RadioInfo;
    public CatMode CatMode;
    private readonly bool IgnoreDialKnob;

    private bool Ptt => ptt ??= ReadPtt();

    private bool? ptt;

    private long RequestedRxFrequency, RequestedTxFrequency;
    private long LastWrittenRxFrequency, LastWrittenTxFrequency;
    public long LastReadRxFrequency, LastReadTxFrequency;
    private Slicer.Mode? RequestedRxMode, RequestedTxMode;
    private Slicer.Mode? LastWrittenRxMode, LastWrittenTxMode;
    private bool? RequestedPtt;

    public event EventHandler? RxTuned;
    public event EventHandler? TxTuned;

    public static RadioInfoList BuildRadioInfoList()
    {
      string path = Path.Combine(Utils.GetUserDataFolder(), "cat_info.json");
      if (!File.Exists(path)) File.WriteAllBytes(path, Resources.cat_info);

      return JsonConvert.DeserializeObject<RadioInfoList>(File.ReadAllText(path))!;
    }

    public CatControlEngine(CatRadioSettings radioSettings, CatSettings catSettings) : base(radioSettings.Host, radioSettings.Port, catSettings)
    {
      RadioInfo = BuildRadioInfoList().First(r => r.radio == radioSettings.RadioType);
    }


    private void LogFreqs(string msg)
    {
      if (log) Log.Information($"{msg}  (RxReq={RequestedRxFrequency:N0}  RxWr={LastWrittenRxFrequency:N0}  RxRd={LastReadRxFrequency:N0})");
    }

    // some radios use 10 Hz steps and some don't, handle them all the same way
    private long RoundTo10(double freq)
    {
      int step = RadioInfo.tuning_step_hz;
      return step * (long)Math.Truncate(freq / step);
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

      StartThread();
    }

    public void SetRxFrequency(double frequency)
    {
      frequency = RoundTo10(frequency);
      LogFreqs($"SetRxFrequency {frequency}");
      RequestedRxFrequency = (long)frequency;
    }

    public void SetTxFrequency(double frequency)
    {
      frequency = RoundTo10(frequency);
      LogFreqs($"SetTxFrequency {frequency}");
      RequestedTxFrequency = (long)frequency;
    }

    public void SetRxMode(Slicer.Mode mode)
    {
      RequestedRxMode = mode;
    }

    public void SetTxMode(Slicer.Mode mode)
    {
      RequestedTxMode = mode;
    }

    public void SetPtt(bool ptt)
    {
      RequestedPtt = ptt;
    }






    //----------------------------------------------------------------------------------------------
    //                                        thread 
    //----------------------------------------------------------------------------------------------
    protected override void ReadWrite()
    {
      // will read PTT only if needed
      ptt = null; 

      if (!IgnoreDialKnob)
      {
        TryReadRxFrequency();
        TryReadTxFrequency();
      }

      TryWriteRxFrequency();
      TryWriteTxFrequency();
      TryWriteRxMode();
      TryWriteTxMode();
      TryWritePtt();
    }


    protected override bool Setup()
    {
      try
      {
        bool ok = false;

        switch (CatMode)
        {
          case CatMode.RxOnly:
          case CatMode.TxOnly:
            ok = SendWriteCommands(RadioInfo.commands.setup_simplex);
            break;
          case CatMode.Split:
            ok = SendWriteCommands(RadioInfo.commands.setup_split);
            break;
          case CatMode.Duplex:
            ok = SendWriteCommands(RadioInfo.commands.setup_duplex);
            break;
        }

        return ok;
      }
      catch (Exception ex)
      {
        Log.Error(ex, $"Failed to set up radio {RadioInfo.radio}");
        return false;
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
        if (!long.TryParse(reply, CultureInfo.InvariantCulture, out long frequency)) BadReply(reply);
        frequency = RoundTo10(frequency);

        bool changed = LastReadRxFrequency != 0 &&     // first read
          IsDiff(frequency, LastReadRxFrequency) &&    // same freq as before
          IsDiff(frequency, LastWrittenRxFrequency) && // new freq was written
          IsDiff(frequency, LastWrittenTxFrequency);   // tx freq read by accident, ignore

        LastReadRxFrequency = frequency;
        if (changed) OnRxFrequencyChanged();
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
      if (!long.TryParse(reply, CultureInfo.InvariantCulture, out long frequency)) BadReply(reply);
      frequency = RoundTo10(frequency);

      bool changed = LastReadTxFrequency != 0 &&
        IsDiff(frequency, LastReadTxFrequency) &&
        IsDiff(frequency, LastWrittenTxFrequency) &&
        IsDiff(frequency, LastWrittenRxFrequency);

      LastReadTxFrequency = frequency;
      if (changed) OnTxFrequencyChanged();
    }

    private bool ReadPtt()
    {
      var reply = SendReadCommand(RadioInfo.commands.read_ptt!);
      bool newPtt = reply == "1";
      return newPtt;
    }




    //----------------------------------------------------------------------------------------------
    //                                        write
    //----------------------------------------------------------------------------------------------
    private void TryWriteRxFrequency()
    {
      if (!NeedToWriteRxFrequency()) return;
      if (!HasCapability(RadioInfo.capabilities.set_main_frequency)) return;

      // RequestedRxFrequency may change on another thread while we are writing, use a local copy
      long frequency = RequestedRxFrequency;

      string command = RadioInfo.commands.set_main_frequency!.Replace("{frequency}", $"{frequency}");
      SendWriteCommand(command);
      LastWrittenRxFrequency = frequency;
      LogFreqs("Rx frequency written");
    }

    private void TryWriteTxFrequency()
    {
      if (!NeedToWriteTxFrequency()) return;

      long frequency = RequestedTxFrequency;

      // tx only, write main frequency
      string command;
      if (CatMode == CatMode.TxOnly && HasCapability(RadioInfo.capabilities.set_main_frequency))
        command = RadioInfo.commands.set_main_frequency!.Replace("{frequency}", $"{frequency}");

      // split or duplex, write split frequency
      else if (CatMode != CatMode.TxOnly && HasCapability(RadioInfo.capabilities.set_split_frequency))
        command = RadioInfo.commands.set_split_frequency!.Replace("{frequency}", $"{frequency}");

      // no capability
      else return;

      SendWriteCommand(command);
      LastWrittenTxFrequency = frequency;
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

      SendWriteCommand(command);
      LastWrittenTxMode = RequestedTxMode;
    }

    private void TryWritePtt()
    {
      if (CatMode == CatMode.RxOnly) return;
      if (!RequestedPtt.HasValue) return;
      if (RequestedPtt == Ptt) return;

      if (RequestedPtt == false && RadioInfo.commands.set_ptt_off != null)
        SendWriteCommand(RadioInfo.commands.set_ptt_off);

      if (RequestedPtt == true && RadioInfo.commands.set_ptt_on != null)
      {
        TryWriteTxFrequencyBeforePtt();
        SendWriteCommand(RadioInfo.commands.set_ptt_on);
      }
    }

    private void TryWriteTxFrequencyBeforePtt()
    {
      if (!RadioInfo.capabilities.set_split_frequency.Contains("before_ptt_on")) return;

      long frequency = RequestedTxFrequency;
      string command;

      if (CatMode == CatMode.TxOnly && RadioInfo.commands.set_main_frequency != null)
        command = RadioInfo.commands.set_main_frequency!.Replace("{frequency}", $"{frequency}");

      else if (CatMode != CatMode.TxOnly && RadioInfo.commands.set_split_frequency != null)
        command = RadioInfo.commands.set_split_frequency!.Replace("{frequency}", $"{frequency}");

      else return;

      SendWriteCommand(command);
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
      if (CatMode == CatMode.TxOnly) return false;
      if (RequestedRxFrequency == 0) return false; // never assigned
      return IsDiff(RequestedRxFrequency, LastWrittenRxFrequency);
    }

    private bool NeedToWriteTxFrequency()
    {
      if (CatMode == CatMode.RxOnly) return false;
      if (RequestedTxFrequency == 0) return false;
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
    //                                      events
    //----------------------------------------------------------------------------------------------
    protected void OnRxFrequencyChanged()
    {
      LogFreqs($"OnRxFrequencyChanged");
      syncContext.Post(s => RxTuned?.Invoke(this, EventArgs.Empty), null);
    }

    protected void OnTxFrequencyChanged()
    {
      LogFreqs($"OnTxFrequencyChanged");
      syncContext.Post(s => TxTuned?.Invoke(this, EventArgs.Empty), null);
    }
  }
}
