namespace OrbiNom
{
  partial class SatGroupsForm
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
      components = new System.ComponentModel.Container();
      bindingSource1 = new BindingSource(components);
      panel1 = new Panel();
      panel2 = new Panel();
      treeView1 = new TreeView();
      panel4 = new Panel();
      panel3 = new Panel();
      button2 = new Button();
      button1 = new Button();
      label2 = new Label();
      panel5 = new Panel();
      listView1 = new ListView();
      columnHeader1 = new ColumnHeader();
      columnHeader2 = new ColumnHeader();
      columnHeader3 = new ColumnHeader();
      columnHeader4 = new ColumnHeader();
      SatelliteListPopupMenu = new ContextMenuStrip(components);
      RenameSatMNU = new ToolStripMenuItem();
      PropertiesSatMNU = new ToolStripMenuItem();
      panel7 = new Panel();
      CountLabel = new Label();
      panel6 = new Panel();
      flowLayoutPanel5 = new FlowLayoutPanel();
      label3 = new Label();
      FilterTextbox = new TextBox();
      button3 = new Button();
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
      label6 = new Label();
      TransponderCheckbox = new CheckBox();
      TransceiverCheckbox = new CheckBox();
      TransmitterCheckbox = new CheckBox();
      flowLayoutPanel1 = new FlowLayoutPanel();
      label7 = new Label();
      AliveCheckbox = new CheckBox();
      FutureCheckbox = new CheckBox();
      ReEnteredCheckbox = new CheckBox();
      label1 = new Label();
      ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
      panel2.SuspendLayout();
      panel3.SuspendLayout();
      panel5.SuspendLayout();
      SatelliteListPopupMenu.SuspendLayout();
      panel7.SuspendLayout();
      panel6.SuspendLayout();
      flowLayoutPanel5.SuspendLayout();
      flowLayoutPanel4.SuspendLayout();
      flowLayoutPanel3.SuspendLayout();
      flowLayoutPanel2.SuspendLayout();
      flowLayoutPanel1.SuspendLayout();
      SuspendLayout();
      // 
      // panel1
      // 
      panel1.Dock = DockStyle.Left;
      panel1.Location = new Point(559, 0);
      panel1.Name = "panel1";
      panel1.Size = new Size(6, 822);
      panel1.TabIndex = 1;
      // 
      // panel2
      // 
      panel2.Controls.Add(treeView1);
      panel2.Controls.Add(panel4);
      panel2.Controls.Add(panel3);
      panel2.Controls.Add(label2);
      panel2.Dock = DockStyle.Fill;
      panel2.Location = new Point(565, 0);
      panel2.Name = "panel2";
      panel2.Size = new Size(291, 822);
      panel2.TabIndex = 2;
      // 
      // treeView1
      // 
      treeView1.Dock = DockStyle.Fill;
      treeView1.Location = new Point(0, 69);
      treeView1.Name = "treeView1";
      treeView1.Size = new Size(291, 729);
      treeView1.TabIndex = 1;
      // 
      // panel4
      // 
      panel4.Dock = DockStyle.Bottom;
      panel4.Location = new Point(0, 798);
      panel4.Name = "panel4";
      panel4.Size = new Size(291, 24);
      panel4.TabIndex = 2;
      // 
      // panel3
      // 
      panel3.Controls.Add(button2);
      panel3.Controls.Add(button1);
      panel3.Dock = DockStyle.Top;
      panel3.Location = new Point(0, 26);
      panel3.Name = "panel3";
      panel3.Size = new Size(291, 43);
      panel3.TabIndex = 0;
      // 
      // button2
      // 
      button2.Location = new Point(59, 3);
      button2.Name = "button2";
      button2.Size = new Size(38, 36);
      button2.TabIndex = 1;
      button2.UseVisualStyleBackColor = true;
      // 
      // button1
      // 
      button1.Location = new Point(11, 3);
      button1.Name = "button1";
      button1.Size = new Size(38, 36);
      button1.TabIndex = 0;
      button1.UseVisualStyleBackColor = true;
      // 
      // label2
      // 
      label2.Dock = DockStyle.Top;
      label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      label2.Location = new Point(0, 0);
      label2.Name = "label2";
      label2.Size = new Size(291, 26);
      label2.TabIndex = 3;
      label2.Text = " Groups";
      // 
      // panel5
      // 
      panel5.Controls.Add(listView1);
      panel5.Controls.Add(panel7);
      panel5.Controls.Add(panel6);
      panel5.Controls.Add(label1);
      panel5.Dock = DockStyle.Left;
      panel5.Location = new Point(0, 0);
      panel5.Name = "panel5";
      panel5.Size = new Size(559, 822);
      panel5.TabIndex = 3;
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
      listView1.Location = new Point(0, 124);
      listView1.MultiSelect = false;
      listView1.Name = "listView1";
      listView1.ShowGroups = false;
      listView1.ShowItemToolTips = true;
      listView1.Size = new Size(559, 675);
      listView1.Sorting = SortOrder.Ascending;
      listView1.TabIndex = 7;
      listView1.UseCompatibleStateImageBehavior = false;
      listView1.View = View.Details;
      listView1.VirtualMode = true;
      listView1.AfterLabelEdit += listView1_AfterLabelEdit;
      listView1.ColumnClick += listView1_ColumnClick;
      listView1.RetrieveVirtualItem += listView1_RetrieveVirtualItem;
      listView1.DoubleClick += PropertiesSatMNU_Click;
      // 
      // columnHeader1
      // 
      columnHeader1.Text = "Name";
      columnHeader1.Width = 180;
      // 
      // columnHeader2
      // 
      columnHeader2.Text = "NORAD ID";
      columnHeader2.Width = 80;
      // 
      // columnHeader3
      // 
      columnHeader3.Text = "Launched";
      columnHeader3.Width = 100;
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
      // 
      // RenameSatMNU
      // 
      RenameSatMNU.Name = "RenameSatMNU";
      RenameSatMNU.ShortcutKeyDisplayString = "";
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
      // panel7
      // 
      panel7.Controls.Add(CountLabel);
      panel7.Dock = DockStyle.Bottom;
      panel7.Location = new Point(0, 799);
      panel7.Name = "panel7";
      panel7.Size = new Size(559, 23);
      panel7.TabIndex = 6;
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
      // panel6
      // 
      panel6.Controls.Add(flowLayoutPanel5);
      panel6.Controls.Add(flowLayoutPanel4);
      panel6.Controls.Add(flowLayoutPanel3);
      panel6.Controls.Add(flowLayoutPanel2);
      panel6.Controls.Add(flowLayoutPanel1);
      panel6.Dock = DockStyle.Top;
      panel6.Location = new Point(0, 18);
      panel6.Name = "panel6";
      panel6.Size = new Size(559, 106);
      panel6.TabIndex = 4;
      // 
      // flowLayoutPanel5
      // 
      flowLayoutPanel5.BackColor = Color.Gainsboro;
      flowLayoutPanel5.BorderStyle = BorderStyle.FixedSingle;
      flowLayoutPanel5.Controls.Add(label3);
      flowLayoutPanel5.Controls.Add(FilterTextbox);
      flowLayoutPanel5.Controls.Add(button3);
      flowLayoutPanel5.Controls.Add(UpdatedDateLabel);
      flowLayoutPanel5.Location = new Point(7, 69);
      flowLayoutPanel5.Name = "flowLayoutPanel5";
      flowLayoutPanel5.Size = new Size(546, 30);
      flowLayoutPanel5.TabIndex = 17;
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
      FilterTextbox.TextChanged += FilterChanged;
      // 
      // button3
      // 
      button3.Font = new Font("Wingdings 2", 9F, FontStyle.Regular, GraphicsUnit.Point, 2);
      button3.ForeColor = SystemColors.ControlDarkDark;
      button3.Location = new Point(227, 3);
      button3.Name = "button3";
      button3.Size = new Size(23, 22);
      button3.TabIndex = 12;
      button3.Text = "Ò";
      button3.UseVisualStyleBackColor = true;
      button3.Click += button3_Click;
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
      HamCheckbox.CheckedChanged += FilterChanged;
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
      NonHamCheckbox.CheckedChanged += FilterChanged;
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
      VhfCheckbox.UseVisualStyleBackColor = true;
      VhfCheckbox.CheckedChanged += FilterChanged;
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
      UhfCheckbox.UseVisualStyleBackColor = true;
      UhfCheckbox.CheckedChanged += FilterChanged;
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
      OtherBandsCheckbox.UseVisualStyleBackColor = true;
      OtherBandsCheckbox.CheckedChanged += FilterChanged;
      // 
      // flowLayoutPanel2
      // 
      flowLayoutPanel2.BackColor = Color.Gainsboro;
      flowLayoutPanel2.BorderStyle = BorderStyle.FixedSingle;
      flowLayoutPanel2.Controls.Add(label6);
      flowLayoutPanel2.Controls.Add(TransponderCheckbox);
      flowLayoutPanel2.Controls.Add(TransceiverCheckbox);
      flowLayoutPanel2.Controls.Add(TransmitterCheckbox);
      flowLayoutPanel2.Location = new Point(7, 38);
      flowLayoutPanel2.Name = "flowLayoutPanel2";
      flowLayoutPanel2.Size = new Size(332, 25);
      flowLayoutPanel2.TabIndex = 14;
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Dock = DockStyle.Left;
      label6.Location = new Point(3, 0);
      label6.Name = "label6";
      label6.Size = new Size(40, 25);
      label6.TabIndex = 8;
      label6.Text = "Radio:";
      label6.TextAlign = ContentAlignment.MiddleLeft;
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
      TransponderCheckbox.CheckedChanged += FilterChanged;
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
      TransceiverCheckbox.CheckedChanged += FilterChanged;
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
      TransmitterCheckbox.CheckedChanged += FilterChanged;
      // 
      // flowLayoutPanel1
      // 
      flowLayoutPanel1.BackColor = Color.Gainsboro;
      flowLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;
      flowLayoutPanel1.Controls.Add(label7);
      flowLayoutPanel1.Controls.Add(AliveCheckbox);
      flowLayoutPanel1.Controls.Add(FutureCheckbox);
      flowLayoutPanel1.Controls.Add(ReEnteredCheckbox);
      flowLayoutPanel1.Location = new Point(7, 5);
      flowLayoutPanel1.Name = "flowLayoutPanel1";
      flowLayoutPanel1.Size = new Size(266, 25);
      flowLayoutPanel1.TabIndex = 13;
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Dock = DockStyle.Left;
      label7.Location = new Point(3, 0);
      label7.Name = "label7";
      label7.Size = new Size(42, 25);
      label7.TabIndex = 4;
      label7.Text = "Status:";
      label7.TextAlign = ContentAlignment.MiddleLeft;
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
      AliveCheckbox.CheckedChanged += FilterChanged;
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
      FutureCheckbox.CheckedChanged += FilterChanged;
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
      ReEnteredCheckbox.CheckedChanged += FilterChanged;
      // 
      // label1
      // 
      label1.Dock = DockStyle.Top;
      label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      label1.Location = new Point(0, 0);
      label1.Name = "label1";
      label1.Size = new Size(559, 18);
      label1.TabIndex = 2;
      label1.Text = " Satellites";
      // 
      // SatGroupsForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(856, 822);
      Controls.Add(panel2);
      Controls.Add(panel1);
      Controls.Add(panel5);
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "SatGroupsForm";
      ShowIcon = false;
      ShowInTaskbar = false;
      StartPosition = FormStartPosition.CenterParent;
      Text = "Edit Satellite Groups";
      ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
      panel2.ResumeLayout(false);
      panel3.ResumeLayout(false);
      panel5.ResumeLayout(false);
      SatelliteListPopupMenu.ResumeLayout(false);
      panel7.ResumeLayout(false);
      panel7.PerformLayout();
      panel6.ResumeLayout(false);
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
      ResumeLayout(false);
    }

    #endregion

    private BindingSource bindingSource1;
    //private SatnogsSatListControl satnogsSatListControl1;
    private Panel panel1;
    private Panel panel2;
    private TreeView treeView1;
    private Panel panel4;
    private Panel panel3;
    private Panel panel5;
    private Label label1;
    private Label label2;
    private Button button2;
    private Button button1;
    private Panel panel6;
    private FlowLayoutPanel flowLayoutPanel5;
    private Label label3;
    private TextBox FilterTextbox;
    private Button button3;
    private Label UpdatedDateLabel;
    private FlowLayoutPanel flowLayoutPanel4;
    private Label label5;
    private CheckBox HamCheckbox;
    private CheckBox NonHamCheckbox;
    private FlowLayoutPanel flowLayoutPanel3;
    private Label label4;
    private CheckBox VhfCheckbox;
    private CheckBox UhfCheckbox;
    private CheckBox OtherBandsCheckbox;
    private FlowLayoutPanel flowLayoutPanel2;
    private Label label6;
    private CheckBox TransponderCheckbox;
    private CheckBox TransceiverCheckbox;
    private CheckBox TransmitterCheckbox;
    private FlowLayoutPanel flowLayoutPanel1;
    private Label label7;
    private CheckBox AliveCheckbox;
    private CheckBox FutureCheckbox;
    private CheckBox ReEnteredCheckbox;
    private Panel panel7;
    private Label CountLabel;
    public ListView listView1;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;
    private ContextMenuStrip SatelliteListPopupMenu;
    private ToolStripMenuItem RenameSatMNU;
    private ToolStripMenuItem PropertiesSatMNU;
  }
}