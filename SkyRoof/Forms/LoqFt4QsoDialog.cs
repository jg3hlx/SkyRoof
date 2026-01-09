namespace SkyRoof
{
  public partial class LoqFt4QsoDialog : Form
  {
    private Context ctx;
    private QsoInfo qso;

    public LoqFt4QsoDialog()
    {
      InitializeComponent();
    }

    internal void PopUp(Context ctx, QsoInfo qso)
    {
      this.ctx = ctx;
      this.qso = qso;
      label1.Text = $"Save FT4 QSO with {qso.Call}?";
      Show(ctx.MainForm);
    }

    private void SaveBtn_Click(object sender, EventArgs e)
    {
      ctx.Ft4ConsolePanel?.WsjtxUdpSender?.SendLogQsoMessage(qso);
      ctx.LoggerInterface.SaveQso(qso);
      Hide();
    }

    private void EditBtn_Click(object sender, EventArgs e)
    {
      if (ctx.QsoEntryPanel == null)
      {
        ctx.MainForm.ShowFloatingPanel(new QsoEntryPanel(ctx));
        ctx.QsoEntryPanel!.ShouldClose = true;
      }
      Hide();
      ctx.QsoEntryPanel!.SetQsoInfo(qso);
      ctx.QsoEntryPanel!.Focus();
    }
    private void LoqFt4QsoDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
      Hide();
    }
  }
}
