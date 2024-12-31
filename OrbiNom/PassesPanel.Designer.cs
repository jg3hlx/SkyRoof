namespace OrbiNom
{
  partial class PassesPanel
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
      radioButton1 = new RadioButton();
      radioButton2 = new RadioButton();
      radioButton3 = new RadioButton();
      panel1 = new Panel();
      flowLayoutPanel1.SuspendLayout();
      SuspendLayout();
      // 
      // flowLayoutPanel1
      // 
      flowLayoutPanel1.Controls.Add(radioButton1);
      flowLayoutPanel1.Controls.Add(radioButton2);
      flowLayoutPanel1.Controls.Add(radioButton3);
      flowLayoutPanel1.Dock = DockStyle.Top;
      flowLayoutPanel1.Location = new Point(0, 0);
      flowLayoutPanel1.Name = "flowLayoutPanel1";
      flowLayoutPanel1.Size = new Size(434, 27);
      flowLayoutPanel1.TabIndex = 0;
      // 
      // radioButton1
      // 
      radioButton1.AutoSize = true;
      radioButton1.Checked = true;
      radioButton1.Location = new Point(3, 3);
      radioButton1.Name = "radioButton1";
      radioButton1.Size = new Size(109, 19);
      radioButton1.TabIndex = 0;
      radioButton1.TabStop = true;
      radioButton1.Text = "Current Satellite";
      radioButton1.UseVisualStyleBackColor = true;
      // 
      // radioButton2
      // 
      radioButton2.AutoSize = true;
      radioButton2.Location = new Point(118, 3);
      radioButton2.Name = "radioButton2";
      radioButton2.Size = new Size(58, 19);
      radioButton2.TabIndex = 1;
      radioButton2.Text = "Group";
      radioButton2.UseVisualStyleBackColor = true;
      // 
      // radioButton3
      // 
      radioButton3.AutoSize = true;
      radioButton3.Location = new Point(182, 3);
      radioButton3.Name = "radioButton3";
      radioButton3.Size = new Size(39, 19);
      radioButton3.TabIndex = 2;
      radioButton3.Text = "All";
      radioButton3.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      panel1.AutoScroll = true;
      panel1.Dock = DockStyle.Fill;
      panel1.Location = new Point(0, 27);
      panel1.Name = "panel1";
      panel1.Size = new Size(434, 582);
      panel1.TabIndex = 1;
      // 
      // PassesPanel
      // 
      AutoScaleMode = AutoScaleMode.Inherit;
      ClientSize = new Size(434, 609);
      Controls.Add(panel1);
      Controls.Add(flowLayoutPanel1);
      Name = "PassesPanel";
      Text = "Satellite Passes";
      FormClosing += PassesPanel_FormClosing;
      flowLayoutPanel1.ResumeLayout(false);
      flowLayoutPanel1.PerformLayout();
      ResumeLayout(false);
    }

    #endregion

    private FlowLayoutPanel flowLayoutPanel1;
    private RadioButton radioButton1;
    private RadioButton radioButton2;
    private Panel panel1;
    private RadioButton radioButton3;
  }
}