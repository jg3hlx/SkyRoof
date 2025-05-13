using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SkyRoof
{
  public class RotatorEngine
  {
    public Context ctx;
    private bool enabled;

    public RotatorEngine()
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
  }
}
