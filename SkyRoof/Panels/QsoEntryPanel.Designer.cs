namespace SkyRoof
{
  partial class QsoEntryPanel
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
      panel1 = new Panel();
      UtcPicker = new DateTimePicker();
      label9 = new Label();
      panel2 = new Panel();
      BandComboBox = new ComboBox();
      label10 = new Label();
      panel3 = new Panel();
      ModeComboBox = new ComboBox();
      label11 = new Label();
      panel4 = new Panel();
      SatComboBox = new ComboBox();
      label12 = new Label();
      panel5 = new Panel();
      label13 = new Label();
      CallEdit = new TextBox();
      panel6 = new Panel();
      label14 = new Label();
      GridEdit = new TextBox();
      panel11 = new Panel();
      StateComboBox = new ComboBox();
      label1 = new Label();
      panel7 = new Panel();
      label15 = new Label();
      SentEdit = new TextBox();
      panel8 = new Panel();
      label16 = new Label();
      RecvEdit = new TextBox();
      panel9 = new Panel();
      label17 = new Label();
      NameEdit = new TextBox();
      ButtonsPanel = new Panel();
      ClearBtn = new Button();
      LogBtn = new Button();
      flowLayoutPanel1.SuspendLayout();
      panel1.SuspendLayout();
      panel2.SuspendLayout();
      panel3.SuspendLayout();
      panel4.SuspendLayout();
      panel5.SuspendLayout();
      panel6.SuspendLayout();
      panel11.SuspendLayout();
      panel7.SuspendLayout();
      panel8.SuspendLayout();
      panel9.SuspendLayout();
      ButtonsPanel.SuspendLayout();
      SuspendLayout();
      // 
      // flowLayoutPanel1
      // 
      flowLayoutPanel1.AutoSize = true;
      flowLayoutPanel1.Controls.Add(panel1);
      flowLayoutPanel1.Controls.Add(panel2);
      flowLayoutPanel1.Controls.Add(panel3);
      flowLayoutPanel1.Controls.Add(panel4);
      flowLayoutPanel1.Controls.Add(panel5);
      flowLayoutPanel1.Controls.Add(panel6);
      flowLayoutPanel1.Controls.Add(panel11);
      flowLayoutPanel1.Controls.Add(panel7);
      flowLayoutPanel1.Controls.Add(panel8);
      flowLayoutPanel1.Controls.Add(panel9);
      flowLayoutPanel1.Controls.Add(ButtonsPanel);
      flowLayoutPanel1.Dock = DockStyle.Fill;
      flowLayoutPanel1.Location = new Point(0, 0);
      flowLayoutPanel1.Name = "flowLayoutPanel1";
      flowLayoutPanel1.Size = new Size(766, 150);
      flowLayoutPanel1.TabIndex = 0;
      // 
      // panel1
      // 
      panel1.BackColor = Color.LightSkyBlue;
      panel1.Controls.Add(UtcPicker);
      panel1.Controls.Add(label9);
      panel1.Location = new Point(3, 3);
      panel1.Name = "panel1";
      panel1.Size = new Size(177, 36);
      panel1.TabIndex = 0;
      // 
      // UtcPicker
      // 
      UtcPicker.CustomFormat = "MM/dd HH:mm";
      UtcPicker.Font = new Font("Courier New", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
      UtcPicker.Format = DateTimePickerFormat.Custom;
      UtcPicker.Location = new Point(45, 7);
      UtcPicker.Name = "UtcPicker";
      UtcPicker.Size = new Size(128, 23);
      UtcPicker.TabIndex = 0;
      // 
      // label9
      // 
      label9.AutoSize = true;
      label9.Location = new Point(3, 11);
      label9.Name = "label9";
      label9.Size = new Size(28, 15);
      label9.TabIndex = 21;
      label9.Text = "UTC";
      label9.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // panel2
      // 
      panel2.BackColor = Color.LightSkyBlue;
      panel2.Controls.Add(BandComboBox);
      panel2.Controls.Add(label10);
      panel2.Location = new Point(186, 3);
      panel2.Name = "panel2";
      panel2.Size = new Size(177, 36);
      panel2.TabIndex = 1;
      // 
      // BandComboBox
      // 
      BandComboBox.AutoCompleteCustomSource.AddRange(new string[] { "2M", "70CM" });
      BandComboBox.Font = new Font("Courier New", 12F);
      BandComboBox.FormattingEnabled = true;
      BandComboBox.Location = new Point(45, 5);
      BandComboBox.Name = "BandComboBox";
      BandComboBox.Size = new Size(128, 26);
      BandComboBox.TabIndex = 22;
      // 
      // label10
      // 
      label10.AutoSize = true;
      label10.Location = new Point(3, 11);
      label10.Name = "label10";
      label10.Size = new Size(39, 15);
      label10.TabIndex = 21;
      label10.Text = "BAND";
      label10.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // panel3
      // 
      panel3.BackColor = Color.LightSkyBlue;
      panel3.Controls.Add(ModeComboBox);
      panel3.Controls.Add(label11);
      panel3.Location = new Point(369, 3);
      panel3.Name = "panel3";
      panel3.Size = new Size(177, 36);
      panel3.TabIndex = 2;
      // 
      // ModeComboBox
      // 
      ModeComboBox.AutoCompleteCustomSource.AddRange(new string[] { "SSB", "CW", "FM", "FT4" });
      ModeComboBox.Font = new Font("Courier New", 12F);
      ModeComboBox.FormattingEnabled = true;
      ModeComboBox.Location = new Point(45, 5);
      ModeComboBox.Name = "ModeComboBox";
      ModeComboBox.Size = new Size(128, 26);
      ModeComboBox.TabIndex = 23;
      // 
      // label11
      // 
      label11.AutoSize = true;
      label11.Location = new Point(3, 11);
      label11.Name = "label11";
      label11.Size = new Size(41, 15);
      label11.TabIndex = 21;
      label11.Text = "MODE";
      label11.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // panel4
      // 
      panel4.BackColor = Color.LightSkyBlue;
      panel4.Controls.Add(SatComboBox);
      panel4.Controls.Add(label12);
      panel4.Location = new Point(552, 3);
      panel4.Name = "panel4";
      panel4.Size = new Size(177, 36);
      panel4.TabIndex = 3;
      // 
      // SatComboBox
      // 
      SatComboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
      SatComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
      SatComboBox.Font = new Font("Courier New", 12F);
      SatComboBox.FormattingEnabled = true;
      SatComboBox.Location = new Point(45, 5);
      SatComboBox.Name = "SatComboBox";
      SatComboBox.Size = new Size(128, 26);
      SatComboBox.TabIndex = 23;
      // 
      // label12
      // 
      label12.AutoSize = true;
      label12.Location = new Point(3, 11);
      label12.Name = "label12";
      label12.Size = new Size(26, 15);
      label12.TabIndex = 21;
      label12.Text = "SAT";
      label12.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // panel5
      // 
      panel5.BackColor = Color.LightSkyBlue;
      panel5.Controls.Add(label13);
      panel5.Controls.Add(CallEdit);
      panel5.Location = new Point(3, 45);
      panel5.Name = "panel5";
      panel5.Size = new Size(177, 36);
      panel5.TabIndex = 4;
      // 
      // label13
      // 
      label13.AutoSize = true;
      label13.Location = new Point(3, 11);
      label13.Margin = new Padding(6);
      label13.Name = "label13";
      label13.Size = new Size(35, 15);
      label13.TabIndex = 21;
      label13.Text = "CALL";
      label13.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // CallEdit
      // 
      CallEdit.CharacterCasing = CharacterCasing.Upper;
      CallEdit.Font = new Font("Courier New", 12F);
      CallEdit.Location = new Point(45, 5);
      CallEdit.Margin = new Padding(13);
      CallEdit.Name = "CallEdit";
      CallEdit.Size = new Size(128, 26);
      CallEdit.TabIndex = 22;
      // 
      // panel6
      // 
      panel6.BackColor = Color.LightSkyBlue;
      panel6.Controls.Add(label14);
      panel6.Controls.Add(GridEdit);
      panel6.Location = new Point(186, 45);
      panel6.Name = "panel6";
      panel6.Size = new Size(177, 36);
      panel6.TabIndex = 5;
      // 
      // label14
      // 
      label14.AutoSize = true;
      label14.Location = new Point(3, 11);
      label14.Name = "label14";
      label14.Size = new Size(33, 15);
      label14.TabIndex = 21;
      label14.Text = "GRID";
      label14.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // GridEdit
      // 
      GridEdit.CharacterCasing = CharacterCasing.Upper;
      GridEdit.Font = new Font("Courier New", 12F);
      GridEdit.Location = new Point(45, 5);
      GridEdit.Margin = new Padding(13);
      GridEdit.Name = "GridEdit";
      GridEdit.Size = new Size(128, 26);
      GridEdit.TabIndex = 22;
      // 
      // panel11
      // 
      panel11.BackColor = Color.LightSkyBlue;
      panel11.Controls.Add(StateComboBox);
      panel11.Controls.Add(label1);
      panel11.Location = new Point(369, 45);
      panel11.Name = "panel11";
      panel11.Size = new Size(177, 36);
      panel11.TabIndex = 6;
      // 
      // StateComboBox
      // 
      StateComboBox.AutoCompleteCustomSource.AddRange(new string[] { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY" });
      StateComboBox.Font = new Font("Courier New", 12F);
      StateComboBox.FormattingEnabled = true;
      StateComboBox.Location = new Point(45, 5);
      StateComboBox.Name = "StateComboBox";
      StateComboBox.Size = new Size(128, 26);
      StateComboBox.TabIndex = 23;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(3, 11);
      label1.Name = "label1";
      label1.Size = new Size(37, 15);
      label1.TabIndex = 21;
      label1.Text = "STATE";
      label1.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // panel7
      // 
      panel7.BackColor = Color.LightSkyBlue;
      panel7.Controls.Add(label15);
      panel7.Controls.Add(SentEdit);
      panel7.Location = new Point(552, 45);
      panel7.Name = "panel7";
      panel7.Size = new Size(177, 36);
      panel7.TabIndex = 7;
      // 
      // label15
      // 
      label15.AutoSize = true;
      label15.Location = new Point(3, 11);
      label15.Name = "label15";
      label15.Size = new Size(34, 15);
      label15.TabIndex = 21;
      label15.Text = "SENT";
      label15.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // SentEdit
      // 
      SentEdit.CharacterCasing = CharacterCasing.Upper;
      SentEdit.Font = new Font("Courier New", 12F);
      SentEdit.Location = new Point(45, 5);
      SentEdit.Margin = new Padding(13);
      SentEdit.Name = "SentEdit";
      SentEdit.Size = new Size(128, 26);
      SentEdit.TabIndex = 22;
      // 
      // panel8
      // 
      panel8.BackColor = Color.LightSkyBlue;
      panel8.Controls.Add(label16);
      panel8.Controls.Add(RecvEdit);
      panel8.Location = new Point(3, 87);
      panel8.Name = "panel8";
      panel8.Size = new Size(177, 36);
      panel8.TabIndex = 8;
      // 
      // label16
      // 
      label16.AutoSize = true;
      label16.Location = new Point(3, 11);
      label16.Name = "label16";
      label16.Size = new Size(35, 15);
      label16.TabIndex = 21;
      label16.Text = "RECV";
      label16.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // RecvEdit
      // 
      RecvEdit.CharacterCasing = CharacterCasing.Upper;
      RecvEdit.Font = new Font("Courier New", 12F);
      RecvEdit.Location = new Point(45, 5);
      RecvEdit.Margin = new Padding(13);
      RecvEdit.Name = "RecvEdit";
      RecvEdit.Size = new Size(128, 26);
      RecvEdit.TabIndex = 22;
      // 
      // panel9
      // 
      panel9.BackColor = Color.LightSkyBlue;
      panel9.Controls.Add(label17);
      panel9.Controls.Add(NameEdit);
      panel9.Location = new Point(186, 87);
      panel9.Name = "panel9";
      panel9.Size = new Size(177, 36);
      panel9.TabIndex = 9;
      // 
      // label17
      // 
      label17.AutoSize = true;
      label17.Location = new Point(3, 11);
      label17.Name = "label17";
      label17.Size = new Size(41, 15);
      label17.TabIndex = 21;
      label17.Text = "NAME";
      label17.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // NameEdit
      // 
      NameEdit.Font = new Font("Courier New", 12F);
      NameEdit.Location = new Point(45, 5);
      NameEdit.Margin = new Padding(13);
      NameEdit.Name = "NameEdit";
      NameEdit.Size = new Size(128, 26);
      NameEdit.TabIndex = 22;
      // 
      // ButtonsPanel
      // 
      ButtonsPanel.BackColor = Color.LightSkyBlue;
      ButtonsPanel.Controls.Add(ClearBtn);
      ButtonsPanel.Controls.Add(LogBtn);
      ButtonsPanel.Location = new Point(369, 87);
      ButtonsPanel.Name = "ButtonsPanel";
      ButtonsPanel.Size = new Size(177, 36);
      ButtonsPanel.TabIndex = 10;
      // 
      // ClearBtn
      // 
      ClearBtn.Location = new Point(94, 6);
      ClearBtn.Margin = new Padding(13);
      ClearBtn.Name = "ClearBtn";
      ClearBtn.Size = new Size(72, 23);
      ClearBtn.TabIndex = 24;
      ClearBtn.Text = "Clear";
      ClearBtn.UseVisualStyleBackColor = true;
      // 
      // LogBtn
      // 
      LogBtn.Location = new Point(11, 7);
      LogBtn.Name = "LogBtn";
      LogBtn.Size = new Size(72, 23);
      LogBtn.TabIndex = 23;
      LogBtn.Text = "Log";
      LogBtn.UseVisualStyleBackColor = true;
      // 
      // QsoEntryPanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(766, 150);
      Controls.Add(flowLayoutPanel1);
      Name = "QsoEntryPanel";
      Text = "QSO Entry";
      FormClosing += QsoEntryPanel_FormClosing;
      flowLayoutPanel1.ResumeLayout(false);
      panel1.ResumeLayout(false);
      panel1.PerformLayout();
      panel2.ResumeLayout(false);
      panel2.PerformLayout();
      panel3.ResumeLayout(false);
      panel3.PerformLayout();
      panel4.ResumeLayout(false);
      panel4.PerformLayout();
      panel5.ResumeLayout(false);
      panel5.PerformLayout();
      panel6.ResumeLayout(false);
      panel6.PerformLayout();
      panel11.ResumeLayout(false);
      panel11.PerformLayout();
      panel7.ResumeLayout(false);
      panel7.PerformLayout();
      panel8.ResumeLayout(false);
      panel8.PerformLayout();
      panel9.ResumeLayout(false);
      panel9.PerformLayout();
      ButtonsPanel.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private FlowLayoutPanel flowLayoutPanel1;
    private Panel panel1;
    private Label label9;
    private TextBox textBox9;
    private Panel panel2;
    private Label label10;
    private Panel panel3;
    private Label label11;
    private Panel panel4;
    private Label label12;
    private Panel panel5;
    private Label label13;
    private TextBox CallEdit;
    private Panel panel6;
    private Label label14;
    private TextBox GridEdit;
    private Panel panel7;
    private Label label15;
    private TextBox SentEdit;
    private Panel panel8;
    private Label label16;
    private TextBox RecvEdit;
    private Panel panel9;
    private Label label17;
    private TextBox NameEdit;
    private Panel ButtonsPanel;
    private Button LogBtn;
    private Button ClearBtn;
    private DateTimePicker UtcPicker;
    private ComboBox BandComboBox;
    private ComboBox ModeComboBox;
    private ComboBox SatComboBox;
    private Panel panel11;
    private ComboBox StateComboBox;
    private Label label1;
  }
}