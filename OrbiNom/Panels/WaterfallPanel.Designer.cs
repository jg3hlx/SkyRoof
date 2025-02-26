namespace OrbiNom
{
  partial class WaterfallPanel
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaterfallPanel));
      splitContainer1 = new SplitContainer();
      ScaleControl = new FrequencyScale();
      WaterfallControl = new WaterfallControl();
      SlidersBtn = new Button();
      ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
      splitContainer1.Panel1.SuspendLayout();
      splitContainer1.Panel2.SuspendLayout();
      splitContainer1.SuspendLayout();
      SuspendLayout();
      // 
      // splitContainer1
      // 
      splitContainer1.Dock = DockStyle.Fill;
      splitContainer1.FixedPanel = FixedPanel.Panel1;
      splitContainer1.Location = new Point(0, 0);
      splitContainer1.Name = "splitContainer1";
      splitContainer1.Orientation = Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      splitContainer1.Panel1.Controls.Add(SlidersBtn);
      splitContainer1.Panel1.Controls.Add(ScaleControl);
      // 
      // splitContainer1.Panel2
      // 
      splitContainer1.Panel2.Controls.Add(WaterfallControl);
      splitContainer1.Size = new Size(800, 450);
      splitContainer1.SplitterDistance = 79;
      splitContainer1.TabIndex = 0;
      // 
      // ScaleControl
      // 
      ScaleControl.Dock = DockStyle.Fill;
      ScaleControl.Location = new Point(0, 0);
      ScaleControl.Name = "ScaleControl";
      ScaleControl.Size = new Size(800, 79);
      ScaleControl.TabIndex = 0;
      // 
      // WaterfallControl
      // 
      WaterfallControl.Dock = DockStyle.Fill;
      WaterfallControl.Location = new Point(0, 0);
      WaterfallControl.Name = "WaterfallControl";
      WaterfallControl.Size = new Size(800, 367);
      WaterfallControl.TabIndex = 0;
      // 
      // SlidersBtn
      // 
      SlidersBtn.BackColor = SystemColors.Control;
      SlidersBtn.FlatStyle = FlatStyle.Popup;
      SlidersBtn.ForeColor = Color.SteelBlue;
      SlidersBtn.Image = (Image)resources.GetObject("SlidersBtn.Image");
      SlidersBtn.Location = new Point(3, 3);
      SlidersBtn.Name = "SlidersBtn";
      SlidersBtn.Size = new Size(31, 28);
      SlidersBtn.TabIndex = 4;
      SlidersBtn.UseVisualStyleBackColor = false;
      SlidersBtn.Click += SlidersBtn_Click;
      // 
      // WaterfallPanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(splitContainer1);
      Name = "WaterfallPanel";
      Text = "Waterfall";
      FormClosing += WaterfallPanel_FormClosing;
      splitContainer1.Panel1.ResumeLayout(false);
      splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
      splitContainer1.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer1;
    public WaterfallControl WaterfallControl;
    public FrequencyScale ScaleControl;
    private Button SlidersBtn;
  }
}