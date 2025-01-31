namespace OrbiNom
{
  partial class EarthViewPanel
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
      openglControl1 = new SharpGL.OpenGLControl();
      RadioButtonsPanel = new FlowLayoutPanel();
      SatelliteRadioBtn = new RadioButton();
      OrbitRadioBtn = new RadioButton();
      ((System.ComponentModel.ISupportInitialize)openglControl1).BeginInit();
      RadioButtonsPanel.SuspendLayout();
      SuspendLayout();
      // 
      // openglControl1
      // 
      openglControl1.Dock = DockStyle.Fill;
      openglControl1.DrawFPS = false;
      openglControl1.Location = new Point(0, 25);
      openglControl1.Margin = new Padding(4, 3, 4, 3);
      openglControl1.Name = "openglControl1";
      openglControl1.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL3_3;
      openglControl1.RenderContextType = SharpGL.RenderContextType.NativeWindow;
      openglControl1.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
      openglControl1.Size = new Size(299, 245);
      openglControl1.TabIndex = 1;
      openglControl1.OpenGLInitialized += openglControl1_OpenGLInitialized;
      openglControl1.OpenGLDraw += openglControl1_OpenGLDraw;
      openglControl1.Resize += openglControl1_Resize;
      // 
      // RadioButtonsPanel
      // 
      RadioButtonsPanel.BackColor = Color.Transparent;
      RadioButtonsPanel.Controls.Add(SatelliteRadioBtn);
      RadioButtonsPanel.Controls.Add(OrbitRadioBtn);
      RadioButtonsPanel.Dock = DockStyle.Top;
      RadioButtonsPanel.Location = new Point(0, 0);
      RadioButtonsPanel.MaximumSize = new Size(0, 60);
      RadioButtonsPanel.Name = "RadioButtonsPanel";
      RadioButtonsPanel.Size = new Size(299, 25);
      RadioButtonsPanel.TabIndex = 3;
      // 
      // SatelliteRadioBtn
      // 
      SatelliteRadioBtn.AutoSize = true;
      SatelliteRadioBtn.Checked = true;
      SatelliteRadioBtn.Location = new Point(3, 3);
      SatelliteRadioBtn.Name = "SatelliteRadioBtn";
      SatelliteRadioBtn.Size = new Size(66, 19);
      SatelliteRadioBtn.TabIndex = 0;
      SatelliteRadioBtn.TabStop = true;
      SatelliteRadioBtn.Text = "Satellite";
      SatelliteRadioBtn.UseVisualStyleBackColor = false;
      SatelliteRadioBtn.CheckedChanged += SatelliteRadioBtn_CheckedChanged;
      // 
      // OrbitRadioBtn
      // 
      OrbitRadioBtn.AutoSize = true;
      OrbitRadioBtn.Enabled = false;
      OrbitRadioBtn.Location = new Point(75, 3);
      OrbitRadioBtn.Name = "OrbitRadioBtn";
      OrbitRadioBtn.Size = new Size(99, 19);
      OrbitRadioBtn.TabIndex = 1;
      OrbitRadioBtn.Text = "Selected Orbit";
      OrbitRadioBtn.UseVisualStyleBackColor = false;
      OrbitRadioBtn.CheckedChanged += SatelliteRadioBtn_CheckedChanged;
      // 
      // EarthViewPanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(299, 270);
      Controls.Add(openglControl1);
      Controls.Add(RadioButtonsPanel);
      Name = "EarthViewPanel";
      Text = "Earth View";
      FormClosing += EarthViewPanel_FormClosing;
      ((System.ComponentModel.ISupportInitialize)openglControl1).EndInit();
      RadioButtonsPanel.ResumeLayout(false);
      RadioButtonsPanel.PerformLayout();
      ResumeLayout(false);
    }

    #endregion

    private SharpGL.OpenGLControl openglControl1;
    private FlowLayoutPanel RadioButtonsPanel;
    private RadioButton SatelliteRadioBtn;
    private RadioButton OrbitRadioBtn;
  }
}