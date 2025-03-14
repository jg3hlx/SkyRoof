namespace OrbiNom
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
      DownlinkDopplerCheckbox = new CheckBox();
      DownlinkManualCheckbox = new CheckBox();
      DownlinkManualSpinner = new NumericUpDown();
      label2 = new Label();
      label1 = new Label();
      UplinkManualSpinner = new NumericUpDown();
      UplinkManualCheckbox = new CheckBox();
      UplinkDopperCheckbox = new CheckBox();
      UplinkFrequencyLabel = new Label();
      UplinkLabel = new Label();
      DownlinkDopplerLabel = new Label();
      UplinkDopplerLabel = new Label();
      DownlinkModeCombobox = new ComboBox();
      UplinkModeCombobox = new ComboBox();
      ((System.ComponentModel.ISupportInitialize)DownlinkManualSpinner).BeginInit();
      ((System.ComponentModel.ISupportInitialize)UplinkManualSpinner).BeginInit();
      SuspendLayout();
      // 
      // SatelliteLabel
      // 
      SatelliteLabel.AutoEllipsis = true;
      SatelliteLabel.Location = new Point(21, 5);
      SatelliteLabel.Name = "SatelliteLabel";
      SatelliteLabel.Size = new Size(60, 15);
      SatelliteLabel.TabIndex = 0;
      SatelliteLabel.Text = "Terrestrial";
      // 
      // DownlinkFrequencyLabel
      // 
      DownlinkFrequencyLabel.BackColor = Color.Black;
      DownlinkFrequencyLabel.Font = new Font("Microsoft Sans Serif", 16F);
      DownlinkFrequencyLabel.ForeColor = Color.Aqua;
      DownlinkFrequencyLabel.Location = new Point(3, 28);
      DownlinkFrequencyLabel.Name = "DownlinkFrequencyLabel";
      DownlinkFrequencyLabel.Size = new Size(170, 34);
      DownlinkFrequencyLabel.TabIndex = 2;
      DownlinkFrequencyLabel.Text = "000,000,000";
      DownlinkFrequencyLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // DownlinkDopplerCheckbox
      // 
      DownlinkDopplerCheckbox.AutoSize = true;
      DownlinkDopplerCheckbox.Location = new Point(190, 8);
      DownlinkDopplerCheckbox.Name = "DownlinkDopplerCheckbox";
      DownlinkDopplerCheckbox.Size = new Size(68, 19);
      DownlinkDopplerCheckbox.TabIndex = 3;
      DownlinkDopplerCheckbox.Text = "Doppler";
      DownlinkDopplerCheckbox.UseVisualStyleBackColor = true;
      DownlinkDopplerCheckbox.CheckedChanged += DownlinkDopplerCheckbox_CheckedChanged;
      // 
      // DownlinkManualCheckbox
      // 
      DownlinkManualCheckbox.AutoSize = true;
      DownlinkManualCheckbox.Location = new Point(190, 37);
      DownlinkManualCheckbox.Name = "DownlinkManualCheckbox";
      DownlinkManualCheckbox.Size = new Size(66, 19);
      DownlinkManualCheckbox.TabIndex = 4;
      DownlinkManualCheckbox.Text = "Manual";
      DownlinkManualCheckbox.UseVisualStyleBackColor = true;
      DownlinkManualCheckbox.CheckedChanged += DownlinkManualCheckbox_CheckedChanged;
      // 
      // DownlinkManualSpinner
      // 
      DownlinkManualSpinner.DecimalPlaces = 3;
      DownlinkManualSpinner.Font = new Font("Segoe UI", 10F);
      DownlinkManualSpinner.Increment = new decimal(new int[] { 2, 0, 0, 131072 });
      DownlinkManualSpinner.Location = new Point(258, 35);
      DownlinkManualSpinner.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      DownlinkManualSpinner.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      DownlinkManualSpinner.Name = "DownlinkManualSpinner";
      DownlinkManualSpinner.Size = new Size(70, 25);
      DownlinkManualSpinner.TabIndex = 6;
      DownlinkManualSpinner.Value = new decimal(new int[] { 20, 0, 0, int.MinValue });
      DownlinkManualSpinner.ValueChanged += DownlinkManualSpinner_ValueChanged;
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
      UplinkManualSpinner.Increment = new decimal(new int[] { 2, 0, 0, 131072 });
      UplinkManualSpinner.Location = new Point(599, 37);
      UplinkManualSpinner.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      UplinkManualSpinner.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      UplinkManualSpinner.Name = "UplinkManualSpinner";
      UplinkManualSpinner.Size = new Size(68, 23);
      UplinkManualSpinner.TabIndex = 13;
      UplinkManualSpinner.Value = new decimal(new int[] { 20, 0, 0, int.MinValue });
      // 
      // UplinkManualCheckbox
      // 
      UplinkManualCheckbox.AutoSize = true;
      UplinkManualCheckbox.Location = new Point(525, 38);
      UplinkManualCheckbox.Name = "UplinkManualCheckbox";
      UplinkManualCheckbox.Size = new Size(66, 19);
      UplinkManualCheckbox.TabIndex = 11;
      UplinkManualCheckbox.Text = "Manual";
      UplinkManualCheckbox.UseVisualStyleBackColor = true;
      // 
      // UplinkDopperCheckbox
      // 
      UplinkDopperCheckbox.AutoSize = true;
      UplinkDopperCheckbox.Location = new Point(525, 9);
      UplinkDopperCheckbox.Name = "UplinkDopperCheckbox";
      UplinkDopperCheckbox.Size = new Size(68, 19);
      UplinkDopperCheckbox.TabIndex = 10;
      UplinkDopperCheckbox.Text = "Doppler";
      UplinkDopperCheckbox.UseVisualStyleBackColor = true;
      // 
      // UplinkFrequencyLabel
      // 
      UplinkFrequencyLabel.BackColor = Color.Black;
      UplinkFrequencyLabel.Font = new Font("Microsoft Sans Serif", 16F);
      UplinkFrequencyLabel.ForeColor = Color.Aqua;
      UplinkFrequencyLabel.Location = new Point(342, 29);
      UplinkFrequencyLabel.Name = "UplinkFrequencyLabel";
      UplinkFrequencyLabel.Size = new Size(170, 34);
      UplinkFrequencyLabel.TabIndex = 9;
      UplinkFrequencyLabel.Text = "000,000,000";
      UplinkFrequencyLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // UplinkLabel
      // 
      UplinkLabel.AutoEllipsis = true;
      UplinkLabel.Location = new Point(356, 7);
      UplinkLabel.Name = "UplinkLabel";
      UplinkLabel.Size = new Size(62, 15);
      UplinkLabel.TabIndex = 8;
      UplinkLabel.Text = "No Uplink";
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
      // DownlinkModeCombobox
      // 
      DownlinkModeCombobox.FormattingEnabled = true;
      DownlinkModeCombobox.Location = new Point(94, 2);
      DownlinkModeCombobox.Name = "DownlinkModeCombobox";
      DownlinkModeCombobox.Size = new Size(79, 23);
      DownlinkModeCombobox.TabIndex = 17;
      DownlinkModeCombobox.SelectedValueChanged += ModeCombobox_SelectedValueChanged;
      // 
      // UplinkModeCombobox
      // 
      UplinkModeCombobox.AutoCompleteMode = AutoCompleteMode.Suggest;
      UplinkModeCombobox.AutoCompleteSource = AutoCompleteSource.ListItems;
      UplinkModeCombobox.FormattingEnabled = true;
      UplinkModeCombobox.Location = new Point(433, 2);
      UplinkModeCombobox.Name = "UplinkModeCombobox";
      UplinkModeCombobox.Size = new Size(79, 23);
      UplinkModeCombobox.TabIndex = 18;
      UplinkModeCombobox.SelectedValueChanged += ModeCombobox_SelectedValueChanged;
      // 
      // FrequencyControl
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BorderStyle = BorderStyle.FixedSingle;
      Controls.Add(UplinkModeCombobox);
      Controls.Add(DownlinkModeCombobox);
      Controls.Add(UplinkDopplerLabel);
      Controls.Add(DownlinkDopplerLabel);
      Controls.Add(label1);
      Controls.Add(UplinkManualSpinner);
      Controls.Add(UplinkManualCheckbox);
      Controls.Add(UplinkDopperCheckbox);
      Controls.Add(UplinkFrequencyLabel);
      Controls.Add(UplinkLabel);
      Controls.Add(label2);
      Controls.Add(DownlinkManualSpinner);
      Controls.Add(DownlinkManualCheckbox);
      Controls.Add(DownlinkDopplerCheckbox);
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
    private CheckBox DownlinkDopplerCheckbox;
    private CheckBox DownlinkManualCheckbox;
    private NumericUpDown DownlinkManualSpinner;
    private Label label2;
    private Label label1;
    private NumericUpDown UplinkManualSpinner;
    private CheckBox UplinkManualCheckbox;
    private CheckBox UplinkDopperCheckbox;
    private Label UplinkFrequencyLabel;
    private Label UplinkLabel;
    private Label DownlinkDopplerLabel;
    private Label UplinkDopplerLabel;
    private ComboBox DownlinkModeCombobox;
    private ComboBox UplinkModeCombobox;
  }
}
