namespace OrbiNom
{
  partial class TimelinePanel
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
      toolTip1 = new ToolTip(components);
      SuspendLayout();
      // 
      // toolTip1
      // 
      toolTip1.AutomaticDelay = 1500;
      toolTip1.AutoPopDelay = 15000;
      toolTip1.InitialDelay = 500;
      toolTip1.ReshowDelay = 300;
      toolTip1.ShowAlways = true;
      // 
      // TimelinePanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 189);
      DoubleBuffered = true;
      Name = "TimelinePanel";
      Text = "Timeline";
      FormClosing += TimelinePanel_FormClosing;
      Paint += TimelinePanel_Paint;
      MouseDown += SatelliteTimelineControl_MouseDown;
      MouseMove += SatelliteTimelineControl_MouseMove;
      MouseUp += SatelliteTimelineControl_MouseUp;
      Resize += TimelinePanel_Resize;
      ResumeLayout(false);
    }

    #endregion

    private ToolTip toolTip1;
  }
}