namespace SkyRoof
{
  public static class RigCtldCommands
  {
    public static string[] setup_duplex = ["U SATMODE 1"];
    public static string[] setup_split = ["U SATMODE 0", "U DUAL_WATCH 0", "V VFOA", "S 1 VFOB", "V VFOA"];
    public static string[] setup_simplex = ["U SATMODE 0", "U DUAL_WATCH 0", "V VFOA", "S 0 VFOB", "V VFOA"];
    public static string read_rx_frequency = "f";
    public static string read_tx_frequency = "i";
    public static string write_rx_frequency = "F {frequency}";
    public static string write_tx_frequency = "I {frequency}";
    public static string write_rx_mode = "M {mode} 0";
    public static string write_tx_mode = "X {mode} 0";
    public static string read_ptt = "t";
    public static string set_ptt_on = "T 1";
    public static string set_ptt_off = "T 0";
  }
}
