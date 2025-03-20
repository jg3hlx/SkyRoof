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
      label3 = new Label();
      label4 = new Label();
      label5 = new Label();
      label6 = new Label();
      DownlinkRitSpinner = new NumericUpDown();
      DownlinkRitCheckbox = new CheckBox();
      label7 = new Label();
      ((System.ComponentModel.ISupportInitialize)DownlinkManualSpinner).BeginInit();
      ((System.ComponentModel.ISupportInitialize)UplinkManualSpinner).BeginInit();
      ((System.ComponentModel.ISupportInitialize)DownlinkRitSpinner).BeginInit();
      SuspendLayout();
      // 
      // SatelliteLabel
      // 
      SatelliteLabel.AutoEllipsis = true;
      SatelliteLabel.Location = new Point(26, 9);
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
      DownlinkFrequencyLabel.Location = new Point(8, 36);
      DownlinkFrequencyLabel.Name = "DownlinkFrequencyLabel";
      DownlinkFrequencyLabel.Size = new Size(170, 34);
      DownlinkFrequencyLabel.TabIndex = 2;
      DownlinkFrequencyLabel.Text = "000,000,000";
      DownlinkFrequencyLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // DownlinkDopplerCheckbox
      // 
      DownlinkDopplerCheckbox.AutoSize = true;
      DownlinkDopplerCheckbox.Location = new Point(195, 6);
      DownlinkDopplerCheckbox.Name = "DownlinkDopplerCheckbox";
      DownlinkDopplerCheckbox.Size = new Size(15, 14);
      DownlinkDopplerCheckbox.TabIndex = 3;
      DownlinkDopplerCheckbox.UseVisualStyleBackColor = true;
      DownlinkDopplerCheckbox.CheckedChanged += DownlinkDopplerCheckbox_CheckedChanged;
      // 
      // DownlinkManualCheckbox
      // 
      DownlinkManualCheckbox.AutoSize = true;
      DownlinkManualCheckbox.Location = new Point(195, 29);
      DownlinkManualCheckbox.Name = "DownlinkManualCheckbox";
      DownlinkManualCheckbox.Size = new Size(15, 14);
      DownlinkManualCheckbox.TabIndex = 4;
      DownlinkManualCheckbox.UseVisualStyleBackColor = true;
      DownlinkManualCheckbox.CheckedChanged += DownlinkManualCheckbox_CheckedChanged;
      // 
      // DownlinkManualSpinner
      // 
      DownlinkManualSpinner.BorderStyle = BorderStyle.None;
      DownlinkManualSpinner.DecimalPlaces = 3;
      DownlinkManualSpinner.Font = new Font("Segoe UI", 10F);
      DownlinkManualSpinner.Increment = new decimal(new int[] { 2, 0, 0, 131072 });
      DownlinkManualSpinner.Location = new Point(263, 27);
      DownlinkManualSpinner.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      DownlinkManualSpinner.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      DownlinkManualSpinner.Name = "DownlinkManualSpinner";
      DownlinkManualSpinner.Size = new Size(72, 21);
      DownlinkManualSpinner.TabIndex = 6;
      DownlinkManualSpinner.ValueChanged += DownlinkManualSpinner_ValueChanged;
      DownlinkManualSpinner.DoubleClick += Spinner_DoubleClick;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Font = new Font("Source Code Pro", 14F, FontStyle.Bold);
      label2.Location = new Point(8, 2);
      label2.Name = "label2";
      label2.Size = new Size(21, 24);
      label2.TabIndex = 7;
      label2.Text = "↓";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Font = new Font("Source Code Pro", 14F, FontStyle.Bold);
      label1.Location = new Point(343, -2);
      label1.Name = "label1";
      label1.Size = new Size(21, 24);
      label1.TabIndex = 14;
      label1.Text = "↑";
      // 
      // UplinkManualSpinner
      // 
      UplinkManualSpinner.BorderStyle = BorderStyle.None;
      UplinkManualSpinner.DecimalPlaces = 3;
      UplinkManualSpinner.Font = new Font("Segoe UI", 9.5F);
      UplinkManualSpinner.Increment = new decimal(new int[] { 2, 0, 0, 131072 });
      UplinkManualSpinner.Location = new Point(600, 27);
      UplinkManualSpinner.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      UplinkManualSpinner.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      UplinkManualSpinner.Name = "UplinkManualSpinner";
      UplinkManualSpinner.Size = new Size(72, 20);
      UplinkManualSpinner.TabIndex = 13;
      UplinkManualSpinner.DoubleClick += Spinner_DoubleClick;
      // 
      // UplinkManualCheckbox
      // 
      UplinkManualCheckbox.AutoSize = true;
      UplinkManualCheckbox.Location = new Point(530, 29);
      UplinkManualCheckbox.Name = "UplinkManualCheckbox";
      UplinkManualCheckbox.Size = new Size(15, 14);
      UplinkManualCheckbox.TabIndex = 11;
      UplinkManualCheckbox.UseVisualStyleBackColor = true;
      // 
      // UplinkDopperCheckbox
      // 
      UplinkDopperCheckbox.AutoSize = true;
      UplinkDopperCheckbox.Location = new Point(530, 6);
      UplinkDopperCheckbox.Name = "UplinkDopperCheckbox";
      UplinkDopperCheckbox.Size = new Size(15, 14);
      UplinkDopperCheckbox.TabIndex = 10;
      UplinkDopperCheckbox.UseVisualStyleBackColor = true;
      // 
      // UplinkFrequencyLabel
      // 
      UplinkFrequencyLabel.BackColor = Color.Black;
      UplinkFrequencyLabel.Font = new Font("Microsoft Sans Serif", 16F);
      UplinkFrequencyLabel.ForeColor = Color.Aqua;
      UplinkFrequencyLabel.Location = new Point(347, 36);
      UplinkFrequencyLabel.Name = "UplinkFrequencyLabel";
      UplinkFrequencyLabel.Size = new Size(170, 34);
      UplinkFrequencyLabel.TabIndex = 9;
      UplinkFrequencyLabel.Text = "000,000,000";
      UplinkFrequencyLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // UplinkLabel
      // 
      UplinkLabel.AutoEllipsis = true;
      UplinkLabel.Location = new Point(361, 7);
      UplinkLabel.Name = "UplinkLabel";
      UplinkLabel.Size = new Size(62, 15);
      UplinkLabel.TabIndex = 8;
      UplinkLabel.Text = "No Uplink";
      // 
      // DownlinkDopplerLabel
      // 
      DownlinkDopplerLabel.BackColor = Color.White;
      DownlinkDopplerLabel.Font = new Font("Segoe UI", 9.5F);
      DownlinkDopplerLabel.Location = new Point(263, 3);
      DownlinkDopplerLabel.Name = "DownlinkDopplerLabel";
      DownlinkDopplerLabel.Size = new Size(56, 20);
      DownlinkDopplerLabel.TabIndex = 15;
      DownlinkDopplerLabel.Text = "0,000";
      DownlinkDopplerLabel.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // UplinkDopplerLabel
      // 
      UplinkDopplerLabel.BackColor = Color.White;
      UplinkDopplerLabel.Font = new Font("Segoe UI", 9.5F);
      UplinkDopplerLabel.Location = new Point(600, 3);
      UplinkDopplerLabel.Name = "UplinkDopplerLabel";
      UplinkDopplerLabel.Size = new Size(56, 20);
      UplinkDopplerLabel.TabIndex = 16;
      UplinkDopplerLabel.Text = "0,000";
      UplinkDopplerLabel.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // DownlinkModeCombobox
      // 
      DownlinkModeCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
      DownlinkModeCombobox.FormattingEnabled = true;
      DownlinkModeCombobox.Location = new Point(99, 6);
      DownlinkModeCombobox.Name = "DownlinkModeCombobox";
      DownlinkModeCombobox.Size = new Size(79, 23);
      DownlinkModeCombobox.TabIndex = 17;
      DownlinkModeCombobox.SelectedValueChanged += ModeCombobox_SelectedValueChanged;
      // 
      // UplinkModeCombobox
      // 
      UplinkModeCombobox.AutoCompleteMode = AutoCompleteMode.Suggest;
      UplinkModeCombobox.AutoCompleteSource = AutoCompleteSource.ListItems;
      UplinkModeCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
      UplinkModeCombobox.FormattingEnabled = true;
      UplinkModeCombobox.Location = new Point(438, 6);
      UplinkModeCombobox.Name = "UplinkModeCombobox";
      UplinkModeCombobox.Size = new Size(79, 23);
      UplinkModeCombobox.TabIndex = 18;
      UplinkModeCombobox.SelectedValueChanged += ModeCombobox_SelectedValueChanged;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(211, 6);
      label3.Name = "label3";
      label3.Size = new Size(49, 15);
      label3.TabIndex = 19;
      label3.Text = "Doppler";
      label3.Click += label3_Click;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(211, 29);
      label4.Name = "label4";
      label4.Size = new Size(47, 15);
      label4.TabIndex = 20;
      label4.Text = "Manual";
      label4.Click += label4_Click;
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new Point(546, 29);
      label5.Name = "label5";
      label5.Size = new Size(47, 15);
      label5.TabIndex = 22;
      label5.Text = "Manual";
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Location = new Point(546, 6);
      label6.Name = "label6";
      label6.Size = new Size(49, 15);
      label6.TabIndex = 21;
      label6.Text = "Doppler";
      // 
      // DownlinkRitSpinner
      // 
      DownlinkRitSpinner.BorderStyle = BorderStyle.None;
      DownlinkRitSpinner.DecimalPlaces = 3;
      DownlinkRitSpinner.Font = new Font("Segoe UI", 10F);
      DownlinkRitSpinner.Increment = new decimal(new int[] { 2, 0, 0, 131072 });
      DownlinkRitSpinner.Location = new Point(263, 52);
      DownlinkRitSpinner.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      DownlinkRitSpinner.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      DownlinkRitSpinner.Name = "DownlinkRitSpinner";
      DownlinkRitSpinner.Size = new Size(72, 21);
      DownlinkRitSpinner.TabIndex = 23;
      DownlinkRitSpinner.DoubleClick += Spinner_DoubleClick;
      // 
      // DownlinkRitCheckbox
      // 
      DownlinkRitCheckbox.AutoSize = true;
      DownlinkRitCheckbox.Location = new Point(195, 52);
      DownlinkRitCheckbox.Name = "DownlinkRitCheckbox";
      DownlinkRitCheckbox.Size = new Size(15, 14);
      DownlinkRitCheckbox.TabIndex = 24;
      DownlinkRitCheckbox.UseVisualStyleBackColor = true;
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Location = new Point(211, 53);
      label7.Name = "label7";
      label7.Size = new Size(23, 15);
      label7.TabIndex = 25;
      label7.Text = "RIT";
      label7.Click += label7_Click;
      // 
      // FrequencyControl
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BorderStyle = BorderStyle.FixedSingle;
      Controls.Add(label7);
      Controls.Add(DownlinkRitCheckbox);
      Controls.Add(DownlinkRitSpinner);
      Controls.Add(label5);
      Controls.Add(label6);
      Controls.Add(label4);
      Controls.Add(label3);
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
      Size = new Size(679, 77);
      ((System.ComponentModel.ISupportInitialize)DownlinkManualSpinner).EndInit();
      ((System.ComponentModel.ISupportInitialize)UplinkManualSpinner).EndInit();
      ((System.ComponentModel.ISupportInitialize)DownlinkRitSpinner).EndInit();
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
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private NumericUpDown DownlinkRitSpinner;
    private CheckBox DownlinkRitCheckbox;
    private Label label7;
  }
}
