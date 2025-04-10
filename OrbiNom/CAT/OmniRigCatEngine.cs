using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using OmniRig;
using VE3NEA;

namespace OrbiNom
{
  public enum OmniRigRig
  {
    Rig1,
    Rig2
  }


  // Controlling the radio in the Sat mode requires a special OmniRig ini file, IC-9700-MIN.ini
  // In this file the vfoa and vfob commands switch between the Main and Sub receivers, while
  // the split command  toggles the Sat mode.
  // This is needed only if rx and tx CAT options are enabled and the same radio is used for both.
  // When only rx or tx is controlled, or rx and tx are two different radios, the standard
  // ini files should be used.
  internal class OmniRigCatEngine : CatEngine
  {
    private IOmniRigX? OmniRig = null;
    private IOmniRigXEvents_Event? events;
    private IRigX? Rig = null;
    private int RigNo;


    //----------------------------------------------------------------------------------------------
    //                                  create / destroy
    //----------------------------------------------------------------------------------------------
    public OmniRigCatEngine(OmniRigSettings settings)
    {
      RigNo = (int)settings.RigNo + 1;

      try
      {
        OmniRig = new OmniRigX();
        Rig = (settings.RigNo == OmniRigRig.Rig1) ? OmniRig.Rig1 : OmniRig.Rig2;
        events = OmniRig as IOmniRigXEvents_Event;
        events!.StatusChange += Events_StatusChange;
        events!.CustomReply += Events_CustomReply;
      }
      catch
      {
        Stop();
      }
    }

    public override void Start(bool rx, bool tx)
    {
      if (rx && tx)
        Rig!.Split = RigParamX.PM_SPLITON;
      else
        Rig!.Split = RigParamX.PM_SPLITOFF;

      base.Start(rx, tx);
    }

    protected override void Stop()
    {
      base.Stop();

      if (events != null)
      {
        events!.StatusChange -= Events_StatusChange;
        events!.CustomReply -= Events_CustomReply;

        Marshal.ReleaseComObject(events);
        events = null;
      }

      if (Rig != null) Marshal.ReleaseComObject(Rig);
      Rig = null;

      if (OmniRig != null) Marshal.ReleaseComObject(OmniRig);
      OmniRig = null;
    }





    //----------------------------------------------------------------------------------------------
    //                                    status
    //----------------------------------------------------------------------------------------------
    private void Events_StatusChange(int RigNumber)
    {
      if (Rig == null || RigNumber != RigNo) return;
      OnStatusChanged();
    }

    public override string GetStatusString()    
    {
      if (Rig == null) return "OmniRig is not installed"; ;

      switch (Rig!.Status)
      {
        case RigStatusX.ST_NOTCONFIGURED: return "OmniRig not configured";
        case RigStatusX.ST_DISABLED: return "CAT interface is disabled";
        case RigStatusX.ST_PORTBUSY: return "COM port is not available";
        case RigStatusX.ST_NOTRESPONDING: return "Radio is not responding";
        case RigStatusX.ST_ONLINE: return "'CAT interface OK";
        default: return "";
      }
    }

    public override bool IsRunning()
    {
      return Rig?.Status == RigStatusX.ST_ONLINE;
    }




    //----------------------------------------------------------------------------------------------
    //                                    set param
    //----------------------------------------------------------------------------------------------
    protected override void InternalSetRxFrequency(long frequency)
    {
      if (rx && tx) Rig.Vfo = RigParamX.PM_VFOA;
      Rig!.Freq = (int)frequency;
      //Debug.WriteLine($"Set Rx frequency: {(int)frequency} Hz");
    }

    protected override void InternalSetTxFrequency(long frequency)
    {
      if (rx && tx) Rig!.Vfo = RigParamX.PM_VFOB;
      Rig!.Freq = (int)frequency;
      Debug.WriteLine($"Set Tx frequency: {(int)frequency} Hz");
    }

    protected override void InternalSetRxMode(Slicer.Mode mode)
    {
      if (rx && tx) Rig!.Vfo = RigParamX.PM_VFOA;
      Rig!.Mode = SlicerModeToOmniRigMode(mode);
    }

    protected override void InternalSetTxMode(Slicer.Mode mode)
    {
      if (rx && tx) Rig!.Vfo = RigParamX.PM_VFOB;
      Rig!.Mode = SlicerModeToOmniRigMode(mode);
    }

    protected override bool InternalGetRxFrequency()
    {
      if (rx && tx) Rig.Vfo = RigParamX.PM_VFOA;
      return SendReadFrequencyCommand();
    }

    protected override bool InternalGetTxFrequency()
    {
      if (rx && tx) Rig.Vfo = RigParamX.PM_VFOB;
      return SendReadFrequencyCommand();
    }

    private RigParamX SlicerModeToOmniRigMode(Slicer.Mode mode)
    {
      return mode switch
      {
        Slicer.Mode.USB => RigParamX.PM_SSB_U,
        Slicer.Mode.LSB => RigParamX.PM_SSB_L,
        Slicer.Mode.USB_D => RigParamX.PM_DIG_U,
        Slicer.Mode.LSB_D => RigParamX.PM_DIG_L,
        Slicer.Mode.CW => RigParamX.PM_CW_U,
        Slicer.Mode.FM => RigParamX.PM_FM,
        _ => throw new NotImplementedException(),
      };
    }




    private readonly byte[] Command = { 0xfe, 0xfe, 0xa2, 0xe0, 0x03, 0xfd };
    private readonly byte[] ReplyMask = {
        0xfe, 0xfe, 0xa2, 0xe0, 0x03, 0xfd,
        0xfe, 0xfe, 0xe0, 0xa2, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0xfd
      };

    private bool SendReadFrequencyCommand()
    {
      // send custom command
      Rig!.SendCustomCommand(Command, ReplyMask.Length, null);

      // wait for custom reply
      return wakeupEvent!.WaitOne(3000);
    }

    private void Events_CustomReply(int RigNumber, object Command, object Reply)
    {
      // verify reply
      if (Rig == null || RigNumber != RigNo) return;
      if (!(Reply is byte[] bytes) || bytes.Length < ReplyMask.Length) return;
      for (int i = 0; i < ReplyMask.Length; i++)
        if (ReplyMask[i] != 0 && bytes[i] != ReplyMask[i]) return;

      // parse reply
      long freq = 0;
      bytes = bytes.Skip(11).Take(5).Reverse().ToArray();
      foreach (byte b in bytes)
        freq = freq * 100 + ((b >> 4) & 0x0F) * 10 + (b & 0x0F);
      NewReadFrequency = freq;
      Debug.WriteLine($"Read frequency: {freq} Hz");

      // notify
      wakeupEvent?.Set();
    }
  }
}
