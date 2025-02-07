using System.Diagnostics;
using System.Reflection;
using VE3NEA;

namespace OrbiNom
{
  public partial class AboutBox : Form
  {
    public AboutBox()
    {
      InitializeComponent();
      Text = $"About {Utils.GetAppName()}";
      label1.Text = Utils.GetVersionString();
      label2.Text = Utils.GetCopyrightString();
    }

    private void WebsiteLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      WebsiteLabel.LinkVisited = true;
      Process.Start(new ProcessStartInfo("https://ve3nea.github.io/OrbiNom") { UseShellExecute = true });
    }
  }
}
