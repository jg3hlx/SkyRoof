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
      bool crossband = ctx.FrequencyControl.RadioLink.IsCrossBand;

      Rx?.Dispose();
      Tx?.Dispose();

      // create rx cat engine
      if (!ctx.Settings.RxCat.Enabled)
        Rx = null;
      else 
        Rx = new CatEngine(ctx.Settings.RxCat);

      // create tx cat engine
      if (!ctx.Settings.TxCat.Enabled || !ctx.FrequencyControl.RadioLink.HasUplink)
        Tx = null;
      else if (IsSameEngine(ctx.Settings.TxCat, ctx.Settings.RxCat))
        Tx = Rx;      
      else
        Tx = new CatEngine(ctx.Settings.TxCat);

      // start engines
      if (Rx != null)
      {
        Rx.RxTuned += (s, e) => ctx.FrequencyControl.RxTuned();
        Rx.StatusChanged += (s, e) => ctx.MainForm.ShowCatStatus();
        Rx.Start(true, Tx == Rx, crossband);
      }
      if (Tx != null)
      {
        Tx.TxTuned += (s, e) => ctx.FrequencyControl.TxTuned();
        if (Tx != Rx)
        {
          Tx.StatusChanged += (s, e) => ctx.MainForm.ShowCatStatus();
          Tx.Start(false, true, crossband);
        }
      }

      ctx.MainForm.ShowCatStatus();
    }

    private bool IsSameEngine(CatSettings txCat, CatSettings rxCat)
    {
      return ctx.Settings.RxCat.Enabled && ctx.Settings.TxCat.Enabled &&      
        IsSameHost(ctx.Settings.RxCat.Host, ctx.Settings.TxCat.Host) &&          
        ctx.Settings.RxCat.Port == ctx.Settings.TxCat.Port &&          
        ctx.Settings.RxCat.RadioModel == ctx.Settings.TxCat.RadioModel;
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
