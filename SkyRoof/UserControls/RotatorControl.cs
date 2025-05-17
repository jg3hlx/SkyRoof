using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkyRoof.Forms;

namespace SkyRoof
{
  public partial class RotatorControl : UserControl
  {
    public Context ctx;
    private RotatorControlEngine? engine;
    private AzElEntryDialog Dialog = new();

    public RotatorControl()
    {
      InitializeComponent();
    }




    //----------------------------------------------------------------------------------------------
    //                                        engine
    //----------------------------------------------------------------------------------------------
    public void ApplySettings()
    {
      engine?.Dispose();
      engine = new RotatorControlEngine(ctx.Settings.Rotator);
    }

    public bool IsRunning()
    {
      return true;
    }

    public void Go(int lat, int lon)
    {
      engine.Go(lat, lon);
    }

    public void StopRotation()
    {
      TrackCheckbox.Checked = false;
      engine.Stop();
    }





    //----------------------------------------------------------------------------------------------
    //                                        UI
    //----------------------------------------------------------------------------------------------
    private void AzEl_Click(object sender, EventArgs e)
    {
      Dialog.Open(ctx);
    }

    private void TrackCheckbox_CheckedChanged(object sender, EventArgs e)
    {
      if (!TrackCheckbox.Checked) StopRotation();
    }

    private void StopBtn_Click(object sender, EventArgs e)
    {
      StopRotation();
    }

    internal string? GetStatusString()
    {
      if (!ctx.Settings.Rotator.Enabled) 
        return "Rotator control disabled";
      else if (!IsRunning()) 
        return "No connection";
      else if (!TrackCheckbox.Checked || ctx.FrequencyControl.RadioLink.IsTerrestrial)
        return "Connected, tracking disabled";
      else 
        return "Connected and tracking";      
    }
  }
}
