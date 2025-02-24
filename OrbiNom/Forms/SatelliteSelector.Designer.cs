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
      TransmitterComboBox.Location = new Point(275, 7);
      TransmitterComboBox.Name = "TransmitterComboBox";
      TransmitterComboBox.Size = new Size(203, 24);
      TransmitterComboBox.TabIndex = 13;
      TransmitterComboBox.DrawItem += TransmitterComboBox_DrawItem;
      TransmitterComboBox.SelectedIndexChanged += TransmitterComboBox_SelectedIndexChanged;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(203, 10);
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
      SatelliteComboBox.Location = new Point(57, 37);
      SatelliteComboBox.Name = "SatelliteComboBox";
      SatelliteComboBox.Size = new Size(121, 24);
      SatelliteComboBox.TabIndex = 11;
      SatelliteComboBox.DrawItem += SatelliteComboBox_DrawItem;
      SatelliteComboBox.SelectedIndexChanged += SatelliteComboBox_SelectedIndexChanged;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(7, 40);
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
      GroupComboBox.Location = new Point(57, 8);
      GroupComboBox.Name = "GroupComboBox";
      GroupComboBox.Size = new Size(121, 23);
      GroupComboBox.TabIndex = 9;
      GroupComboBox.SelectedIndexChanged += GroupComboBox_SelectedIndexChanged;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(7, 10);
      label1.Name = "label1";
      label1.Size = new Size(40, 15);
      label1.TabIndex = 8;
      label1.Text = "Group";
      // 
      // GainSlider
      // 
      GainSlider.Location = new Point(363, 36);
      GainSlider.Maximum = 100;
      GainSlider.Name = "GainSlider";
      GainSlider.Size = new Size(121, 45);
      GainSlider.TabIndex = 16;
      GainSlider.TickFrequency = 10;
      toolTip1.SetToolTip(GainSlider, "SDR Gain");
      GainSlider.ValueChanged += GainSlider_ValueChanged;
      // 
      // comboBox1
      // 
      comboBox1.FormattingEnabled = true;
      comboBox1.Location = new Point(275, 38);
      comboBox1.Name = "comboBox1";
      comboBox1.Size = new Size(79, 23);
      comboBox1.TabIndex = 14;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(203, 40);
      label4.Name = "label4";
      label4.Size = new Size(38, 15);
      label4.TabIndex = 15;
      label4.Text = "Mode";
      // 
      // SatelliteSelector
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
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
      Size = new Size(496, 65);
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
  }
}
