namespace SkyRoof
{
  partial class Ft4ConsolePanel
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
      splitContainer1 = new SplitContainer();
      AudioWaterfall = new AudioWaterfallWidget();
      MessageListWidget = new Ft4MessageListWidget();
      panel1 = new Panel();
      ft4TimeBar1 = new SkyRoof.Widgets.Ft4TimeBar();
      HaltTxBtn = new Button();
      EnableTxBtn = new Button();
      TuneBtn = new Button();
      ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
      splitContainer1.Panel1.SuspendLayout();
      splitContainer1.Panel2.SuspendLayout();
      splitContainer1.SuspendLayout();
      panel1.SuspendLayout();
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
      splitContainer1.Panel1.Controls.Add(AudioWaterfall);
      // 
      // splitContainer1.Panel2
      // 
      splitContainer1.Panel2.Controls.Add(MessageListWidget);
      splitContainer1.Panel2.Controls.Add(panel1);
      splitContainer1.Size = new Size(800, 450);
      splitContainer1.SplitterDistance = 241;
      splitContainer1.TabIndex = 2;
      // 
      // AudioWaterfall
      // 
      AudioWaterfall.Dock = DockStyle.Fill;
      AudioWaterfall.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
      AudioWaterfall.Location = new Point(0, 0);
      AudioWaterfall.Margin = new Padding(4, 3, 4, 3);
      AudioWaterfall.Name = "AudioWaterfall";
      AudioWaterfall.Size = new Size(800, 241);
      AudioWaterfall.TabIndex = 2;
      AudioWaterfall.MouseMove += AudioWaterfall_MouseMove;
      // 
      // MessageListWidget
      // 
      MessageListWidget.Dock = DockStyle.Fill;
      MessageListWidget.Location = new Point(0, 0);
      MessageListWidget.Margin = new Padding(4, 3, 4, 3);
      MessageListWidget.Name = "MessageListWidget";
      MessageListWidget.Padding = new Padding(1);
      MessageListWidget.Size = new Size(527, 205);
      MessageListWidget.TabIndex = 3;
      // 
      // panel1
      // 
      panel1.Controls.Add(ft4TimeBar1);
      panel1.Controls.Add(HaltTxBtn);
      panel1.Controls.Add(EnableTxBtn);
      panel1.Controls.Add(TuneBtn);
      panel1.Dock = DockStyle.Right;
      panel1.Location = new Point(527, 0);
      panel1.Name = "panel1";
      panel1.Size = new Size(273, 205);
      panel1.TabIndex = 4;
      // 
      // ft4TimeBar1
      // 
      ft4TimeBar1.Location = new Point(18, 11);
      ft4TimeBar1.Name = "ft4TimeBar1";
      ft4TimeBar1.Size = new Size(236, 15);
      ft4TimeBar1.TabIndex = 4;
      // 
      // HaltTxBtn
      // 
      HaltTxBtn.Location = new Point(99, 42);
      HaltTxBtn.Name = "HaltTxBtn";
      HaltTxBtn.Size = new Size(75, 23);
      HaltTxBtn.TabIndex = 2;
      HaltTxBtn.Text = "Halt TX";
      HaltTxBtn.UseVisualStyleBackColor = true;
      HaltTxBtn.Click += HaltTxBtn_Click;
      // 
      // EnableTxBtn
      // 
      EnableTxBtn.Location = new Point(18, 42);
      EnableTxBtn.Name = "EnableTxBtn";
      EnableTxBtn.Size = new Size(75, 23);
      EnableTxBtn.TabIndex = 1;
      EnableTxBtn.Text = "Enable TX";
      EnableTxBtn.UseVisualStyleBackColor = true;
      EnableTxBtn.Click += EnableTxBtn_Click;
      // 
      // TuneBtn
      // 
      TuneBtn.Location = new Point(180, 42);
      TuneBtn.Name = "TuneBtn";
      TuneBtn.Size = new Size(75, 23);
      TuneBtn.TabIndex = 0;
      TuneBtn.Text = "Tune";
      TuneBtn.UseVisualStyleBackColor = true;
      TuneBtn.Click += TuneBtn_Click;
      // 
      // Ft4ConsolePanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(splitContainer1);
      Name = "Ft4ConsolePanel";
      Text = "FT4 Console";
      FormClosing += Ft4ConsolePanel_FormClosing;
      splitContainer1.Panel1.ResumeLayout(false);
      splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
      splitContainer1.ResumeLayout(false);
      panel1.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer1;
    private AudioWaterfallWidget AudioWaterfall;
    private Ft4MessageListWidget MessageListWidget;
    private Panel panel1;
    private Button EnableTxBtn;
    private Button TuneBtn;
    private Button HaltTxBtn;
    private Widgets.Ft4TimeBar ft4TimeBar1;
  }
}