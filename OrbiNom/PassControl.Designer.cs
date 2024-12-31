namespace OrbiNom
{
  partial class PassControl
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      panel1 = new Panel();
      SatNameLabel = new Label();
      label2 = new Label();
      label3 = new Label();
      label4 = new Label();
      SuspendLayout();
      // 
      // panel1
      // 
      panel1.BackColor = Color.PowderBlue;
      panel1.Dock = DockStyle.Right;
      panel1.Location = new Point(263, 0);
      panel1.Name = "panel1";
      panel1.Size = new Size(37, 37);
      panel1.TabIndex = 0;
      // 
      // SatNameLabel
      // 
      SatNameLabel.AutoSize = true;
      SatNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
      SatNameLabel.Location = new Point(3, 2);
      SatNameLabel.Name = "SatNameLabel";
      SatNameLabel.Size = new Size(64, 15);
      SatNameLabel.TabIndex = 1;
      SatNameLabel.Text = "SONATE-2";
      // 
      // label2
      // 
      label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      label2.AutoSize = true;
      label2.Location = new Point(175, 2);
      label2.Name = "label2";
      label2.Size = new Size(88, 15);
      label2.TabIndex = 2;
      label2.Text = "in 48h 21m 15s ";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(3, 19);
      label3.Name = "label3";
      label3.Size = new Size(178, 15);
      label3.TabIndex = 3;
      label3.Text = "2024-12-29  12:34:56  to  12:49:08";
      // 
      // label4
      // 
      label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      label4.AutoSize = true;
      label4.Location = new Point(197, 19);
      label4.Name = "label4";
      label4.Size = new Size(66, 15);
      label4.TabIndex = 4;
      label4.Text = "11min   57°";
      // 
      // PassControl
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = SystemColors.Window;
      BorderStyle = BorderStyle.FixedSingle;
      Controls.Add(label4);
      Controls.Add(label3);
      Controls.Add(label2);
      Controls.Add(SatNameLabel);
      Controls.Add(panel1);
      Name = "PassControl";
      Size = new Size(300, 37);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Panel panel1;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    public Label SatNameLabel;
  }
}
