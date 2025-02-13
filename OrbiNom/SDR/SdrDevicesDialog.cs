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
    private SoapySdrDeviceInfo Sdr;

    public SdrDevicesDialog()
    {
      InitializeComponent();
    }
    private void Grid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
    {
      string helpKeyword = PropertyGridEx.GetItemProperty(e.ChangedItem, "HelpKeyword");

      switch (helpKeyword)
      {
        case "JTSkimmer.AirspySettings.VgaGain":
        case "JTSkimmer.AirspySettings.MixerGain":
          ValidateGain(e, 15);
          break;

        case "JTSkimmer.AirspySettings.LnaGain":
          ValidateGain(e, 14);
          break;
      }
    }


    private void ValidateGain(PropertyValueChangedEventArgs e, byte maxValue)
    {
      //      // ensure that the value is in range
      //      if ((byte)e.ChangedItem.Value > maxValue)
      //        e.ChangedItem.PropertyDescriptor.SetValue(e.ChangedItem.Parent.Value, maxValue);
      //
      //      // only in custom gain mode
      //      var sett = (AirspySettings)Grid.SelectedObject;
      //      if (sett.GainMode != AirspyGainMode.Custom)
      //      {
      //        e.ChangedItem.PropertyDescriptor.SetValue(e.ChangedItem.Parent.Value, e.OldValue);
      //        MessageBox.Show($"This setting has effect only when Gain Mode is set to Custom", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      //      }
    }

    private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Grid.ResetSelectedProperty();
    }

    private void ShowSdrList()
    {
      listBox1.Items.Clear();
      listBox1.Items.AddRange(SoapySdr.EnumerateDevices());
      if (listBox1.Items.Count > 0) listBox1.SelectedIndex = 0;
    }

    private void SdrDevicesDialog_Load(object sender, EventArgs e)
    {
      ShowSdrList();
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      Sdr = (SoapySdrDeviceInfo)listBox1.Items[listBox1.SelectedIndex];
      Grid.SelectedObject = Sdr.Properties;
    }

    private void SdrDevicesDialog_Shown(object sender, EventArgs e)
    {
    }
  }
}
