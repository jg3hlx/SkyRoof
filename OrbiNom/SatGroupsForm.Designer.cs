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
      SatellitesListView = new SatnogsSatListControl();
      label1 = new Label();
      ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
      panel2.SuspendLayout();
      panel3.SuspendLayout();
      panel5.SuspendLayout();
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
      panel5.Controls.Add(SatellitesListView);
      panel5.Controls.Add(label1);
      panel5.Dock = DockStyle.Left;
      panel5.Location = new Point(0, 0);
      panel5.Name = "panel5";
      panel5.Size = new Size(559, 822);
      panel5.TabIndex = 3;
      // 
      // SatellitesListView
      // 
      SatellitesListView.Dock = DockStyle.Fill;
      SatellitesListView.Location = new Point(0, 18);
      SatellitesListView.Name = "SatellitesListView";
      SatellitesListView.Size = new Size(559, 804);
      SatellitesListView.TabIndex = 1;
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
      ResumeLayout(false);
    }

    #endregion

    private BindingSource bindingSource1;
    private SatnogsSatListControl satnogsSatListControl1;
    private Panel panel1;
    private Panel panel2;
    private TreeView treeView1;
    private Panel panel4;
    private Panel panel3;
    private Panel panel5;
    public SatnogsSatListControl SatellitesListView;
    private Label label1;
    private Label label2;
    private Button button2;
    private Button button1;
  }
}