using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VE3NEA;

namespace OrbiNom
{
  public partial class SdrDevicesDialog : Form
  {
    private readonly Context ctx;
    private readonly Image RadioOnImage, RadioOffImage;
    public List<SoapySdrDeviceInfo> Devices;

    public SdrDevicesDialog()
    {
      InitializeComponent();
    }

    public SdrDevicesDialog(Context ctx)
    {
      InitializeComponent();
      this.ctx = ctx;
      BuildDeviceList();

      using (var ms = new MemoryStream(Properties.Resources.radio_button_on)) { RadioOnImage = Image.FromStream(ms); }
      using (var ms = new MemoryStream(Properties.Resources.radio_button_off)) { RadioOffImage = Image.FromStream(ms); }
    }

    private void BuildDeviceList()
    {
      Devices = Utils.DeepClone(ctx.Settings.Sdr.Devices);
      var oldNames = Devices.Select(x => x.Name).ToList();

      var presentDevices = SoapySdr.EnumerateDevices();
      var presentNames = presentDevices.Select(x => x.Name).ToList();

      foreach (var dev in Devices) dev.Present = presentNames.Contains(dev.Name);
      Devices.AddRange(presentDevices.Where(dev => !oldNames.Contains(dev.Name)));

      listBox1.Items.Clear();
      listBox1.Items.AddRange(Devices.ToArray());

      int index = Devices.FindIndex(dev => dev.Name == ctx.Settings.Sdr.SelectedDeviceName);
      if (index == -1) index = 0;
      if (listBox1.Items.Count > index) listBox1.SelectedIndex = index;
    }

    private void ResetMNU_Click(object sender, EventArgs e)
    {
      Grid.ResetSelectedProperty();
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      var sdr = (SoapySdrDeviceInfo)listBox1.Items[listBox1.SelectedIndex];
      Grid.SelectedObject = sdr.Properties;
      listBox1.Invalidate();
    }

    private void SdrDevicesDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.OK)
      {
        ctx.Settings.Sdr.Devices = Devices;
        string? selectedName = Devices.Count >= 0 ? Devices[listBox1.SelectedIndex].Name : null;
        ctx.Settings.Sdr.SelectedDeviceName = selectedName;
      }
    }

    private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
    {
      var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
      Brush brush = selected ? Brushes.LightBlue : Brushes.White;
      e.Graphics.FillRectangle(brush, e.Bounds);

      var image = e.Index == listBox1.SelectedIndex ? RadioOnImage : RadioOffImage;
      e.Graphics.DrawImage(image, e.Bounds.Left, e.Bounds.Top);

      brush = Devices[e.Index].Present ? Brushes.Black : Brushes.Gray;
      e.Graphics.DrawString(Devices[e.Index].Name, listBox1.Font, brush, e.Bounds.Left + image.Width, e.Bounds.Top);
    }

    private void listBox1_MouseDown(object sender, MouseEventArgs e)
    {
      listBox1.Invalidate();
    }
  }
}
