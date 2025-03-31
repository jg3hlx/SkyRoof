using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbiNom
{
  public abstract class CatEngine : IDisposable
  {
    public double RxFrequency { get; protected set; }
    public double TxFrequency { get; protected set; }


    public event EventHandler? StatusChanged;
    public event EventHandler? RxTuned;
    public event EventHandler? TxTuned;


    public static CatEngine CreateEngine(OmniRigSettings settings) 
    {
      return new OmniRigCatEngine();
    }

    public static CatEngine CreateEngine(RigctldSettings settings)
    {
      return new RigctldCatEngine();
    }

    protected void OnStatusChanged()
    {
      StatusChanged?.Invoke(this, EventArgs.Empty);
    }

    protected void OnRxFrequencyChanged()
    {
      RxTuned?.Invoke(this, EventArgs.Empty);
    }

    protected void OnTxFrequencyChanged()
    {
      TxTuned?.Invoke(this, EventArgs.Empty);
    }


    public abstract void SetupRadio(bool rx, bool tx);
    public abstract void SetRxFrequency(double frequency);
    public abstract void SetTxFrequency(double frequency);

    public abstract void SetRxMode(Slicer.Mode mode);
    public abstract void SetTxMode(Slicer.Mode mode);

    public abstract bool IsRunning();
    public abstract string GetStatusString();

    public abstract void Dispose();
  }
}
