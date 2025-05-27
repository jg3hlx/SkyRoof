namespace SkyRoof
{
  public class CatControl
  {
    public Context ctx;
    public CatControlEngine? Rx, Tx;


    internal void ApplySettings()
    {
      bool crossband = ctx.FrequencyControl.RadioLink.IsCrossBand;

      Rx?.Dispose();
      Tx?.Dispose();

      // create rx cat engine
      if (!ctx.Settings.Cat.RxCat.Enabled)
        Rx = null;
      else 
        Rx = new CatControlEngine(ctx.Settings.Cat.RxCat, ctx.Settings.Cat);

      // create tx cat engine
      if (!ctx.Settings.Cat.TxCat.Enabled || !ctx.FrequencyControl.RadioLink.HasUplink)
        Tx = null;
      else if (IsSameEngine(ctx.Settings.Cat.TxCat, ctx.Settings.Cat.RxCat))
        Tx = Rx;      
      else
        Tx = new CatControlEngine(ctx.Settings.Cat.TxCat, ctx.Settings.Cat);

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

    private bool IsSameEngine(CatRadioSettings txCat, CatRadioSettings rxCat)
    {
      return rxCat.Enabled 
        && txCat.Enabled 
        && IsSameHost(rxCat.Host, txCat.Host) 
        && rxCat.Port == txCat.Port 
        && rxCat.RadioType == txCat.RadioType;
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
