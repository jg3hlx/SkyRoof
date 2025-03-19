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
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaterfallPanel));
      SplitContainer = new SplitContainer();
      SlidersBtn = new Button();
      ScaleControl = new FrequencyScale();
      contextMenuStrip1 = new ContextMenuStrip(components);
      SelectTransmitterMNU = new ToolStripMenuItem();
      AddToGroupMNU = new ToolStripMenuItem();
      ReportToAmsatMNU = new ToolStripMenuItem();
      WaterfallControl = new WaterfallControl();
      toolTip1 = new ToolTip(components);
      ((System.ComponentModel.ISupportInitialize)SplitContainer).BeginInit();
      SplitContainer.Panel1.SuspendLayout();
      SplitContainer.Panel2.SuspendLayout();
      SplitContainer.SuspendLayout();
      contextMenuStrip1.SuspendLayout();
      SuspendLayout();
      // 
      // SplitContainer
      // 
      SplitContainer.Dock = DockStyle.Fill;
      SplitContainer.FixedPanel = FixedPanel.Panel1;
      SplitContainer.Location = new Point(0, 0);
      SplitContainer.Name = "SplitContainer";
      SplitContainer.Orientation = Orientation.Horizontal;
      // 
      // SplitContainer.Panel1
      // 
      SplitContainer.Panel1.Controls.Add(SlidersBtn);
      SplitContainer.Panel1.Controls.Add(ScaleControl);
      // 
      // SplitContainer.Panel2
      // 
      SplitContainer.Panel2.Controls.Add(WaterfallControl);
      SplitContainer.Size = new Size(800, 450);
      SplitContainer.SplitterDistance = 79;
      SplitContainer.TabIndex = 0;
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
      // ScaleControl
      // 
      ScaleControl.ContextMenuStrip = contextMenuStrip1;
      ScaleControl.Dock = DockStyle.Fill;
      ScaleControl.Location = new Point(0, 0);
      ScaleControl.Name = "ScaleControl";
      ScaleControl.Size = new Size(800, 79);
      ScaleControl.TabIndex = 0;
      // 
      // contextMenuStrip1
      // 
      contextMenuStrip1.Items.AddRange(new ToolStripItem[] { SelectTransmitterMNU, AddToGroupMNU, ReportToAmsatMNU });
      contextMenuStrip1.Name = "contextMenuStrip1";
      contextMenuStrip1.Size = new Size(174, 70);
      contextMenuStrip1.Opening += contextMenuStrip1_Opening;
      // 
      // SelectTransmitterMNU
      // 
      SelectTransmitterMNU.Name = "SelectTransmitterMNU";
      SelectTransmitterMNU.Size = new Size(173, 22);
      SelectTransmitterMNU.Text = "Select Transmitter";
      // 
      // AddToGroupMNU
      // 
      AddToGroupMNU.Name = "AddToGroupMNU";
      AddToGroupMNU.Size = new Size(173, 22);
      AddToGroupMNU.Text = "Add to Group";
      // 
      // ReportToAmsatMNU
      // 
      ReportToAmsatMNU.Name = "ReportToAmsatMNU";
      ReportToAmsatMNU.Size = new Size(173, 22);
      ReportToAmsatMNU.Text = "Report to AMSAT...";
      ReportToAmsatMNU.Click += ReportToAmsatMNU_Click;
      // 
      // WaterfallControl
      // 
      WaterfallControl.Dock = DockStyle.Fill;
      WaterfallControl.Location = new Point(0, 0);
      WaterfallControl.Name = "WaterfallControl";
      WaterfallControl.Size = new Size(800, 367);
      WaterfallControl.TabIndex = 0;
      WaterfallControl.Resize += WaterfallControl_Resize;
      // 
      // WaterfallPanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(SplitContainer);
      Name = "WaterfallPanel";
      Text = "Waterfall";
      FormClosing += WaterfallPanel_FormClosing;
      SplitContainer.Panel1.ResumeLayout(false);
      SplitContainer.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)SplitContainer).EndInit();
      SplitContainer.ResumeLayout(false);
      contextMenuStrip1.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer1;
    public WaterfallControl WaterfallControl;
    public FrequencyScale ScaleControl;
    private Button SlidersBtn;
    public SplitContainer SplitContainer;
    private ToolTip toolTip1;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem SelectTransmitterMNU;
    private ToolStripMenuItem AddToGroupMNU;
    private ToolStripMenuItem ReportToAmsatMNU;
  }
}