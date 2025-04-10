using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace OrbiNom
{
  public abstract class CatEngine : IDisposable
  {
    protected bool rx;
    protected bool tx;
    protected SynchronizationContext syncContext = SynchronizationContext.Current!;
    protected EventWaitHandle? wakeupEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
    protected Thread? processingThread;
    protected bool stopping = false;
    public long RequestedRxFrequency, WrittenRxFrequency;
    public long RequestedTxFrequency, WrittenTxFrequency;
    public long ReadRxFrequency, ReadTxFrequency, NewReadFrequency;
    public Slicer.Mode? RequestedRxMode, WrittenRxMode = null;
    public Slicer.Mode? RequestedTxMode, WrittenTxMode = null;

    public double RxFrequency { get; protected set; }
    public double TxFrequency { get; protected set; }


    public event EventHandler? StatusChanged;
    public event EventHandler? RxTuned;
    public event EventHandler? TxTuned;

    public static CatEngine CreateEngine(OmniRigSettings settings) 
    {
      return new OmniRigCatEngine(settings);
    }

    public static CatEngine CreateEngine(RigctldSettings settings)
    {
      return new RigctldCatEngine();
    }

    public virtual void Start(bool rx, bool tx)
    {
      this.rx = rx;
      this.tx = tx;

      processingThread = new Thread(new ThreadStart(ProcessingThreadProcedure));
      processingThread.IsBackground = true;
      processingThread.Name = GetType().Name;
      processingThread.Start();
      processingThread.Priority = ThreadPriority.Highest;
    }

    protected virtual void Stop()
    {
      if (stopping) return;
      stopping = true;
      wakeupEvent!.Set();
      processingThread!.Join();
      processingThread = null;
      wakeupEvent.Dispose();
      wakeupEvent = null;
    }

    public void SetRxFrequency(double frequency)
    {
      RequestedRxFrequency = (long)Math.Truncate(frequency);
      //Debug.WriteLine($"RequestedRxFrequency <- {frequency}");
    }
    public void SetTxFrequency(double frequency)
    {
      RequestedTxFrequency = (long)Math.Truncate(frequency);
      Debug.WriteLine($"RequestedTxFrequency <- {frequency}");
    }

    public void SetRxMode(Slicer.Mode mode) => RequestedRxMode = mode;    
    public void SetTxMode(Slicer.Mode mode) => RequestedTxMode = mode;


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

    private bool IsDiff(long freq1, long freq2)
    {
      return freq1 != 0 && Math.Abs(freq1 - freq2) > 0;
    }

    protected virtual void ProcessingThreadProcedure()
    {
      while (!stopping)
      {
        if (stopping) break;
        try
        {
          // set rx frequency
          long newRxFrequency = RequestedRxFrequency;
          if (rx && IsDiff(newRxFrequency, WrittenRxFrequency))
          {
            InternalSetRxFrequency(newRxFrequency);
            WrittenRxFrequency = newRxFrequency;
            //Debug.WriteLine($"WrittenRxFrequency <- {WrittenRxFrequency}");
          }

          // set tx frequency
          long newTxFrequency = RequestedTxFrequency;
          if (tx && IsDiff(newTxFrequency, WrittenTxFrequency))
          {
            InternalSetTxFrequency(newTxFrequency);
            WrittenTxFrequency = newTxFrequency;
            Debug.WriteLine($"WrittenTxFrequency <- {WrittenTxFrequency}");
          }

          // read rx frequency
          if (rx)
          {
           bool ok = InternalGetRxFrequency();
            if (ok && IsDiff(NewReadFrequency, ReadRxFrequency))
            {
              ReadRxFrequency = NewReadFrequency;
              if (ReadRxFrequency != WrittenRxFrequency)
                syncContext.Post(_ => OnRxFrequencyChanged(), null);
            }
          }

          // read tx frequency
          if (tx)
          {
            bool ok = InternalGetTxFrequency();
            if (ok && IsDiff(NewReadFrequency, ReadTxFrequency))
            {
              ReadTxFrequency = NewReadFrequency;
              if (ReadTxFrequency != WrittenTxFrequency)
                syncContext.Post(_ => OnTxFrequencyChanged(), null);
            }
          }

          // set rx mode
          if (rx && RequestedRxMode != null && RequestedRxMode != WrittenRxMode)
          {
            InternalSetRxMode((Slicer.Mode)RequestedRxMode);
            WrittenRxMode = RequestedRxMode;
          }

          // set tx mode
          if (tx && RequestedTxMode != null && RequestedTxMode != WrittenTxMode)
          {
            InternalSetTxMode((Slicer.Mode)RequestedTxMode);
            WrittenTxMode = RequestedTxMode;
          }
        }
        catch (Exception ex)
        {
          Log.Error(ex, $"Error in {GetType().Name}");
        }
      }
    }

    protected abstract bool InternalGetTxFrequency();
    protected abstract bool InternalGetRxFrequency();
    protected abstract void InternalSetRxFrequency(long frequency);
    protected abstract void InternalSetTxFrequency(long frequency);
    protected abstract void InternalSetRxMode(Slicer.Mode mode);
    protected abstract void InternalSetTxMode(Slicer.Mode mode);
    public abstract bool IsRunning();
    public abstract string GetStatusString();


    public virtual void Dispose()
    {
      Stop();
    }
  }
}
