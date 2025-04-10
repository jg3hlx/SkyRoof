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

    protected override void InternalSetRxFrequency(long frequency)
    {
      throw new NotImplementedException();
    }

    protected override void InternalSetRxMode(Slicer.Mode mode)
    {
      throw new NotImplementedException();
    }

    protected override void InternalSetTxFrequency(long frequency)
    {
      throw new NotImplementedException();
    }

    protected override void InternalSetTxMode(Slicer.Mode mode)
    {
      throw new NotImplementedException();
    }

    public override void Start(bool rx, bool tx)
    {
      
    }

    public override void Dispose()
    {

    }

    protected override bool InternalGetTxFrequency()
    {
      throw new NotImplementedException();
    }

    protected override bool InternalGetRxFrequency()
    {
      throw new NotImplementedException();
    }
  }
}
