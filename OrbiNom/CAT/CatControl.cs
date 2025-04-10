using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbiNom
{
  public class CatControl
  {
    public Context ctx;
    public CatEngine? Rx, Tx;


    internal void Setup()
    {
      Rx?.Dispose();
      Tx?.Dispose();

      // create rx cat engine
      if (!ctx.Settings.RxCat.Enabled)
        Rx = null;
      else if (ctx.Settings.RxCat.EngineType == EngineType.OmniRig)
        Rx = CatEngine.CreateEngine(ctx.Settings.RxCat.OmniRig);
      else
        Rx = CatEngine.CreateEngine(ctx.Settings.RxCat.Rigctld);

      // create tx cat engine
      if (!ctx.Settings.TxCat.Enabled || !ctx.FrequencyControl.HasUplink)
        Tx = null;
      else if (IsSameEngine(ctx.Settings.TxCat, ctx.Settings.RxCat))
        Tx = Rx;
      else if (ctx.Settings.TxCat.EngineType == EngineType.OmniRig)
        Tx = CatEngine.CreateEngine(ctx.Settings.TxCat.OmniRig);
      else
        Tx = CatEngine.CreateEngine(ctx.Settings.TxCat.Rigctld);

      // start engines
      if (Rx != null)
      {
        Rx.RxTuned += (s, e) => ctx.FrequencyControl.RxTuned();
        Rx.StatusChanged += (s, e) => ctx.MainForm.ShowCatStatus();
        Rx.Start(true, Tx == Rx);
      }
      if (Tx != null)
      {
        Tx.TxTuned += (s, e) => ctx.FrequencyControl.TxTuned();
        if (Tx != Rx)
        {
          Tx.StatusChanged += (s, e) => ctx.MainForm.ShowCatStatus();
          Tx.Start(false, true);
        }
      }

      ctx.MainForm.ShowCatStatus();
    }

    private bool IsSameEngine(CatSettings txCat, CatSettings rxCat)
    {
      if (!ctx.Settings.RxCat.Enabled || !ctx.Settings.TxCat.Enabled)
        return false;

      if (ctx.Settings.RxCat.EngineType != ctx.Settings.TxCat.EngineType)
        return false;

      if (ctx.Settings.RxCat.EngineType == EngineType.OmniRig)
        return ctx.Settings.RxCat.OmniRig.RigNo == ctx.Settings.TxCat.OmniRig.RigNo;
      else
        return IsSameHost(ctx.Settings.RxCat.Rigctld.Host, ctx.Settings.TxCat.Rigctld.Host) &&
          ctx.Settings.RxCat.Rigctld.Port == ctx.Settings.TxCat.Rigctld.Port;
    }

    private bool IsSameHost(string host1, string host2)
    {
      if (string.IsNullOrEmpty(host1) || string.IsNullOrEmpty(host2))
        return false;

      if (host1 == "127.0.0.1") host1 = "localhost";
      if (host2 == "127.0.0.1") host2 = "localhost";

      return string.Equals(host1, host2, StringComparison.OrdinalIgnoreCase);
    }
  }
}
