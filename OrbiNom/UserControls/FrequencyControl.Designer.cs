namespace OrbiNom.UserControls
{
  partial class FrequencyControl
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
      SatelliteLabel = new Label();
      FrequencyLabel = new Label();
      DopplerCheckbox = new CheckBox();
      ManualCheckbox = new CheckBox();
      DopplerUpDown = new NumericUpDown();
      ManualUpDown = new NumericUpDown();
      label2 = new Label();
      ((System.ComponentModel.ISupportInitialize)DopplerUpDown).BeginInit();
      ((System.ComponentModel.ISupportInitialize)ManualUpDown).BeginInit();
      SuspendLayout();
      // 
      // SatelliteLabel
      // 
      SatelliteLabel.AutoEllipsis = true;
      SatelliteLabel.Location = new Point(21, 5);
      SatelliteLabel.Name = "SatelliteLabel";
      SatelliteLabel.Size = new Size(156, 15);
      SatelliteLabel.TabIndex = 0;
      SatelliteLabel.Text = "SONATE-2";
      // 
      // FrequencyLabel
      // 
      FrequencyLabel.BackColor = Color.Black;
      FrequencyLabel.Font = new Font("Microsoft Sans Serif", 16F);
      FrequencyLabel.ForeColor = Color.Aqua;
      FrequencyLabel.Location = new Point(7, 23);
      FrequencyLabel.Name = "FrequencyLabel";
      FrequencyLabel.Size = new Size(170, 34);
      FrequencyLabel.TabIndex = 2;
      FrequencyLabel.Text = "435,000,000";
      FrequencyLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // DopplerCheckbox
      // 
      DopplerCheckbox.AutoSize = true;
      DopplerCheckbox.Location = new Point(190, 9);
      DopplerCheckbox.Name = "DopplerCheckbox";
      DopplerCheckbox.Size = new Size(68, 19);
      DopplerCheckbox.TabIndex = 3;
      DopplerCheckbox.Text = "Doppler";
      DopplerCheckbox.UseVisualStyleBackColor = true;
      // 
      // ManualCheckbox
      // 
      ManualCheckbox.AutoSize = true;
      ManualCheckbox.Location = new Point(190, 38);
      ManualCheckbox.Name = "ManualCheckbox";
      ManualCheckbox.Size = new Size(66, 19);
      ManualCheckbox.TabIndex = 4;
      ManualCheckbox.Text = "Manual";
      ManualCheckbox.UseVisualStyleBackColor = true;
      // 
      // DopplerUpDown
      // 
      DopplerUpDown.DecimalPlaces = 3;
      DopplerUpDown.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
      DopplerUpDown.Location = new Point(264, 8);
      DopplerUpDown.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      DopplerUpDown.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      DopplerUpDown.Name = "DopplerUpDown";
      DopplerUpDown.Size = new Size(68, 23);
      DopplerUpDown.TabIndex = 5;
      DopplerUpDown.Value = new decimal(new int[] { 20, 0, 0, 0 });
      // 
      // ManualUpDown
      // 
      ManualUpDown.DecimalPlaces = 3;
      ManualUpDown.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
      ManualUpDown.Location = new Point(264, 37);
      ManualUpDown.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      ManualUpDown.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      ManualUpDown.Name = "ManualUpDown";
      ManualUpDown.Size = new Size(68, 23);
      ManualUpDown.TabIndex = 6;
      ManualUpDown.Value = new decimal(new int[] { 20, 0, 0, int.MinValue });
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Font = new Font("Source Code Pro", 14F, FontStyle.Bold);
      label2.Location = new Point(3, -2);
      label2.Name = "label2";
      label2.Size = new Size(21, 24);
      label2.TabIndex = 7;
      label2.Text = "↓";
      // 
      // FrequencyControl
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BorderStyle = BorderStyle.FixedSingle;
      Controls.Add(label2);
      Controls.Add(ManualUpDown);
      Controls.Add(DopplerUpDown);
      Controls.Add(ManualCheckbox);
      Controls.Add(DopplerCheckbox);
      Controls.Add(FrequencyLabel);
      Controls.Add(SatelliteLabel);
      Name = "FrequencyControl";
      Size = new Size(342, 66);
      ((System.ComponentModel.ISupportInitialize)DopplerUpDown).EndInit();
      ((System.ComponentModel.ISupportInitialize)ManualUpDown).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Label SatelliteLabel;
    private Label FrequencyLabel;
    private CheckBox DopplerCheckbox;
    private CheckBox ManualCheckbox;
    private NumericUpDown DopplerUpDown;
    private NumericUpDown ManualUpDown;
    private Label label2;
  }
}
