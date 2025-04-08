using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public override void SetupRadio(bool rx, bool tx)
    {
      base.SetupRadio(rx, tx);

      if (rx && tx)
      {
        Rig!.Split = RigParamX.PM_SPLITON;
      }
      else
      {
        Rig!.Split = RigParamX.PM_SPLITOFF;
      }
    }

    private void Stop()
    {
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

    public override void Dispose()
    {
      Stop();
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
    public override void SetRxFrequency(double frequency)
    {
      if (rx && tx) Rig.Vfo = RigParamX.PM_VFOA;
      Rig!.Freq = (int)frequency;
    }

    public override void SetTxFrequency(double frequency)
    {
      if (rx && tx) Rig!.Vfo = RigParamX.PM_VFOB;
      Rig!.Freq = (int)frequency;
    }

    public override void SetRxMode(Slicer.Mode mode)
    {
      if (rx && tx) Rig!.Vfo = RigParamX.PM_VFOA;
      Rig!.Mode = SlicerModeToOmniRigMode(mode);
    }

    public override void SetTxMode(Slicer.Mode mode)
    {
      if (rx && tx) Rig!.Vfo = RigParamX.PM_VFOB;
      Rig!.Mode = SlicerModeToOmniRigMode(mode);
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






    private void Events_CustomReply(int RigNumber, object Command, object Reply)
    {

    }
  }
}
