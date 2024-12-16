namespace OrbiNom
{
  partial class SatelliteDetailsControl
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
      splitContainer1 = new SplitContainer();
      SatAkaLabel = new Label();
      SatellitePropertyGrid = new PropertyGrid();
      SatNameLabel = new Label();
      label2 = new Label();
      ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
      splitContainer1.Panel1.SuspendLayout();
      splitContainer1.Panel2.SuspendLayout();
      splitContainer1.SuspendLayout();
      SuspendLayout();
      // 
      // splitContainer1
      // 
      splitContainer1.Dock = DockStyle.Fill;
      splitContainer1.Location = new Point(0, 0);
      splitContainer1.Name = "splitContainer1";
      splitContainer1.Orientation = Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      splitContainer1.Panel1.Controls.Add(SatAkaLabel);
      splitContainer1.Panel1.Controls.Add(SatellitePropertyGrid);
      splitContainer1.Panel1.Controls.Add(SatNameLabel);
      // 
      // splitContainer1.Panel2
      // 
      splitContainer1.Panel2.Controls.Add(label2);
      splitContainer1.Size = new Size(346, 418);
      splitContainer1.SplitterDistance = 279;
      splitContainer1.TabIndex = 1;
      // 
      // SatAkaLabel
      // 
      SatAkaLabel.Dock = DockStyle.Top;
      SatAkaLabel.Location = new Point(0, 23);
      SatAkaLabel.Name = "SatAkaLabel";
      SatAkaLabel.Size = new Size(346, 23);
      SatAkaLabel.TabIndex = 3;
      SatAkaLabel.Text = "___";
      SatAkaLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // SatellitePropertyGrid
      // 
      SatellitePropertyGrid.DisabledItemForeColor = SystemColors.WindowText;
      SatellitePropertyGrid.Dock = DockStyle.Fill;
      SatellitePropertyGrid.HelpVisible = false;
      SatellitePropertyGrid.Location = new Point(0, 23);
      SatellitePropertyGrid.Name = "SatellitePropertyGrid";
      SatellitePropertyGrid.PropertySort = PropertySort.Alphabetical;
      SatellitePropertyGrid.Size = new Size(346, 256);
      SatellitePropertyGrid.TabIndex = 2;
      SatellitePropertyGrid.ToolbarVisible = false;
      // 
      // SatNameLabel
      // 
      SatNameLabel.Dock = DockStyle.Top;
      SatNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
      SatNameLabel.Location = new Point(0, 0);
      SatNameLabel.Name = "SatNameLabel";
      SatNameLabel.Size = new Size(346, 23);
      SatNameLabel.TabIndex = 1;
      SatNameLabel.Text = "___";
      SatNameLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // label2
      // 
      label2.Dock = DockStyle.Top;
      label2.Font = new Font("Segoe UI", 9F);
      label2.Location = new Point(0, 0);
      label2.Name = "label2";
      label2.Size = new Size(346, 23);
      label2.TabIndex = 1;
      label2.Text = "Transmitters";
      label2.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // SatelliteDetailsControl
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(splitContainer1);
      Name = "SatelliteDetailsControl";
      Size = new Size(346, 418);
      splitContainer1.Panel1.ResumeLayout(false);
      splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
      splitContainer1.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer1;
    private Label label2;
    public PropertyGrid SatellitePropertyGrid;
    public Label SatNameLabel;
    public Label SatAkaLabel;
  }
}
