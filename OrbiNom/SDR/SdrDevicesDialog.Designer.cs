using SharpGL.SceneGraph.Primitives;

namespace OrbiNom
{
  partial class SdrDevicesDialog
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
      okBtn = new Button();
      cancelBtn = new Button();
      label1 = new Label();
      Grid = new PropertyGrid();
      PropertyGridMenu = new ContextMenuStrip(components);
      resetToolStripMenuItem = new ToolStripMenuItem();
      imageList1 = new ImageList(components);
      toolTip = new ToolTip(components);
      listBox1 = new ListBox();
      PropertyGridMenu.SuspendLayout();
      SuspendLayout();
      // 
      // okBtn
      // 
      okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      okBtn.DialogResult = DialogResult.OK;
      okBtn.Location = new Point(350, 324);
      okBtn.Margin = new Padding(4, 3, 4, 3);
      okBtn.Name = "okBtn";
      okBtn.Size = new Size(88, 27);
      okBtn.TabIndex = 6;
      okBtn.Text = "OK";
      okBtn.UseVisualStyleBackColor = true;
      // 
      // cancelBtn
      // 
      cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      cancelBtn.DialogResult = DialogResult.Cancel;
      cancelBtn.Location = new Point(446, 324);
      cancelBtn.Margin = new Padding(4, 3, 4, 3);
      cancelBtn.Name = "cancelBtn";
      cancelBtn.Size = new Size(88, 27);
      cancelBtn.TabIndex = 7;
      cancelBtn.Text = "Cancel";
      cancelBtn.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(286, 12);
      label1.Name = "label1";
      label1.Size = new Size(71, 15);
      label1.TabIndex = 8;
      label1.Text = "SDR DEvices";
      // 
      // Grid
      // 
      Grid.BackColor = SystemColors.Control;
      Grid.ContextMenuStrip = PropertyGridMenu;
      Grid.Location = new Point(13, 34);
      Grid.Margin = new Padding(4, 3, 4, 3);
      Grid.Name = "Grid";
      Grid.Size = new Size(249, 313);
      Grid.TabIndex = 1;
      Grid.ToolbarVisible = false;
      // 
      // PropertyGridMenu
      // 
      PropertyGridMenu.Items.AddRange(new ToolStripItem[] { resetToolStripMenuItem });
      PropertyGridMenu.Name = "PropertyGridMenu";
      PropertyGridMenu.Size = new Size(103, 26);
      // 
      // resetToolStripMenuItem
      // 
      resetToolStripMenuItem.Name = "resetToolStripMenuItem";
      resetToolStripMenuItem.Size = new Size(102, 22);
      resetToolStripMenuItem.Text = "Reset";
      resetToolStripMenuItem.Click += ResetMNU_Click;
      // 
      // imageList1
      // 
      imageList1.ColorDepth = ColorDepth.Depth32Bit;
      imageList1.ImageSize = new Size(16, 16);
      imageList1.TransparentColor = Color.Transparent;
      // 
      // listBox1
      // 
      listBox1.DisplayMember = "Name";
      listBox1.DrawMode = DrawMode.OwnerDrawFixed;
      listBox1.Location = new Point(269, 34);
      listBox1.Name = "listBox1";
      listBox1.Size = new Size(268, 132);
      listBox1.TabIndex = 9;
      listBox1.DrawItem += listBox1_DrawItem;
      listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
      listBox1.MouseDown += listBox1_MouseDown;
      // 
      // SdrDevicesDialog
      // 
      AcceptButton = okBtn;
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      CancelButton = cancelBtn;
      ClientSize = new Size(547, 363);
      Controls.Add(listBox1);
      Controls.Add(Grid);
      Controls.Add(label1);
      Controls.Add(cancelBtn);
      Controls.Add(okBtn);
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "SdrDevicesDialog";
      StartPosition = FormStartPosition.CenterParent;
      Text = "SDR Devices";
      FormClosing += SdrDevicesDialog_FormClosing;
      PropertyGridMenu.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Button okBtn;
    private Button cancelBtn;
    private Label label1;
    private PropertyGrid Grid;
    private ImageList imageList1;
    private ToolTip toolTip;
    private ContextMenuStrip PropertyGridMenu;
    private ToolStripMenuItem resetToolStripMenuItem;
    private ListBox listBox1;
  }
}