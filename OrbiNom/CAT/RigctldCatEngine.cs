using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbiNom
{
  internal class RigctldCatEngine : CatEngine
  {
    public override string GetStatusString()
    {
      return "OK";
    }

    public override bool IsRunning()
    {
      return true;
    }

    public override void SetRxFrequency(double frequency)
    {
      throw new NotImplementedException();
    }

    public override void SetRxMode(Slicer.Mode mode)
    {
      throw new NotImplementedException();
    }

    public override void SetTxFrequency(double frequency)
    {
      throw new NotImplementedException();
    }

    public override void SetTxMode(Slicer.Mode mode)
    {
      throw new NotImplementedException();
    }

    public override void SetupRadio(bool rx, bool tx)
    {
      
    }

    public override void Dispose()
    {

    }
  }
}
