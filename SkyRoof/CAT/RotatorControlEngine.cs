using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SkyRoof
{
  public class RotatorControlEngine : ControlEngine
  {
    private bool enabled;

    public RotatorControlEngine(RotatorSettings settings) : base(settings.Host, settings.Port, settings)
    {
    
    }
    

    public bool Enabled {get => enabled; set => SetEnabled(value); }

    private void SetEnabled(bool value)
    {
      
    }

    public void Go(int azimuth, int elevation)
    {
    }

    public void Stop()
    {
    }

    protected override void SendCommands()
    {
     
    }

    protected override bool Setup()
    {
      // nothing needs to be done here

      return true;
    }
  }
}
