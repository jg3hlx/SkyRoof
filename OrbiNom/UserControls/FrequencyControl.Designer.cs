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
      DownlinkManualSpinner = new NumericUpDown();
      label2 = new Label();
      label1 = new Label();
      UplinkManualSpinner = new NumericUpDown();
      checkBox1 = new CheckBox();
      checkBox2 = new CheckBox();
      UplinkFrequencyLabel = new Label();
      UplinkLabel = new Label();
      DownlinkDopplerLabel = new Label();
      UplinkDopplerLabel = new Label();
      ((System.ComponentModel.ISupportInitialize)DownlinkManualSpinner).BeginInit();
      ((System.ComponentModel.ISupportInitialize)UplinkManualSpinner).BeginInit();
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
      DopplerCheckbox.Location = new Point(190, 8);
      DopplerCheckbox.Name = "DopplerCheckbox";
      DopplerCheckbox.Size = new Size(68, 19);
      DopplerCheckbox.TabIndex = 3;
      DopplerCheckbox.Text = "Doppler";
      DopplerCheckbox.UseVisualStyleBackColor = true;
      // 
      // ManualCheckbox
      // 
      ManualCheckbox.AutoSize = true;
      ManualCheckbox.Location = new Point(190, 37);
      ManualCheckbox.Name = "ManualCheckbox";
      ManualCheckbox.Size = new Size(66, 19);
      ManualCheckbox.TabIndex = 4;
      ManualCheckbox.Text = "Manual";
      ManualCheckbox.UseVisualStyleBackColor = true;
      // 
      // DownlinkManualSpinner
      // 
      DownlinkManualSpinner.DecimalPlaces = 3;
      DownlinkManualSpinner.Font = new Font("Segoe UI", 10F);
      DownlinkManualSpinner.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
      DownlinkManualSpinner.Location = new Point(258, 35);
      DownlinkManualSpinner.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      DownlinkManualSpinner.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      DownlinkManualSpinner.Name = "DownlinkManualSpinner";
      DownlinkManualSpinner.Size = new Size(70, 25);
      DownlinkManualSpinner.TabIndex = 6;
      DownlinkManualSpinner.Value = new decimal(new int[] { 20, 0, 0, int.MinValue });
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
      // UplinkManualSpinner
      // 
      UplinkManualSpinner.DecimalPlaces = 3;
      UplinkManualSpinner.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
      UplinkManualSpinner.Location = new Point(599, 37);
      UplinkManualSpinner.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      UplinkManualSpinner.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      UplinkManualSpinner.Name = "UplinkManualSpinner";
      UplinkManualSpinner.Size = new Size(68, 23);
      UplinkManualSpinner.TabIndex = 13;
      UplinkManualSpinner.Value = new decimal(new int[] { 20, 0, 0, int.MinValue });
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
      // DownlinkDopplerLabel
      // 
      DownlinkDopplerLabel.BackColor = Color.White;
      DownlinkDopplerLabel.Font = new Font("Segoe UI", 9.5F);
      DownlinkDopplerLabel.Location = new Point(258, 6);
      DownlinkDopplerLabel.Name = "DownlinkDopplerLabel";
      DownlinkDopplerLabel.Size = new Size(55, 20);
      DownlinkDopplerLabel.TabIndex = 15;
      DownlinkDopplerLabel.Text = "+20,000";
      DownlinkDopplerLabel.TextAlign = ContentAlignment.MiddleRight;
      // 
      // UplinkDopplerLabel
      // 
      UplinkDopplerLabel.BackColor = Color.White;
      UplinkDopplerLabel.Font = new Font("Segoe UI", 9.5F);
      UplinkDopplerLabel.Location = new Point(599, 6);
      UplinkDopplerLabel.Name = "UplinkDopplerLabel";
      UplinkDopplerLabel.Size = new Size(55, 20);
      UplinkDopplerLabel.TabIndex = 16;
      UplinkDopplerLabel.Text = "+20,000";
      UplinkDopplerLabel.TextAlign = ContentAlignment.MiddleRight;
      // 
      // FrequencyControl
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BorderStyle = BorderStyle.FixedSingle;
      Controls.Add(UplinkDopplerLabel);
      Controls.Add(DownlinkDopplerLabel);
      Controls.Add(label1);
      Controls.Add(UplinkManualSpinner);
      Controls.Add(checkBox1);
      Controls.Add(checkBox2);
      Controls.Add(UplinkFrequencyLabel);
      Controls.Add(UplinkLabel);
      Controls.Add(label2);
      Controls.Add(DownlinkManualSpinner);
      Controls.Add(ManualCheckbox);
      Controls.Add(DopplerCheckbox);
      Controls.Add(DownlinkFrequencyLabel);
      Controls.Add(SatelliteLabel);
      Name = "FrequencyControl";
      Size = new Size(679, 66);
      ((System.ComponentModel.ISupportInitialize)DownlinkManualSpinner).EndInit();
      ((System.ComponentModel.ISupportInitialize)UplinkManualSpinner).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Label SatelliteLabel;
    private Label DownlinkFrequencyLabel;
    private CheckBox DopplerCheckbox;
    private CheckBox ManualCheckbox;
    private NumericUpDown DownlinkManualSpinner;
    private Label label2;
    private Label label1;
    private NumericUpDown UplinkManualSpinner;
    private CheckBox checkBox1;
    private CheckBox checkBox2;
    private Label UplinkFrequencyLabel;
    private Label UplinkLabel;
    private Label DownlinkDopplerLabel;
    private Label UplinkDopplerLabel;
  }
}
