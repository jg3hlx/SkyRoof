namespace SkyRoof
{
  public partial class AmsatReportDialog : Form
  {
    private Context ctx;
    private SatnogsDbSatellite Satellite;

    public AmsatReportDialog()
    {
      InitializeComponent();
    }

    public static void SendReport(Context ctx, SatnogsDbSatellite satellite)
    {
      var dialog = new AmsatReportDialog();
      dialog.ctx = ctx;
      dialog.Satellite = satellite;
      dialog.ShowDialog();
    }

    private void AmsatReportDialog_Load(object sender, EventArgs e)
    {
      comboBox1.Items.AddRange(Satellite.AmsatEntries.ToArray());
      comboBox1.SelectedIndex = 0;

      if (Satellite.norad_cat_id == 25544)
        comboBox2.Items.AddRange(["Heard", "Telemetry Only", "Not Heard", "Crew Active"]);
      else
        comboBox2.Items.AddRange(["Heard", "Telemetry Only", "Not Heard"]);

      comboBox2.SelectedIndex = 0;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      string? errorMessage = ctx.AmsatStatusLoader.SendAmsatStatus(comboBox1.Text, comboBox2.Text);
      if (errorMessage != null) 
        MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
 
      Close();
    }
  }
}
