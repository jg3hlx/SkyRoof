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
      SplitContainer = new SplitContainer();
      AudioWaterfall = new AudioWaterfallWidget();
      MessageListWidget = new Ft4MessageListWidget();
      panel1 = new Panel();
      label1 = new Label();
      flowLayoutPanel1 = new FlowLayoutPanel();
      button1 = new Button();
      button2 = new Button();
      button3 = new Button();
      button4 = new Button();
      button5 = new Button();
      button6 = new Button();
      groupBox2 = new GroupBox();
      TxToRxBtn = new Button();
      RxToTxBtn = new Button();
      label4 = new Label();
      label5 = new Label();
      RxSpinner = new NumericUpDown();
      label3 = new Label();
      label2 = new Label();
      TxSpinner = new NumericUpDown();
      OddEvenGroupBox = new GroupBox();
      EvenRadioBtn = new RadioButton();
      OddRadioBtn = new RadioButton();
      ft4TimeBar1 = new SkyRoof.Widgets.Ft4TimeBar();
      HaltTxBtn = new Button();
      EnableTxBtn = new Button();
      TuneBtn = new Button();
      ((System.ComponentModel.ISupportInitialize)SplitContainer).BeginInit();
      SplitContainer.Panel1.SuspendLayout();
      SplitContainer.Panel2.SuspendLayout();
      SplitContainer.SuspendLayout();
      panel1.SuspendLayout();
      flowLayoutPanel1.SuspendLayout();
      groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)RxSpinner).BeginInit();
      ((System.ComponentModel.ISupportInitialize)TxSpinner).BeginInit();
      OddEvenGroupBox.SuspendLayout();
      SuspendLayout();
      // 
      // SplitContainer
      // 
      SplitContainer.Dock = DockStyle.Fill;
      SplitContainer.FixedPanel = FixedPanel.Panel2;
      SplitContainer.Location = new Point(0, 0);
      SplitContainer.Name = "SplitContainer";
      SplitContainer.Orientation = Orientation.Horizontal;
      // 
      // SplitContainer.Panel1
      // 
      SplitContainer.Panel1.Controls.Add(AudioWaterfall);
      // 
      // SplitContainer.Panel2
      // 
      SplitContainer.Panel2.Controls.Add(MessageListWidget);
      SplitContainer.Panel2.Controls.Add(panel1);
      SplitContainer.Size = new Size(800, 533);
      SplitContainer.SplitterDistance = 266;
      SplitContainer.TabIndex = 2;
      // 
      // AudioWaterfall
      // 
      AudioWaterfall.Dock = DockStyle.Fill;
      AudioWaterfall.Font = new Font("Courier New", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
      AudioWaterfall.Location = new Point(0, 0);
      AudioWaterfall.Margin = new Padding(4, 3, 4, 3);
      AudioWaterfall.Name = "AudioWaterfall";
      AudioWaterfall.Size = new Size(800, 266);
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
      MessageListWidget.Size = new Size(527, 263);
      MessageListWidget.TabIndex = 3;
      // 
      // panel1
      // 
      panel1.Controls.Add(label1);
      panel1.Controls.Add(flowLayoutPanel1);
      panel1.Controls.Add(groupBox2);
      panel1.Controls.Add(OddEvenGroupBox);
      panel1.Controls.Add(ft4TimeBar1);
      panel1.Controls.Add(HaltTxBtn);
      panel1.Controls.Add(EnableTxBtn);
      panel1.Controls.Add(TuneBtn);
      panel1.Dock = DockStyle.Right;
      panel1.Location = new Point(527, 0);
      panel1.Name = "panel1";
      panel1.Size = new Size(273, 263);
      panel1.TabIndex = 4;
      // 
      // label1
      // 
      label1.BackColor = SystemColors.Window;
      label1.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      label1.Location = new Point(14, 165);
      label1.Name = "label1";
      label1.Size = new Size(246, 21);
      label1.TabIndex = 32;
      label1.Text = "CQ VE3NEA FN03";
      label1.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // flowLayoutPanel1
      // 
      flowLayoutPanel1.Controls.Add(button1);
      flowLayoutPanel1.Controls.Add(button2);
      flowLayoutPanel1.Controls.Add(button3);
      flowLayoutPanel1.Controls.Add(button4);
      flowLayoutPanel1.Controls.Add(button5);
      flowLayoutPanel1.Controls.Add(button6);
      flowLayoutPanel1.Location = new Point(14, 196);
      flowLayoutPanel1.Name = "flowLayoutPanel1";
      flowLayoutPanel1.Size = new Size(246, 59);
      flowLayoutPanel1.TabIndex = 31;
      // 
      // button1
      // 
      button1.Location = new Point(3, 3);
      button1.Name = "button1";
      button1.Size = new Size(76, 23);
      button1.TabIndex = 0;
      button1.Text = "CQ";
      button1.UseVisualStyleBackColor = true;
      // 
      // button2
      // 
      button2.Location = new Point(85, 3);
      button2.Name = "button2";
      button2.Size = new Size(76, 23);
      button2.TabIndex = 1;
      button2.Text = "dB";
      button2.UseVisualStyleBackColor = true;
      // 
      // button3
      // 
      button3.Location = new Point(167, 3);
      button3.Name = "button3";
      button3.Size = new Size(76, 23);
      button3.TabIndex = 2;
      button3.Text = "RR73";
      button3.UseVisualStyleBackColor = true;
      // 
      // button4
      // 
      button4.Location = new Point(3, 32);
      button4.Name = "button4";
      button4.Size = new Size(76, 23);
      button4.TabIndex = 3;
      button4.Text = "DE";
      button4.UseVisualStyleBackColor = true;
      // 
      // button5
      // 
      button5.Location = new Point(85, 32);
      button5.Name = "button5";
      button5.Size = new Size(76, 23);
      button5.TabIndex = 4;
      button5.Text = "RR-dB";
      button5.UseVisualStyleBackColor = true;
      // 
      // button6
      // 
      button6.Location = new Point(167, 32);
      button6.Name = "button6";
      button6.Size = new Size(76, 23);
      button6.TabIndex = 5;
      button6.Text = "73";
      button6.UseVisualStyleBackColor = true;
      // 
      // groupBox2
      // 
      groupBox2.Controls.Add(TxToRxBtn);
      groupBox2.Controls.Add(RxToTxBtn);
      groupBox2.Controls.Add(label4);
      groupBox2.Controls.Add(label5);
      groupBox2.Controls.Add(RxSpinner);
      groupBox2.Controls.Add(label3);
      groupBox2.Controls.Add(label2);
      groupBox2.Controls.Add(TxSpinner);
      groupBox2.Location = new Point(96, 77);
      groupBox2.Margin = new Padding(4, 3, 4, 3);
      groupBox2.Name = "groupBox2";
      groupBox2.Padding = new Padding(4, 3, 4, 3);
      groupBox2.Size = new Size(164, 82);
      groupBox2.TabIndex = 30;
      groupBox2.TabStop = false;
      // 
      // TxToRxBtn
      // 
      TxToRxBtn.Font = new Font("Wingdings 3", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 2);
      TxToRxBtn.ForeColor = Color.LightGreen;
      TxToRxBtn.Location = new Point(122, 14);
      TxToRxBtn.Margin = new Padding(4, 3, 4, 3);
      TxToRxBtn.Name = "TxToRxBtn";
      TxToRxBtn.Size = new Size(36, 27);
      TxToRxBtn.TabIndex = 25;
      TxToRxBtn.TabStop = false;
      TxToRxBtn.Text = "q";
      TxToRxBtn.UseVisualStyleBackColor = true;
      TxToRxBtn.Click += TxToRxBtn_Click;
      // 
      // RxToTxBtn
      // 
      RxToTxBtn.Font = new Font("Wingdings 3", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 2);
      RxToTxBtn.ForeColor = Color.LightCoral;
      RxToTxBtn.Location = new Point(122, 50);
      RxToTxBtn.Margin = new Padding(4, 3, 4, 3);
      RxToTxBtn.Name = "RxToTxBtn";
      RxToTxBtn.Size = new Size(36, 27);
      RxToTxBtn.TabIndex = 24;
      RxToTxBtn.TabStop = false;
      RxToTxBtn.Text = "p";
      RxToTxBtn.UseVisualStyleBackColor = true;
      RxToTxBtn.Click += RxToTxBtn_Click;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(96, 55);
      label4.Margin = new Padding(4, 0, 4, 0);
      label4.Name = "label4";
      label4.Size = new Size(21, 15);
      label4.TabIndex = 23;
      label4.Text = "Hz";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new Point(5, 55);
      label5.Margin = new Padding(4, 0, 4, 0);
      label5.Name = "label5";
      label5.Size = new Size(21, 15);
      label5.TabIndex = 22;
      label5.Text = "RX";
      // 
      // RxSpinner
      // 
      RxSpinner.Increment = new decimal(new int[] { 10, 0, 0, 0 });
      RxSpinner.Location = new Point(32, 53);
      RxSpinner.Margin = new Padding(4, 3, 4, 3);
      RxSpinner.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
      RxSpinner.Name = "RxSpinner";
      RxSpinner.Size = new Size(59, 23);
      RxSpinner.TabIndex = 3;
      RxSpinner.Value = new decimal(new int[] { 1500, 0, 0, 0 });
      RxSpinner.ValueChanged += RxSpinner_ValueChanged;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(96, 18);
      label3.Margin = new Padding(4, 0, 4, 0);
      label3.Name = "label3";
      label3.Size = new Size(21, 15);
      label3.TabIndex = 20;
      label3.Text = "Hz";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(5, 18);
      label2.Margin = new Padding(4, 0, 4, 0);
      label2.Name = "label2";
      label2.Size = new Size(20, 15);
      label2.TabIndex = 19;
      label2.Text = "TX";
      // 
      // TxSpinner
      // 
      TxSpinner.Increment = new decimal(new int[] { 10, 0, 0, 0 });
      TxSpinner.Location = new Point(32, 16);
      TxSpinner.Margin = new Padding(4, 3, 4, 3);
      TxSpinner.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
      TxSpinner.Name = "TxSpinner";
      TxSpinner.Size = new Size(59, 23);
      TxSpinner.TabIndex = 2;
      TxSpinner.Value = new decimal(new int[] { 1500, 0, 0, 0 });
      TxSpinner.ValueChanged += TxSpinner_ValueChanged;
      // 
      // OddEvenGroupBox
      // 
      OddEvenGroupBox.Controls.Add(EvenRadioBtn);
      OddEvenGroupBox.Controls.Add(OddRadioBtn);
      OddEvenGroupBox.Location = new Point(14, 77);
      OddEvenGroupBox.Margin = new Padding(4, 3, 4, 3);
      OddEvenGroupBox.Name = "OddEvenGroupBox";
      OddEvenGroupBox.Padding = new Padding(4, 3, 4, 3);
      OddEvenGroupBox.Size = new Size(75, 82);
      OddEvenGroupBox.TabIndex = 29;
      OddEvenGroupBox.TabStop = false;
      OddEvenGroupBox.Text = "TX Even";
      // 
      // EvenRadioBtn
      // 
      EvenRadioBtn.AutoSize = true;
      EvenRadioBtn.Checked = true;
      EvenRadioBtn.Font = new Font("Webdings", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 2);
      EvenRadioBtn.ForeColor = Color.Teal;
      EvenRadioBtn.Location = new Point(13, 48);
      EvenRadioBtn.Margin = new Padding(4, 3, 4, 3);
      EvenRadioBtn.Name = "EvenRadioBtn";
      EvenRadioBtn.Size = new Size(42, 24);
      EvenRadioBtn.TabIndex = 1;
      EvenRadioBtn.TabStop = true;
      EvenRadioBtn.Text = "n";
      EvenRadioBtn.UseVisualStyleBackColor = true;
      // 
      // OddRadioBtn
      // 
      OddRadioBtn.AutoSize = true;
      OddRadioBtn.Font = new Font("Webdings", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 2);
      OddRadioBtn.ForeColor = Color.Olive;
      OddRadioBtn.Location = new Point(13, 23);
      OddRadioBtn.Margin = new Padding(4, 3, 4, 3);
      OddRadioBtn.Name = "OddRadioBtn";
      OddRadioBtn.Size = new Size(42, 24);
      OddRadioBtn.TabIndex = 0;
      OddRadioBtn.Text = "n";
      OddRadioBtn.UseVisualStyleBackColor = true;
      OddRadioBtn.CheckedChanged += OddRadioBtn_CheckedChanged;
      // 
      // ft4TimeBar1
      // 
      ft4TimeBar1.Location = new Point(14, 11);
      ft4TimeBar1.Name = "ft4TimeBar1";
      ft4TimeBar1.Size = new Size(246, 15);
      ft4TimeBar1.TabIndex = 4;
      // 
      // HaltTxBtn
      // 
      HaltTxBtn.Location = new Point(114, 42);
      HaltTxBtn.Name = "HaltTxBtn";
      HaltTxBtn.Size = new Size(88, 28);
      HaltTxBtn.TabIndex = 2;
      HaltTxBtn.Text = "Halt TX";
      HaltTxBtn.UseVisualStyleBackColor = true;
      HaltTxBtn.MouseDown += HaltTxBtn_MouseDown;
      // 
      // EnableTxBtn
      // 
      EnableTxBtn.Location = new Point(14, 43);
      EnableTxBtn.Name = "EnableTxBtn";
      EnableTxBtn.Size = new Size(88, 28);
      EnableTxBtn.TabIndex = 1;
      EnableTxBtn.Text = "Enable TX";
      EnableTxBtn.UseVisualStyleBackColor = true;
      EnableTxBtn.MouseDown += EnableTxBtn_MouseDown;
      // 
      // TuneBtn
      // 
      TuneBtn.Location = new Point(214, 42);
      TuneBtn.Name = "TuneBtn";
      TuneBtn.Size = new Size(42, 28);
      TuneBtn.TabIndex = 0;
      TuneBtn.Text = "Tune";
      TuneBtn.UseVisualStyleBackColor = true;
      TuneBtn.MouseDown += TuneBtn_MouseDown;
      // 
      // Ft4ConsolePanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 533);
      Controls.Add(SplitContainer);
      Name = "Ft4ConsolePanel";
      Text = "FT4 Console";
      FormClosing += Ft4ConsolePanel_FormClosing;
      Shown += Ft4ConsolePanel_Shown;
      SplitContainer.Panel1.ResumeLayout(false);
      SplitContainer.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)SplitContainer).EndInit();
      SplitContainer.ResumeLayout(false);
      panel1.ResumeLayout(false);
      flowLayoutPanel1.ResumeLayout(false);
      groupBox2.ResumeLayout(false);
      groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)RxSpinner).EndInit();
      ((System.ComponentModel.ISupportInitialize)TxSpinner).EndInit();
      OddEvenGroupBox.ResumeLayout(false);
      OddEvenGroupBox.PerformLayout();
      ResumeLayout(false);
    }

    #endregion
    private AudioWaterfallWidget AudioWaterfall;
    private Ft4MessageListWidget MessageListWidget;
    private Panel panel1;
    private Button EnableTxBtn;
    private Button TuneBtn;
    private Button HaltTxBtn;
    private Widgets.Ft4TimeBar ft4TimeBar1;
    private GroupBox groupBox2;
    private Button TxToRxBtn;
    private Button RxToTxBtn;
    private Label label4;
    private Label label5;
    private NumericUpDown RxSpinner;
    private Label label3;
    private Label label2;
    private NumericUpDown TxSpinner;
    private GroupBox OddEvenGroupBox;
    private RadioButton EvenRadioBtn;
    private RadioButton OddRadioBtn;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button button1;
    private Button button2;
    private Button button3;
    private Button button4;
    private Button button5;
    private Button button6;
    private Label label1;
    public SplitContainer SplitContainer;
  }
}