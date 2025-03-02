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
      DownlinkFrequencyLabel = new Label();
      DopplerCheckbox = new CheckBox();
      ManualCheckbox = new CheckBox();
      DopplerUpDown = new NumericUpDown();
      ManualUpDown = new NumericUpDown();
      label2 = new Label();
      label1 = new Label();
      numericUpDown1 = new NumericUpDown();
      numericUpDown2 = new NumericUpDown();
      checkBox1 = new CheckBox();
      checkBox2 = new CheckBox();
      UplinkFrequencyLabel = new Label();
      UplinkLabel = new Label();
      ((System.ComponentModel.ISupportInitialize)DopplerUpDown).BeginInit();
      ((System.ComponentModel.ISupportInitialize)ManualUpDown).BeginInit();
      ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
      ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
      SuspendLayout();
      // 
      // SatelliteLabel
      // 
      SatelliteLabel.AutoEllipsis = true;
      SatelliteLabel.Location = new Point(21, 5);
      SatelliteLabel.Name = "SatelliteLabel";
      SatelliteLabel.Size = new Size(156, 15);
      SatelliteLabel.TabIndex = 0;
      SatelliteLabel.Text = "Downlink";
      // 
      // DownlinkFrequencyLabel
      // 
      DownlinkFrequencyLabel.BackColor = Color.Black;
      DownlinkFrequencyLabel.Font = new Font("Microsoft Sans Serif", 16F);
      DownlinkFrequencyLabel.ForeColor = Color.Aqua;
      DownlinkFrequencyLabel.Location = new Point(7, 23);
      DownlinkFrequencyLabel.Name = "DownlinkFrequencyLabel";
      DownlinkFrequencyLabel.Size = new Size(170, 34);
      DownlinkFrequencyLabel.TabIndex = 2;
      DownlinkFrequencyLabel.Text = "000,000,000";
      DownlinkFrequencyLabel.TextAlign = ContentAlignment.MiddleCenter;
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
      // label1
      // 
      label1.AutoSize = true;
      label1.Font = new Font("Source Code Pro", 14F, FontStyle.Bold);
      label1.Location = new Point(338, -2);
      label1.Name = "label1";
      label1.Size = new Size(21, 24);
      label1.TabIndex = 14;
      label1.Text = "↑";
      // 
      // numericUpDown1
      // 
      numericUpDown1.DecimalPlaces = 3;
      numericUpDown1.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
      numericUpDown1.Location = new Point(599, 37);
      numericUpDown1.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      numericUpDown1.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      numericUpDown1.Name = "numericUpDown1";
      numericUpDown1.Size = new Size(68, 23);
      numericUpDown1.TabIndex = 13;
      numericUpDown1.Value = new decimal(new int[] { 20, 0, 0, int.MinValue });
      // 
      // numericUpDown2
      // 
      numericUpDown2.DecimalPlaces = 3;
      numericUpDown2.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
      numericUpDown2.Location = new Point(599, 8);
      numericUpDown2.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      numericUpDown2.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      numericUpDown2.Name = "numericUpDown2";
      numericUpDown2.Size = new Size(68, 23);
      numericUpDown2.TabIndex = 12;
      numericUpDown2.Value = new decimal(new int[] { 20, 0, 0, 0 });
      // 
      // checkBox1
      // 
      checkBox1.AutoSize = true;
      checkBox1.Location = new Point(525, 38);
      checkBox1.Name = "checkBox1";
      checkBox1.Size = new Size(66, 19);
      checkBox1.TabIndex = 11;
      checkBox1.Text = "Manual";
      checkBox1.UseVisualStyleBackColor = true;
      // 
      // checkBox2
      // 
      checkBox2.AutoSize = true;
      checkBox2.Location = new Point(525, 9);
      checkBox2.Name = "checkBox2";
      checkBox2.Size = new Size(68, 19);
      checkBox2.TabIndex = 10;
      checkBox2.Text = "Doppler";
      checkBox2.UseVisualStyleBackColor = true;
      // 
      // UplinkFrequencyLabel
      // 
      UplinkFrequencyLabel.BackColor = Color.Black;
      UplinkFrequencyLabel.Font = new Font("Microsoft Sans Serif", 16F);
      UplinkFrequencyLabel.ForeColor = Color.Aqua;
      UplinkFrequencyLabel.Location = new Point(342, 23);
      UplinkFrequencyLabel.Name = "UplinkFrequencyLabel";
      UplinkFrequencyLabel.Size = new Size(170, 34);
      UplinkFrequencyLabel.TabIndex = 9;
      UplinkFrequencyLabel.Text = "000,000,000";
      UplinkFrequencyLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // UplinkLabel
      // 
      UplinkLabel.AutoEllipsis = true;
      UplinkLabel.Location = new Point(356, 5);
      UplinkLabel.Name = "UplinkLabel";
      UplinkLabel.Size = new Size(156, 15);
      UplinkLabel.TabIndex = 8;
      UplinkLabel.Text = "Uplink";
      // 
      // FrequencyControl
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BorderStyle = BorderStyle.FixedSingle;
      Controls.Add(label1);
      Controls.Add(numericUpDown1);
      Controls.Add(numericUpDown2);
      Controls.Add(checkBox1);
      Controls.Add(checkBox2);
      Controls.Add(UplinkFrequencyLabel);
      Controls.Add(UplinkLabel);
      Controls.Add(label2);
      Controls.Add(ManualUpDown);
      Controls.Add(DopplerUpDown);
      Controls.Add(ManualCheckbox);
      Controls.Add(DopplerCheckbox);
      Controls.Add(DownlinkFrequencyLabel);
      Controls.Add(SatelliteLabel);
      Name = "FrequencyControl";
      Size = new Size(679, 66);
      ((System.ComponentModel.ISupportInitialize)DopplerUpDown).EndInit();
      ((System.ComponentModel.ISupportInitialize)ManualUpDown).EndInit();
      ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
      ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Label SatelliteLabel;
    private Label DownlinkFrequencyLabel;
    private CheckBox DopplerCheckbox;
    private CheckBox ManualCheckbox;
    private NumericUpDown DopplerUpDown;
    private NumericUpDown ManualUpDown;
    private Label label2;
    private Label label1;
    private NumericUpDown numericUpDown1;
    private NumericUpDown numericUpDown2;
    private CheckBox checkBox1;
    private CheckBox checkBox2;
    private Label UplinkFrequencyLabel;
    private Label UplinkLabel;
  }
}
