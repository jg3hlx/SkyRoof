using VE3NEA;

namespace OrbiNom
{
  internal partial class UserDetailsDialog : Form
  {
    private Context ctx;

    internal UserDetailsDialog()
    {
      InitializeComponent();
    }

    internal UserDetailsDialog(Context ctx)
    {
      InitializeComponent();

      this.ctx = ctx;
      LoadSettings();
      ValidateAll();
    }

    internal static bool UserDetailsAvailable(Context ctx)
    {
      var sett = ctx.Settings.User;
      return !string.IsNullOrEmpty(sett.Call) && !string.IsNullOrEmpty(sett.Square);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      SaveSettings();
    }

    private void LoadSettings()
    {
      textBox1.Text = ctx.Settings.User.Call;
      if (ctx.Settings.User.Square != "JJ00jj") textBox2.Text = ctx.Settings.User.Square;
    }

    private void SaveSettings()
    {
      ctx.Settings.User.Call = textBox1.Text.Trim();
      ctx.Settings.User.Square = textBox2.Text.Trim();
    }

    private void textBoxes_TextChanged(object sender, EventArgs e)
    {
      ValidateAll();
    }

    private void ValidateAll()
    {
      bool allOk = true;

      // call
      bool ok = Utils.CallsignRegex.IsMatch(textBox1.Text.Trim());
      label2.ForeColor = ok ? SystemColors.WindowText : Color.Red;
      allOk = allOk && ok;

      // square
      ok = textBox2.Text.Length == 6 && GridSquare.IsValid(textBox2.Text.Trim());
      label3.ForeColor = ok ? SystemColors.WindowText : Color.Red;
      allOk = allOk && ok;

      okBtn.Enabled = allOk;
    }
  }
}