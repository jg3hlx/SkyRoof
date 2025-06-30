using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;
using WeifenLuo.WinFormsUI.Docking;

namespace SkyRoof
{
  public partial class QsoEntryPanel : DockContent
  {
    private Context ctx;

    public QsoEntryPanel()
    {
      InitializeComponent();
    }

    public QsoEntryPanel(Context ctx)
    {
      this.ctx = ctx;
      Log.Information("Creating QsoEntryPanel");
      InitializeComponent();

      ctx.QsoEntryPanel = this;
      ctx.MainForm.QsoEntryMNU.Checked = true;

      ApplySettings();
    }

    private void QsoEntryPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      Log.Information("Closing QsoEntryPanel");
      ctx.QsoEntryPanel = null;
      ctx.MainForm.QsoEntryMNU.Checked = false;
    }

    internal void ApplySettings()
    {
      ShowHideFields();
    }

    private void ShowHideFields()
    {
      var visibleFields = ctx.Settings.QsoEntry.Fields;

      foreach (var control in flowLayoutPanel1.Controls)
      {
        if (control is Panel panel)
          panel.Visible = panel.Name == "ButtonsPanel" || visibleFields.HasFlag((QsoFields)(1 << panel.TabIndex));
      }
    }
  }
}

