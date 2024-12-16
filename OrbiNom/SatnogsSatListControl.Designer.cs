namespace OrbiNom
{
  partial class SatnogsSatListControl
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
      CountLabel = new Label();
      label3 = new Label();
      FilterTextbox = new TextBox();
      label2 = new Label();
      TransmitterCheckbox = new CheckBox();
      TransceiverCheckbox = new CheckBox();
      TransponderCheckbox = new CheckBox();
      label1 = new Label();
      columnHeader3 = new ColumnHeader();
      columnHeader2 = new ColumnHeader();
      columnHeader1 = new ColumnHeader();
      ReEnteredCheckbox = new CheckBox();
      FutureCheckbox = new CheckBox();
      panel2 = new Panel();
      panel1 = new Panel();
      flowLayoutPanel5 = new FlowLayoutPanel();
      button1 = new Button();
      UpdatedDateLabel = new Label();
      flowLayoutPanel4 = new FlowLayoutPanel();
      label5 = new Label();
      HamCheckbox = new CheckBox();
      NonHamCheckbox = new CheckBox();
      flowLayoutPanel3 = new FlowLayoutPanel();
      label4 = new Label();
      VhfCheckbox = new CheckBox();
      UhfCheckbox = new CheckBox();
      OtherBandsCheckbox = new CheckBox();
      flowLayoutPanel2 = new FlowLayoutPanel();
      flowLayoutPanel1 = new FlowLayoutPanel();
      AliveCheckbox = new CheckBox();
      listView1 = new ListView();
      columnHeader4 = new ColumnHeader();
      SatelliteListPopupMenu = new ContextMenuStrip(components);
      RenameSatMNU = new ToolStripMenuItem();
      PropertiesSatMNU = new ToolStripMenuItem();
      toolTip1 = new ToolTip(components);
      panel2.SuspendLayout();
      panel1.SuspendLayout();
      flowLayoutPanel5.SuspendLayout();
      flowLayoutPanel4.SuspendLayout();
      flowLayoutPanel3.SuspendLayout();
      flowLayoutPanel2.SuspendLayout();
      flowLayoutPanel1.SuspendLayout();
      SatelliteListPopupMenu.SuspendLayout();
      SuspendLayout();
      // 
      // CountLabel
      // 
      CountLabel.AutoSize = true;
      CountLabel.Location = new Point(11, 4);
      CountLabel.Name = "CountLabel";
      CountLabel.Size = new Size(13, 15);
      CountLabel.TabIndex = 0;
      CountLabel.Text = "0";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Dock = DockStyle.Left;
      label3.Location = new Point(3, 0);
      label3.Name = "label3";
      label3.Size = new Size(45, 29);
      label3.TabIndex = 11;
      label3.Text = "Search:";
      label3.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // FilterTextbox
      // 
      FilterTextbox.Location = new Point(54, 3);
      FilterTextbox.Name = "FilterTextbox";
      FilterTextbox.Size = new Size(167, 23);
      FilterTextbox.TabIndex = 10;
      toolTip1.SetToolTip(FilterTextbox, "Search satellites by name, callsign or NORAD id");
      FilterTextbox.TextChanged += Filter_Changed;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Dock = DockStyle.Left;
      label2.Location = new Point(3, 0);
      label2.Name = "label2";
      label2.Size = new Size(40, 25);
      label2.TabIndex = 8;
      label2.Text = "Radio:";
      label2.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // TransmitterCheckbox
      // 
      TransmitterCheckbox.AutoSize = true;
      TransmitterCheckbox.Checked = true;
      TransmitterCheckbox.CheckState = CheckState.Checked;
      TransmitterCheckbox.Location = new Point(236, 3);
      TransmitterCheckbox.Name = "TransmitterCheckbox";
      TransmitterCheckbox.Size = new Size(85, 19);
      TransmitterCheckbox.TabIndex = 7;
      TransmitterCheckbox.Text = "Transmitter";
      TransmitterCheckbox.UseVisualStyleBackColor = true;
      TransmitterCheckbox.Click += Filter_Changed;
      // 
      // TransceiverCheckbox
      // 
      TransceiverCheckbox.AutoSize = true;
      TransceiverCheckbox.Checked = true;
      TransceiverCheckbox.CheckState = CheckState.Checked;
      TransceiverCheckbox.Location = new Point(146, 3);
      TransceiverCheckbox.Name = "TransceiverCheckbox";
      TransceiverCheckbox.Size = new Size(84, 19);
      TransceiverCheckbox.TabIndex = 6;
      TransceiverCheckbox.Text = "Transceiver";
      TransceiverCheckbox.UseVisualStyleBackColor = true;
      TransceiverCheckbox.Click += Filter_Changed;
      // 
      // TransponderCheckbox
      // 
      TransponderCheckbox.AutoSize = true;
      TransponderCheckbox.Checked = true;
      TransponderCheckbox.CheckState = CheckState.Checked;
      TransponderCheckbox.Location = new Point(49, 3);
      TransponderCheckbox.Name = "TransponderCheckbox";
      TransponderCheckbox.Size = new Size(91, 19);
      TransponderCheckbox.TabIndex = 5;
      TransponderCheckbox.Text = "Transponder";
      TransponderCheckbox.UseVisualStyleBackColor = true;
      TransponderCheckbox.Click += Filter_Changed;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Dock = DockStyle.Left;
      label1.Location = new Point(3, 0);
      label1.Name = "label1";
      label1.Size = new Size(42, 25);
      label1.TabIndex = 4;
      label1.Text = "Status:";
      label1.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // columnHeader3
      // 
      columnHeader3.Text = "Launched";
      columnHeader3.Width = 100;
      // 
      // columnHeader2
      // 
      columnHeader2.Text = "NORAD ID";
      columnHeader2.Width = 80;
      // 
      // columnHeader1
      // 
      columnHeader1.Text = "Name";
      columnHeader1.Width = 180;
      // 
      // ReEnteredCheckbox
      // 
      ReEnteredCheckbox.AutoSize = true;
      ReEnteredCheckbox.Location = new Point(175, 3);
      ReEnteredCheckbox.Name = "ReEnteredCheckbox";
      ReEnteredCheckbox.Size = new Size(84, 19);
      ReEnteredCheckbox.TabIndex = 3;
      ReEnteredCheckbox.Text = "Re-Entered";
      ReEnteredCheckbox.UseVisualStyleBackColor = true;
      ReEnteredCheckbox.CheckedChanged += Filter_Changed;
      // 
      // FutureCheckbox
      // 
      FutureCheckbox.AutoSize = true;
      FutureCheckbox.Checked = true;
      FutureCheckbox.CheckState = CheckState.Checked;
      FutureCheckbox.Location = new Point(109, 3);
      FutureCheckbox.Name = "FutureCheckbox";
      FutureCheckbox.Size = new Size(60, 19);
      FutureCheckbox.TabIndex = 2;
      FutureCheckbox.Text = "Future";
      FutureCheckbox.UseVisualStyleBackColor = true;
      FutureCheckbox.CheckedChanged += Filter_Changed;
      // 
      // panel2
      // 
      panel2.Controls.Add(CountLabel);
      panel2.Dock = DockStyle.Bottom;
      panel2.Location = new Point(0, 355);
      panel2.Name = "panel2";
      panel2.Size = new Size(562, 23);
      panel2.TabIndex = 5;
      // 
      // panel1
      // 
      panel1.Controls.Add(flowLayoutPanel5);
      panel1.Controls.Add(flowLayoutPanel4);
      panel1.Controls.Add(flowLayoutPanel3);
      panel1.Controls.Add(flowLayoutPanel2);
      panel1.Controls.Add(flowLayoutPanel1);
      panel1.Dock = DockStyle.Top;
      panel1.Location = new Point(0, 0);
      panel1.Name = "panel1";
      panel1.Size = new Size(562, 106);
      panel1.TabIndex = 3;
      // 
      // flowLayoutPanel5
      // 
      flowLayoutPanel5.BackColor = Color.Gainsboro;
      flowLayoutPanel5.BorderStyle = BorderStyle.FixedSingle;
      flowLayoutPanel5.Controls.Add(label3);
      flowLayoutPanel5.Controls.Add(FilterTextbox);
      flowLayoutPanel5.Controls.Add(button1);
      flowLayoutPanel5.Controls.Add(UpdatedDateLabel);
      flowLayoutPanel5.Location = new Point(7, 69);
      flowLayoutPanel5.Name = "flowLayoutPanel5";
      flowLayoutPanel5.Size = new Size(546, 30);
      flowLayoutPanel5.TabIndex = 17;
      // 
      // button1
      // 
      button1.Font = new Font("Wingdings 2", 9F, FontStyle.Regular, GraphicsUnit.Point, 2);
      button1.ForeColor = SystemColors.ControlDarkDark;
      button1.Location = new Point(227, 3);
      button1.Name = "button1";
      button1.Size = new Size(23, 22);
      button1.TabIndex = 12;
      button1.Text = "Ò";
      toolTip1.SetToolTip(button1, "Clear Search String");
      button1.UseVisualStyleBackColor = true;
      button1.Click += button1_Click;
      // 
      // UpdatedDateLabel
      // 
      UpdatedDateLabel.Location = new Point(256, 0);
      UpdatedDateLabel.Name = "UpdatedDateLabel";
      UpdatedDateLabel.Size = new Size(281, 23);
      UpdatedDateLabel.TabIndex = 13;
      UpdatedDateLabel.Text = "Updated";
      UpdatedDateLabel.TextAlign = ContentAlignment.BottomRight;
      // 
      // flowLayoutPanel4
      // 
      flowLayoutPanel4.BackColor = Color.Gainsboro;
      flowLayoutPanel4.BorderStyle = BorderStyle.FixedSingle;
      flowLayoutPanel4.Controls.Add(label5);
      flowLayoutPanel4.Controls.Add(HamCheckbox);
      flowLayoutPanel4.Controls.Add(NonHamCheckbox);
      flowLayoutPanel4.Location = new Point(347, 38);
      flowLayoutPanel4.Name = "flowLayoutPanel4";
      flowLayoutPanel4.Size = new Size(206, 25);
      flowLayoutPanel4.TabIndex = 16;
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Dock = DockStyle.Left;
      label5.Location = new Point(3, 0);
      label5.Name = "label5";
      label5.Size = new Size(47, 25);
      label5.TabIndex = 6;
      label5.Text = "Service:";
      label5.TextAlign = ContentAlignment.MiddleLeft;
      label5.Click += Filter_Changed;
      // 
      // HamCheckbox
      // 
      HamCheckbox.AutoSize = true;
      HamCheckbox.Checked = true;
      HamCheckbox.CheckState = CheckState.Checked;
      HamCheckbox.Location = new Point(56, 3);
      HamCheckbox.Name = "HamCheckbox";
      HamCheckbox.Size = new Size(52, 19);
      HamCheckbox.TabIndex = 7;
      HamCheckbox.Text = "Ham";
      HamCheckbox.UseVisualStyleBackColor = true;
      HamCheckbox.Click += Filter_Changed;
      // 
      // NonHamCheckbox
      // 
      NonHamCheckbox.AutoSize = true;
      NonHamCheckbox.Checked = true;
      NonHamCheckbox.CheckState = CheckState.Checked;
      NonHamCheckbox.Location = new Point(114, 3);
      NonHamCheckbox.Name = "NonHamCheckbox";
      NonHamCheckbox.Size = new Size(80, 19);
      NonHamCheckbox.TabIndex = 8;
      NonHamCheckbox.Text = "Non-Ham";
      NonHamCheckbox.UseVisualStyleBackColor = true;
      NonHamCheckbox.Click += Filter_Changed;
      // 
      // flowLayoutPanel3
      // 
      flowLayoutPanel3.BackColor = Color.Gainsboro;
      flowLayoutPanel3.BorderStyle = BorderStyle.FixedSingle;
      flowLayoutPanel3.Controls.Add(label4);
      flowLayoutPanel3.Controls.Add(VhfCheckbox);
      flowLayoutPanel3.Controls.Add(UhfCheckbox);
      flowLayoutPanel3.Controls.Add(OtherBandsCheckbox);
      flowLayoutPanel3.Location = new Point(281, 5);
      flowLayoutPanel3.Name = "flowLayoutPanel3";
      flowLayoutPanel3.Size = new Size(272, 25);
      flowLayoutPanel3.TabIndex = 15;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Dock = DockStyle.Left;
      label4.Location = new Point(3, 0);
      label4.Name = "label4";
      label4.Size = new Size(42, 25);
      label4.TabIndex = 5;
      label4.Text = "Bands:";
      label4.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // VhfCheckbox
      // 
      VhfCheckbox.AutoSize = true;
      VhfCheckbox.Checked = true;
      VhfCheckbox.CheckState = CheckState.Checked;
      VhfCheckbox.Location = new Point(51, 3);
      VhfCheckbox.Name = "VhfCheckbox";
      VhfCheckbox.Size = new Size(48, 19);
      VhfCheckbox.TabIndex = 6;
      VhfCheckbox.Text = "VHF";
      toolTip1.SetToolTip(VhfCheckbox, "Downlink on the 2 M band");
      VhfCheckbox.UseVisualStyleBackColor = true;
      VhfCheckbox.Click += Filter_Changed;
      // 
      // UhfCheckbox
      // 
      UhfCheckbox.AutoSize = true;
      UhfCheckbox.Checked = true;
      UhfCheckbox.CheckState = CheckState.Checked;
      UhfCheckbox.Location = new Point(105, 3);
      UhfCheckbox.Name = "UhfCheckbox";
      UhfCheckbox.Size = new Size(49, 19);
      UhfCheckbox.TabIndex = 7;
      UhfCheckbox.Text = "UHF";
      toolTip1.SetToolTip(UhfCheckbox, "Downlink on the 70 cm band");
      UhfCheckbox.UseVisualStyleBackColor = true;
      UhfCheckbox.Click += Filter_Changed;
      // 
      // OtherBandsCheckbox
      // 
      OtherBandsCheckbox.AutoSize = true;
      OtherBandsCheckbox.Checked = true;
      OtherBandsCheckbox.CheckState = CheckState.Checked;
      OtherBandsCheckbox.Location = new Point(160, 3);
      OtherBandsCheckbox.Name = "OtherBandsCheckbox";
      OtherBandsCheckbox.Size = new Size(56, 19);
      OtherBandsCheckbox.TabIndex = 8;
      OtherBandsCheckbox.Text = "Other";
      toolTip1.SetToolTip(OtherBandsCheckbox, "Downlink on other bands");
      OtherBandsCheckbox.UseVisualStyleBackColor = true;
      OtherBandsCheckbox.Click += Filter_Changed;
      // 
      // flowLayoutPanel2
      // 
      flowLayoutPanel2.BackColor = Color.Gainsboro;
      flowLayoutPanel2.BorderStyle = BorderStyle.FixedSingle;
      flowLayoutPanel2.Controls.Add(label2);
      flowLayoutPanel2.Controls.Add(TransponderCheckbox);
      flowLayoutPanel2.Controls.Add(TransceiverCheckbox);
      flowLayoutPanel2.Controls.Add(TransmitterCheckbox);
      flowLayoutPanel2.Location = new Point(7, 38);
      flowLayoutPanel2.Name = "flowLayoutPanel2";
      flowLayoutPanel2.Size = new Size(332, 25);
      flowLayoutPanel2.TabIndex = 14;
      // 
      // flowLayoutPanel1
      // 
      flowLayoutPanel1.BackColor = Color.Gainsboro;
      flowLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;
      flowLayoutPanel1.Controls.Add(label1);
      flowLayoutPanel1.Controls.Add(AliveCheckbox);
      flowLayoutPanel1.Controls.Add(FutureCheckbox);
      flowLayoutPanel1.Controls.Add(ReEnteredCheckbox);
      flowLayoutPanel1.Location = new Point(7, 5);
      flowLayoutPanel1.Name = "flowLayoutPanel1";
      flowLayoutPanel1.Size = new Size(266, 25);
      flowLayoutPanel1.TabIndex = 13;
      // 
      // AliveCheckbox
      // 
      AliveCheckbox.AutoSize = true;
      AliveCheckbox.Checked = true;
      AliveCheckbox.CheckState = CheckState.Checked;
      AliveCheckbox.Location = new Point(51, 3);
      AliveCheckbox.Name = "AliveCheckbox";
      AliveCheckbox.Size = new Size(52, 19);
      AliveCheckbox.TabIndex = 0;
      AliveCheckbox.Text = "Alive";
      AliveCheckbox.UseVisualStyleBackColor = true;
      AliveCheckbox.CheckedChanged += Filter_Changed;
      // 
      // listView1
      // 
      listView1.Activation = ItemActivation.OneClick;
      listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
      listView1.ContextMenuStrip = SatelliteListPopupMenu;
      listView1.Dock = DockStyle.Fill;
      listView1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      listView1.FullRowSelect = true;
      listView1.LabelEdit = true;
      listView1.Location = new Point(0, 106);
      listView1.MultiSelect = false;
      listView1.Name = "listView1";
      listView1.ShowGroups = false;
      listView1.ShowItemToolTips = true;
      listView1.Size = new Size(562, 249);
      listView1.Sorting = SortOrder.Ascending;
      listView1.TabIndex = 4;
      listView1.UseCompatibleStateImageBehavior = false;
      listView1.View = View.Details;
      listView1.VirtualMode = true;
      listView1.AfterLabelEdit += listView1_AfterLabelEdit;
      listView1.ColumnClick += listView1_ColumnClick;
      listView1.RetrieveVirtualItem += listView1_RetrieveVirtualItem;
      // 
      // columnHeader4
      // 
      columnHeader4.Text = "Service";
      columnHeader4.Width = 170;
      // 
      // SatelliteListPopupMenu
      // 
      SatelliteListPopupMenu.Items.AddRange(new ToolStripItem[] { RenameSatMNU, PropertiesSatMNU });
      SatelliteListPopupMenu.Name = "SatelliteListPopupMenu";
      SatelliteListPopupMenu.Size = new Size(179, 48);
      SatelliteListPopupMenu.Opening += SatelliteListPopupMenu_Opening;
      // 
      // RenameSatMNU
      // 
      RenameSatMNU.Name = "RenameSatMNU";
      RenameSatMNU.ShortcutKeyDisplayString = "F2";
      RenameSatMNU.ShortcutKeys = Keys.F2;
      RenameSatMNU.Size = new Size(178, 22);
      RenameSatMNU.Text = "Rename";
      RenameSatMNU.Click += RenameSatMNU_Click;
      // 
      // PropertiesSatMNU
      // 
      PropertiesSatMNU.Name = "PropertiesSatMNU";
      PropertiesSatMNU.ShortcutKeys = Keys.Control | Keys.D;
      PropertiesSatMNU.Size = new Size(178, 22);
      PropertiesSatMNU.Text = "Properties...";
      PropertiesSatMNU.Click += PropertiesSatMNU_Click;
      // 
      // SatnogsSatListControl
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(listView1);
      Controls.Add(panel2);
      Controls.Add(panel1);
      DoubleBuffered = true;
      Name = "SatnogsSatListControl";
      Size = new Size(562, 378);
      panel2.ResumeLayout(false);
      panel2.PerformLayout();
      panel1.ResumeLayout(false);
      flowLayoutPanel5.ResumeLayout(false);
      flowLayoutPanel5.PerformLayout();
      flowLayoutPanel4.ResumeLayout(false);
      flowLayoutPanel4.PerformLayout();
      flowLayoutPanel3.ResumeLayout(false);
      flowLayoutPanel3.PerformLayout();
      flowLayoutPanel2.ResumeLayout(false);
      flowLayoutPanel2.PerformLayout();
      flowLayoutPanel1.ResumeLayout(false);
      flowLayoutPanel1.PerformLayout();
      SatelliteListPopupMenu.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private Label CountLabel;
    private Label label3;
    private TextBox FilterTextbox;
    private Label label2;
    private CheckBox TransmitterCheckbox;
    private CheckBox TransceiverCheckbox;
    private CheckBox TransponderCheckbox;
    private Label label1;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader1;
    private CheckBox ReEnteredCheckbox;
    private CheckBox FutureCheckbox;
    private Panel panel2;
    private Panel panel1;
    private CheckBox AliveCheckbox;
    private ColumnHeader columnHeader4;
    public ListView listView1;
    private FlowLayoutPanel flowLayoutPanel2;
    private FlowLayoutPanel flowLayoutPanel1;
    private FlowLayoutPanel flowLayoutPanel4;
    private Label label5;
    private CheckBox HamCheckbox;
    private CheckBox NonHamCheckbox;
    private FlowLayoutPanel flowLayoutPanel3;
    private Label label4;
    private CheckBox VhfCheckbox;
    private CheckBox UhfCheckbox;
    private CheckBox OtherBandsCheckbox;
    private FlowLayoutPanel flowLayoutPanel5;
    private ToolTip toolTip1;
    private Button button1;
    private Label UpdatedDateLabel;
    private ContextMenuStrip SatelliteListPopupMenu;
    private ToolStripMenuItem RenameSatMNU;
    private ToolStripMenuItem PropertiesSatMNU;
  }
}
