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
      flowLayoutPanel1 = new FlowLayoutPanel();
      RealTimeRadioBtn = new RadioButton();
      OrbitRadioBtn = new RadioButton();
      DrawPanel = new Panel();
      toolTip1 = new ToolTip(components);
      toolTip2 = new ToolTip(components);
      FlowPanel = new FlowLayoutPanel();
      flowLayoutPanel1.SuspendLayout();
      SuspendLayout();
      // 
      // flowLayoutPanel1
      // 
      flowLayoutPanel1.AutoSize = true;
      flowLayoutPanel1.Controls.Add(RealTimeRadioBtn);
      flowLayoutPanel1.Controls.Add(OrbitRadioBtn);
      flowLayoutPanel1.Dock = DockStyle.Top;
      flowLayoutPanel1.Location = new Point(0, 0);
      flowLayoutPanel1.MaximumSize = new Size(0, 60);
      flowLayoutPanel1.Name = "flowLayoutPanel1";
      flowLayoutPanel1.Size = new Size(800, 25);
      flowLayoutPanel1.TabIndex = 1;
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
      DrawPanel.Dock = DockStyle.Fill;
      DrawPanel.Font = new Font("Segoe UI", 11F);
      DrawPanel.Location = new Point(0, 25);
      DrawPanel.Name = "DrawPanel";
      DrawPanel.Size = new Size(800, 425);
      DrawPanel.TabIndex = 3;
      DrawPanel.Paint += panel_Paint;
      DrawPanel.MouseDown += DrawPanel_MouseDown;
      DrawPanel.MouseLeave += DrawPanel_MouseLeave;
      DrawPanel.MouseMove += DrawPanel_MouseMove;
      DrawPanel.Resize += panel_Resize;
      // 
      // FlowPanel
      // 
      FlowPanel.AutoSize = true;
      FlowPanel.BackColor = Color.White;
      FlowPanel.Dock = DockStyle.Top;
      FlowPanel.Location = new Point(0, 25);
      FlowPanel.MaximumSize = new Size(0, 60);
      FlowPanel.Name = "FlowPanel";
      FlowPanel.Size = new Size(800, 0);
      FlowPanel.TabIndex = 6;
      // 
      // SkyViewPanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(DrawPanel);
      Controls.Add(FlowPanel);
      Controls.Add(flowLayoutPanel1);
      Name = "SkyViewPanel";
      Text = "Sky View";
      FormClosing += SkyViewPanel_FormClosing;
      flowLayoutPanel1.ResumeLayout(false);
      flowLayoutPanel1.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private FlowLayoutPanel flowLayoutPanel1;
    private RadioButton RealTimeRadioBtn;
    private RadioButton OrbitRadioBtn;
    private Panel DrawPanel;
    private ToolTip toolTip1;
    private ToolTip toolTip2;
    private FlowLayoutPanel FlowPanel;
  }
}