using System.ComponentModel;

namespace SkyRoof
{
  public class UserSettings
  {
    [DisplayName("コールサイン")]
    [Description("あなたのコールサイン")]
    public string Call { get; set; } = "";

    [DisplayName("グリッド・ロケーター")]
    [Description("あなたのグリッド・ロケーター")]
    public string Square { get; set; } = "";

    [DisplayName("高度")]
    [Description("あなたの海面からの高度、メートル単位")]
    public int Altitude { get; set; } = 0;

    public override string ToString() { return ""; }
  }
}