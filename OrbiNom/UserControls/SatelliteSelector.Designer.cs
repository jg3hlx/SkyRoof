namespace OrbiNom
{
  partial class SatelliteSelector
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
      components = new System.ComponentModel.Container();
      TransmitterComboBox = new ComboBox();
      label3 = new Label();
      SatelliteComboBox = new ComboBox();
      label2 = new Label();
      GroupComboBox = new ComboBox();
      label1 = new Label();
      toolTip1 = new ToolTip(components);
      GainSlider = new TrackBar();
      TuneToBtn = new Button();
      comboBox1 = new ComboBox();
      label4 = new Label();
      ((System.ComponentModel.ISupportInitialize)GainSlider).BeginInit();
      SuspendLayout();
      // 
      // TransmitterComboBox
      // 
      TransmitterComboBox.DisplayMember = "description";
      TransmitterComboBox.DrawMode = DrawMode.OwnerDrawFixed;
      TransmitterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      TransmitterComboBox.FormattingEnabled = true;
      TransmitterComboBox.Location = new Point(273, 7);
      TransmitterComboBox.Name = "TransmitterComboBox";
      TransmitterComboBox.Size = new Size(203, 24);
      TransmitterComboBox.TabIndex = 13;
      TransmitterComboBox.DrawItem += TransmitterComboBox_DrawItem;
      TransmitterComboBox.SelectedIndexChanged += TransmitterComboBox_SelectedIndexChanged;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(201, 10);
      label3.Name = "label3";
      label3.Size = new Size(66, 15);
      label3.TabIndex = 12;
      label3.Text = "Transmitter";
      // 
      // SatelliteComboBox
      // 
      SatelliteComboBox.DisplayMember = "name";
      SatelliteComboBox.DrawMode = DrawMode.OwnerDrawFixed;
      SatelliteComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      SatelliteComboBox.FormattingEnabled = true;
      SatelliteComboBox.Location = new Point(55, 37);
      SatelliteComboBox.Name = "SatelliteComboBox";
      SatelliteComboBox.Size = new Size(140, 24);
      SatelliteComboBox.TabIndex = 11;
      SatelliteComboBox.DrawItem += SatelliteComboBox_DrawItem;
      SatelliteComboBox.SelectedIndexChanged += SatelliteComboBox_SelectedIndexChanged;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(5, 40);
      label2.Name = "label2";
      label2.Size = new Size(48, 15);
      label2.TabIndex = 10;
      label2.Text = "Satellite";
      // 
      // GroupComboBox
      // 
      GroupComboBox.DisplayMember = "Name";
      GroupComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      GroupComboBox.FormattingEnabled = true;
      GroupComboBox.Location = new Point(55, 8);
      GroupComboBox.Name = "GroupComboBox";
      GroupComboBox.Size = new Size(140, 23);
      GroupComboBox.TabIndex = 9;
      GroupComboBox.SelectedIndexChanged += GroupComboBox_SelectedIndexChanged;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(5, 10);
      label1.Name = "label1";
      label1.Size = new Size(40, 15);
      label1.TabIndex = 8;
      label1.Text = "Group";
      // 
      // GainSlider
      // 
      GainSlider.Location = new Point(361, 36);
      GainSlider.Maximum = 100;
      GainSlider.Name = "GainSlider";
      GainSlider.Size = new Size(121, 45);
      GainSlider.TabIndex = 16;
      GainSlider.TickFrequency = 10;
      toolTip1.SetToolTip(GainSlider, "SDR Gain");
      GainSlider.ValueChanged += GainSlider_ValueChanged;
      // 
      // TuneToBtn
      // 
      TuneToBtn.BackColor = Color.Transparent;
      TuneToBtn.Font = new Font("Wingdings 3", 9F);
      TuneToBtn.ForeColor = Color.Gray;
      TuneToBtn.Location = new Point(478, 6);
      TuneToBtn.Name = "TuneToBtn";
      TuneToBtn.Size = new Size(16, 26);
      TuneToBtn.TabIndex = 17;
      TuneToBtn.Text = "u";
      toolTip1.SetToolTip(TuneToBtn, "Tune to Transmitter");
      TuneToBtn.UseVisualStyleBackColor = false;
      TuneToBtn.Click += TuneToBtn_Click;
      // 
      // comboBox1
      // 
      comboBox1.FormattingEnabled = true;
      comboBox1.Location = new Point(273, 38);
      comboBox1.Name = "comboBox1";
      comboBox1.Size = new Size(79, 23);
      comboBox1.TabIndex = 14;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(201, 40);
      label4.Name = "label4";
      label4.Size = new Size(38, 15);
      label4.TabIndex = 15;
      label4.Text = "Mode";
      // 
      // SatelliteSelector
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BorderStyle = BorderStyle.FixedSingle;
      Controls.Add(TuneToBtn);
      Controls.Add(GainSlider);
      Controls.Add(label4);
      Controls.Add(comboBox1);
      Controls.Add(TransmitterComboBox);
      Controls.Add(label3);
      Controls.Add(SatelliteComboBox);
      Controls.Add(label2);
      Controls.Add(GroupComboBox);
      Controls.Add(label1);
      Name = "SatelliteSelector";
      Size = new Size(498, 63);
      ((System.ComponentModel.ISupportInitialize)GainSlider).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private ComboBox TransmitterComboBox;
    private Label label3;
    private ComboBox SatelliteComboBox;
    private Label label2;
    private ComboBox GroupComboBox;
    private Label label1;
    private ToolTip toolTip1;
    private ComboBox comboBox1;
    private Label label4;
    public TrackBar GainSlider;
    private Button TuneToBtn;
  }
}
