using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace OrbiNom
{
  internal class RigctldCatEngine : CatEngine
  {
    private TcpClient TcpClient;
    private CatSettings settings;
    bool VfoMode;

    public RigctldCatEngine(CatSettings settings) : base(settings)
    {
      this.settings = settings;

      TcpClient = new();
      TcpClient.SendTimeout = 200;
      TcpClient.ReceiveTimeout = 1000;
    }


    //----------------------------------------------------------------------------------------------
    //                                      override
    //----------------------------------------------------------------------------------------------
    public override void Start(bool rx, bool tx, bool crossband)
    {
      //this.RxEnabled = rx;
      //this.TxEnabled = tx;
      //this.CrossBand = crossband;
      //RadioInfo = RadioInfoList.FirstOrDefault(r => r.radio == settings.RadioModel);

      try
      {
        TcpClient.Connect(settings.Host, settings.Port);
      }
      catch (SocketException ex)
      {
        Log.Error(ex, "Failed to connect to rigctld");
        return;
      }

      VfoMode = TrySetVfoMode();

//      // rx only or tx only
//      if (rx != tx) SetupSimplex();
//      
//      // rx and tx on the same band
//      else if (!crossband) SetupSplit();
//
//      // rx and tx on different bands
//      else SetupDuplex();

      base.Start(rx, tx, crossband);
    }

    protected override void Stop()
    {

    }

    //protected override bool InternalGetRxFrequency()
    //{
    //  return true;
    //}
    //
    //protected override void InternalSetRxFrequency(long frequency)
    //{
    //  WriteFrequency(frequency, false);
    //}
    //
    //protected override void InternalSetRxMode(Slicer.Mode mode)
    //{
    //  WriteMode(mode, false);
    //}
    //
    //protected override bool InternalGetTxFrequency()
    //{
    //  return true;
    //}
    //
    //protected override void InternalSetTxFrequency(long frequency)
    //{
    //  WriteFrequency(frequency, TxEnabled == RxEnabled);
    //}
    //
    //protected override void InternalSetTxMode(Slicer.Mode mode)
    //{
    //  WriteMode(mode, TxEnabled == RxEnabled);
    //}
    //
    //public override string GetStatusString()
    //{
    //  return IsRunning() ? "Connected" : "Disconnected";
    //}
    //
    //public override bool IsRunning()
    //{
    //  return TcpClient.Connected;
    //}




    //----------------------------------------------------------------------------------------------
    //                                       private
    //----------------------------------------------------------------------------------------------
    private string? SendCommand(string command)
    {
      try
      {
        Log.Information($"Sending command to rigctld: {command}");
        byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\n");
        TcpClient.Client.Send(commandBytes);
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

    private bool SendWriteCommand(string command)
    {
      var reply = SendCommand(command);
      if (reply == "RPRT 0") return true;

      else if (reply != null) BadReply(reply);
      return false;
    }

    private string? SendReadCommand(string command)
    {
      var reply = SendCommand(command);
      if (reply == null) return null;
      if (reply.EndsWith("\n")) return reply.Substring(0, reply.Length - 1);

      BadReply(reply);
      return null;
    }

    private void BadReply(string reply)
    {
      Log.Error($"Unexpected reply from rigctld: {reply}");
    }




    //----------------------------------------------------------------------------------------------
    //                                       read / write
    //----------------------------------------------------------------------------------------------
    private bool TrySetVfoMode()
    {
      SendWriteCommand("\\set_vfo_opt 0");
      var reply = SendReadCommand("\\chk_vfo");

      if (reply == null) return false;
      if (reply != "0" && reply != "1") BadReply(reply);

      return reply == "1";
    }

    private void SetDuplex(bool value)
    {
      string vfo = VfoMode ? " CurrVFO" : "";

      if (value)
      {
        string command = $"S{vfo} 1 Sub";
        SendWriteCommand(command);
      }
      else
      {
        string command = $"S{vfo} 0 VFOA";
        SendWriteCommand(command);
        command = $"U{vfo} DUAL_WATCHH 0";
        SendWriteCommand(command);
      }
    }

    private bool IsSatModeSupported()
    {
      var reply = SendReadCommand("U ?");
      if (reply == null) return false;
      return reply.Contains("SATMODE");
    }

    private void SetSatMode(bool enabled)
    {
      string command = "U SATMODE " + (enabled ? "1" : "0");
      SendWriteCommand(command);
    }

    private long? ReadFrequency(bool split)
    {
      string command = split ? "i" : "f";
      if (VfoMode) command += " CurrVFO";
      var reply = SendReadCommand(command);

      if (reply == null) return null;
      if (long.TryParse(reply, out long freq)) return freq;

      BadReply(reply);
      return null;
    }

    private bool WriteFrequency(long frequency, bool split)
    {
      string command = split ? "I" : "F";
      string vfoMode = VfoMode ? " CurrVFO" : "";
      command = $"{command}{vfoMode} {frequency}";
      return SendWriteCommand(command);
    }

    private void WriteMode(Slicer.Mode mode, bool split)
    {
      string command = split ? "X" : "M";
      string vfo = VfoMode ? " CurrVFO" : "";
      command = $"{command}{vfo} {mode} 0";
      SendWriteCommand(command);
    }

    private bool? ReadPtt()
    {
      string command = VfoMode ? "t CurrVFO" : "t";
      var reply = SendReadCommand(command);
      if (reply == null) return null;
      if (reply != "0" && reply != "1") BadReply(reply);
      return reply == "1";
    }

    private void WritePtt(bool enabled)
    {
      string value = enabled ? "1" : "0";
      string command = VfoMode ? $"T CurrVFO {value}" : $"T {value}";
      SendWriteCommand(command);
    }
  }
}
