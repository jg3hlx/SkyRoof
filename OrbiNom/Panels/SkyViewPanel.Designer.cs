namespace OrbiNom
{
  partial class SkyViewPanel
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
      RadioButtonsPanel = new FlowLayoutPanel();
      RealTimeRadioBtn = new RadioButton();
      OrbitRadioBtn = new RadioButton();
      DrawPanel = new Panel();
      contextMenuStrip1 = new ContextMenuStrip(components);
      SelectSatelliteMNU = new ToolStripMenuItem();
      SatelliteDetailsMNU = new ToolStripMenuItem();
      SatelliteTransmittersMNU = new ToolStripMenuItem();
      toolTip1 = new ToolTip(components);
      FlowPanel = new FlowLayoutPanel();
      RadioButtonsPanel.SuspendLayout();
      contextMenuStrip1.SuspendLayout();
      SuspendLayout();
      // 
      // RadioButtonsPanel
      // 
      RadioButtonsPanel.AutoSize = true;
      RadioButtonsPanel.Controls.Add(RealTimeRadioBtn);
      RadioButtonsPanel.Controls.Add(OrbitRadioBtn);
      RadioButtonsPanel.Dock = DockStyle.Top;
      RadioButtonsPanel.Location = new Point(0, 0);
      RadioButtonsPanel.MaximumSize = new Size(0, 60);
      RadioButtonsPanel.Name = "RadioButtonsPanel";
      RadioButtonsPanel.Size = new Size(314, 25);
      RadioButtonsPanel.TabIndex = 1;
      // 
      // RealTimeRadioBtn
      // 
      RealTimeRadioBtn.AutoSize = true;
      RealTimeRadioBtn.BackColor = Color.FromArgb(230, 249, 255);
      RealTimeRadioBtn.Checked = true;
      RealTimeRadioBtn.Location = new Point(3, 3);
      RealTimeRadioBtn.Name = "RealTimeRadioBtn";
      RealTimeRadioBtn.Size = new Size(76, 19);
      RealTimeRadioBtn.TabIndex = 0;
      RealTimeRadioBtn.TabStop = true;
      RealTimeRadioBtn.Text = "Real Time";
      RealTimeRadioBtn.UseVisualStyleBackColor = false;
      RealTimeRadioBtn.CheckedChanged += radioButton_CheckedChanged;
      // 
      // OrbitRadioBtn
      // 
      OrbitRadioBtn.AutoSize = true;
      OrbitRadioBtn.BackColor = Color.FromArgb(224, 224, 224);
      OrbitRadioBtn.Enabled = false;
      OrbitRadioBtn.Location = new Point(85, 3);
      OrbitRadioBtn.Name = "OrbitRadioBtn";
      OrbitRadioBtn.Size = new Size(99, 19);
      OrbitRadioBtn.TabIndex = 1;
      OrbitRadioBtn.Text = "Selected Orbit";
      OrbitRadioBtn.UseVisualStyleBackColor = false;
      OrbitRadioBtn.CheckedChanged += radioButton_CheckedChanged;
      // 
      // DrawPanel
      // 
      DrawPanel.ContextMenuStrip = contextMenuStrip1;
      DrawPanel.Dock = DockStyle.Fill;
      DrawPanel.Font = new Font("Segoe UI", 11F);
      DrawPanel.Location = new Point(0, 25);
      DrawPanel.Name = "DrawPanel";
      DrawPanel.Size = new Size(314, 270);
      DrawPanel.TabIndex = 3;
      DrawPanel.Paint += panel_Paint;
      DrawPanel.DoubleClick += DrawPanel_DoubleClick;
      DrawPanel.MouseDown += DrawPanel_MouseDown;
      DrawPanel.MouseLeave += DrawPanel_MouseLeave;
      DrawPanel.MouseMove += DrawPanel_MouseMove;
      DrawPanel.Resize += panel_Resize;
      // 
      // contextMenuStrip1
      // 
      contextMenuStrip1.Items.AddRange(new ToolStripItem[] { SelectSatelliteMNU, SatelliteDetailsMNU, SatelliteTransmittersMNU });
      contextMenuStrip1.Name = "contextMenuStrip1";
      contextMenuStrip1.Size = new Size(206, 70);
      contextMenuStrip1.Opening += contextMenuStrip1_Opening;
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
      // FlowPanel
      // 
      FlowPanel.AutoSize = true;
      FlowPanel.BackColor = Color.White;
      FlowPanel.Dock = DockStyle.Top;
      FlowPanel.Location = new Point(0, 25);
      FlowPanel.MaximumSize = new Size(0, 60);
      FlowPanel.Name = "FlowPanel";
      FlowPanel.Size = new Size(314, 0);
      FlowPanel.TabIndex = 6;
      // 
      // SkyViewPanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(314, 295);
      Controls.Add(DrawPanel);
      Controls.Add(FlowPanel);
      Controls.Add(RadioButtonsPanel);
      Name = "SkyViewPanel";
      Text = "Sky View";
      FormClosing += SkyViewPanel_FormClosing;
      Load += SkyViewPanel_Load;
      RadioButtonsPanel.ResumeLayout(false);
      RadioButtonsPanel.PerformLayout();
      contextMenuStrip1.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private FlowLayoutPanel RadioButtonsPanel;
    private RadioButton RealTimeRadioBtn;
    private RadioButton OrbitRadioBtn;
    private Panel DrawPanel;
    private ToolTip toolTip1;
    private FlowLayoutPanel FlowPanel;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem SelectSatelliteMNU;
    private ToolStripMenuItem SatelliteDetailsMNU;
    private ToolStripMenuItem SatelliteTransmittersMNU;
  }
}