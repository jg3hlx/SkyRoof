namespace OrbiNom
{
  partial class GroupViewPanel
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupViewPanel));
      listView1 = new ListView();
      columnHeader1 = new ColumnHeader();
      columnHeader2 = new ColumnHeader();
      columnHeader3 = new ColumnHeader();
      imageList1 = new ImageList(components);
      GroupNameLabel = new Label();
      SuspendLayout();
      // 
      // listView1
      // 
      listView1.Activation = ItemActivation.OneClick;
      listView1.AllowDrop = true;
      listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
      listView1.Dock = DockStyle.Fill;
      listView1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
      listView1.FullRowSelect = true;
      listView1.LabelEdit = true;
      listView1.Location = new Point(0, 23);
      listView1.Name = "listView1";
      listView1.ShowGroups = false;
      listView1.ShowItemToolTips = true;
      listView1.Size = new Size(396, 229);
      listView1.SmallImageList = imageList1;
      listView1.Sorting = SortOrder.Ascending;
      listView1.TabIndex = 8;
      listView1.UseCompatibleStateImageBehavior = false;
      listView1.View = View.Details;
      listView1.VirtualMode = true;
      listView1.RetrieveVirtualItem += listView1_RetrieveVirtualItem;
      listView1.DoubleClick += listView1_DoubleClick;
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
      columnHeader3.Text = "Next Pass";
      columnHeader3.Width = 100;
      // 
      // imageList1
      // 
      imageList1.ColorDepth = ColorDepth.Depth32Bit;
      imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
      imageList1.TransparentColor = Color.Transparent;
      imageList1.Images.SetKeyName(0, "Arrow2 Right.png");
      imageList1.Images.SetKeyName(1, "checkmark.bmp");
      // 
      // GroupNameLabel
      // 
      GroupNameLabel.Dock = DockStyle.Top;
      GroupNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
      GroupNameLabel.Location = new Point(0, 0);
      GroupNameLabel.Name = "GroupNameLabel";
      GroupNameLabel.Size = new Size(396, 23);
      GroupNameLabel.TabIndex = 9;
      GroupNameLabel.Text = "___";
      GroupNameLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // GroupViewPanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(396, 252);
      Controls.Add(listView1);
      Controls.Add(GroupNameLabel);
      Name = "GroupViewPanel";
      Text = "Current Satellite Group";
      FormClosing += GroupViewPanel_FormClosing;
      ResumeLayout(false);
    }

    #endregion

    public ListView listView1;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    public Label GroupNameLabel;
    private ImageList imageList1;
  }
}