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
      flowLayoutPanel1 = new FlowLayoutPanel();
      RealTimeRadioBtn = new RadioButton();
      PassRadioBtn = new RadioButton();
      panel = new Panel();
      flowLayoutPanel1.SuspendLayout();
      SuspendLayout();
      // 
      // flowLayoutPanel1
      // 
      flowLayoutPanel1.Controls.Add(RealTimeRadioBtn);
      flowLayoutPanel1.Controls.Add(PassRadioBtn);
      flowLayoutPanel1.Dock = DockStyle.Top;
      flowLayoutPanel1.Location = new Point(0, 0);
      flowLayoutPanel1.Name = "flowLayoutPanel1";
      flowLayoutPanel1.Size = new Size(800, 27);
      flowLayoutPanel1.TabIndex = 1;
      // 
      // RealTimeRadioBtn
      // 
      RealTimeRadioBtn.AutoSize = true;
      RealTimeRadioBtn.Checked = true;
      RealTimeRadioBtn.Location = new Point(3, 3);
      RealTimeRadioBtn.Name = "RealTimeRadioBtn";
      RealTimeRadioBtn.Size = new Size(76, 19);
      RealTimeRadioBtn.TabIndex = 0;
      RealTimeRadioBtn.TabStop = true;
      RealTimeRadioBtn.Text = "Real Time";
      RealTimeRadioBtn.UseVisualStyleBackColor = true;
      RealTimeRadioBtn.CheckedChanged += radioButton_CheckedChanged;
      // 
      // PassRadioBtn
      // 
      PassRadioBtn.AutoSize = true;
      PassRadioBtn.Enabled = false;
      PassRadioBtn.Location = new Point(85, 3);
      PassRadioBtn.Name = "PassRadioBtn";
      PassRadioBtn.Size = new Size(99, 19);
      PassRadioBtn.TabIndex = 1;
      PassRadioBtn.Text = "Selected Orbit";
      PassRadioBtn.UseVisualStyleBackColor = true;
      PassRadioBtn.CheckedChanged += radioButton_CheckedChanged;
      // 
      // panel
      // 
      panel.Dock = DockStyle.Fill;
      panel.Font = new Font("Segoe UI", 11F);
      panel.Location = new Point(0, 27);
      panel.Name = "panel";
      panel.Size = new Size(800, 423);
      panel.TabIndex = 2;
      panel.Paint += panel_Paint;
      panel.Resize += panel_Resize;
      // 
      // SkyViewPanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(panel);
      Controls.Add(flowLayoutPanel1);
      Name = "SkyViewPanel";
      Text = "Sky View";
      FormClosing += SkyViewPanel_FormClosing;
      flowLayoutPanel1.ResumeLayout(false);
      flowLayoutPanel1.PerformLayout();
      ResumeLayout(false);
    }

    #endregion

    private FlowLayoutPanel flowLayoutPanel1;
    private RadioButton RealTimeRadioBtn;
    private RadioButton PassRadioBtn;
    private Panel panel;
  }
}