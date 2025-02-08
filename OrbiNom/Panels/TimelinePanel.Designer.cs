namespace OrbiNom
{
  partial class TimelinePanel
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      toolTip1 = new ToolTip(components);
      contextMenuStrip1 = new ContextMenuStrip(components);
      SelectSatelliteMNU = new ToolStripMenuItem();
      SatelliteDetailsMNU = new ToolStripMenuItem();
      SatelliteTransmittersMNU = new ToolStripMenuItem();
      contextMenuStrip1.SuspendLayout();
      SuspendLayout();
      // 
      // toolTip1
      // 
      toolTip1.AutomaticDelay = 1500;
      toolTip1.AutoPopDelay = 15000;
      toolTip1.InitialDelay = 500;
      toolTip1.ReshowDelay = 300;
      toolTip1.ShowAlways = true;
      // 
      // contextMenuStrip1
      // 
      contextMenuStrip1.Items.AddRange(new ToolStripItem[] { SelectSatelliteMNU, SatelliteDetailsMNU, SatelliteTransmittersMNU });
      contextMenuStrip1.Name = "contextMenuStrip1";
      contextMenuStrip1.Size = new Size(206, 70);
      // 
      // SelectSatelliteMNU
      // 
      SelectSatelliteMNU.Name = "SelectSatelliteMNU";
      SelectSatelliteMNU.ShortcutKeyDisplayString = "Dbl-Click";
      SelectSatelliteMNU.Size = new Size(205, 22);
      SelectSatelliteMNU.Text = "Select Satellite";
      SelectSatelliteMNU.Click += SelectSatelliteMNU_Click;
      // 
      // SatelliteDetailsMNU
      // 
      SatelliteDetailsMNU.Name = "SatelliteDetailsMNU";
      SatelliteDetailsMNU.Size = new Size(205, 22);
      SatelliteDetailsMNU.Text = "Satellite Details...";
      SatelliteDetailsMNU.Click += SatelliteDetailsMNU_Click;
      // 
      // SatelliteTransmittersMNU
      // 
      SatelliteTransmittersMNU.Name = "SatelliteTransmittersMNU";
      SatelliteTransmittersMNU.Size = new Size(205, 22);
      SatelliteTransmittersMNU.Text = "Satellite Transmitters...";
      SatelliteTransmittersMNU.Click += SatelliteTransmittersMNU_Click;
      // 
      // TimelinePanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(966, 185);
      ContextMenuStrip = contextMenuStrip1;
      DoubleBuffered = true;
      Name = "TimelinePanel";
      Text = "Timeline";
      FormClosing += TimelinePanel_FormClosing;
      Load += TimelinePanel_Load;
      Paint += TimelinePanel_Paint;
      DoubleClick += TimelinePanel_DoubleClick;
      MouseDown += SatelliteTimelineControl_MouseDown;
      MouseLeave += TimelinePanel_MouseLeave;
      MouseMove += SatelliteTimelineControl_MouseMove;
      MouseUp += SatelliteTimelineControl_MouseUp;
      Resize += TimelinePanel_Resize;
      contextMenuStrip1.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private ToolTip toolTip1;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem SelectSatelliteMNU;
    private ToolStripMenuItem SatelliteDetailsMNU;
    private ToolStripMenuItem SatelliteTransmittersMNU;
  }
}