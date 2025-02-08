namespace OrbiNom
{
  partial class TransmittersPanel
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
      listView1 = new ListView();
      columnHeader1 = new ColumnHeader();
      columnHeader2 = new ColumnHeader();
      columnHeader3 = new ColumnHeader();
      SatNameLabel = new Label();
      SuspendLayout();
      // 
      // listView1
      // 
      listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
      listView1.Dock = DockStyle.Fill;
      listView1.FullRowSelect = true;
      listView1.LabelWrap = false;
      listView1.Location = new Point(0, 23);
      listView1.MultiSelect = false;
      listView1.Name = "listView1";
      listView1.ShowItemToolTips = true;
      listView1.Size = new Size(471, 325);
      listView1.Sorting = SortOrder.Ascending;
      listView1.TabIndex = 4;
      listView1.UseCompatibleStateImageBehavior = false;
      listView1.View = View.Details;
      listView1.ColumnClick += listView1_ColumnClick;
      // 
      // columnHeader1
      // 
      columnHeader1.Text = "Transmitter";
      columnHeader1.Width = 200;
      // 
      // columnHeader2
      // 
      columnHeader2.Text = "Uplink";
      columnHeader2.Width = 120;
      // 
      // columnHeader3
      // 
      columnHeader3.Text = "Downlink";
      columnHeader3.Width = 120;
      // 
      // SatNameLabel
      // 
      SatNameLabel.Dock = DockStyle.Top;
      SatNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
      SatNameLabel.Location = new Point(0, 0);
      SatNameLabel.Name = "SatNameLabel";
      SatNameLabel.Size = new Size(471, 23);
      SatNameLabel.TabIndex = 10;
      SatNameLabel.Text = "___";
      SatNameLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // TransmittersPanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(471, 348);
      Controls.Add(listView1);
      Controls.Add(SatNameLabel);
      Name = "TransmittersPanel";
      Text = "Satellite Transmitters";
      FormClosing += TransmittersPanel_FormClosing;
      Load += TransmittersPanel_Load;
      ResumeLayout(false);
    }

    #endregion

    private ListView listView1;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    public Label SatNameLabel;
  }
}