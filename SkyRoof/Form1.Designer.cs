namespace Sixer
{
  partial class Form1
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
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.TxToRxBtn = new System.Windows.Forms.Button();
      this.RxToTxBtn = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.RxSpinner = new System.Windows.Forms.NumericUpDown();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.TxSpinner = new System.Windows.Forms.NumericUpDown();
      this.OddEvenGroupBox = new System.Windows.Forms.GroupBox();
      this.EvenRadioBtn = new System.Windows.Forms.RadioButton();
      this.OddRadioBtn = new System.Windows.Forms.RadioButton();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.RxSpinner)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.TxSpinner)).BeginInit();
      this.OddEvenGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.TxToRxBtn);
      this.groupBox2.Controls.Add(this.RxToTxBtn);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.label5);
      this.groupBox2.Controls.Add(this.RxSpinner);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this.TxSpinner);
      this.groupBox2.Location = new System.Drawing.Point(365, 187);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(141, 76);
      this.groupBox2.TabIndex = 28;
      this.groupBox2.TabStop = false;
      // 
      // TxToRxBtn
      // 
      this.TxToRxBtn.Font = new System.Drawing.Font("Wingdings 3", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
      this.TxToRxBtn.ForeColor = System.Drawing.Color.LightGreen;
      this.TxToRxBtn.Location = new System.Drawing.Point(106, 12);
      this.TxToRxBtn.Name = "TxToRxBtn";
      this.TxToRxBtn.Size = new System.Drawing.Size(31, 23);
      this.TxToRxBtn.TabIndex = 25;
      this.TxToRxBtn.TabStop = false;
      this.TxToRxBtn.Text = "q";
      this.TxToRxBtn.UseVisualStyleBackColor = true;
      // 
      // RxToTxBtn
      // 
      this.RxToTxBtn.Font = new System.Drawing.Font("Wingdings 3", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
      this.RxToTxBtn.ForeColor = System.Drawing.Color.LightCoral;
      this.RxToTxBtn.Location = new System.Drawing.Point(106, 43);
      this.RxToTxBtn.Name = "RxToTxBtn";
      this.RxToTxBtn.Size = new System.Drawing.Size(31, 23);
      this.RxToTxBtn.TabIndex = 24;
      this.RxToTxBtn.TabStop = false;
      this.RxToTxBtn.Text = "p";
      this.RxToTxBtn.UseVisualStyleBackColor = true;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(83, 48);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(20, 13);
      this.label4.TabIndex = 23;
      this.label4.Text = "Hz";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(5, 48);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(22, 13);
      this.label5.TabIndex = 22;
      this.label5.Text = "RX";
      // 
      // RxSpinner
      // 
      this.RxSpinner.Location = new System.Drawing.Point(29, 46);
      this.RxSpinner.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
      this.RxSpinner.Name = "RxSpinner";
      this.RxSpinner.Size = new System.Drawing.Size(51, 20);
      this.RxSpinner.TabIndex = 3;
      this.RxSpinner.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(83, 16);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(20, 13);
      this.label3.TabIndex = 20;
      this.label3.Text = "Hz";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(5, 16);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(21, 13);
      this.label2.TabIndex = 19;
      this.label2.Text = "TX";
      // 
      // TxSpinner
      // 
      this.TxSpinner.Location = new System.Drawing.Point(29, 14);
      this.TxSpinner.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
      this.TxSpinner.Name = "TxSpinner";
      this.TxSpinner.Size = new System.Drawing.Size(51, 20);
      this.TxSpinner.TabIndex = 2;
      this.TxSpinner.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
      // 
      // OddEvenGroupBox
      // 
      this.OddEvenGroupBox.Controls.Add(this.EvenRadioBtn);
      this.OddEvenGroupBox.Controls.Add(this.OddRadioBtn);
      this.OddEvenGroupBox.Location = new System.Drawing.Point(295, 187);
      this.OddEvenGroupBox.Name = "OddEvenGroupBox";
      this.OddEvenGroupBox.Size = new System.Drawing.Size(64, 76);
      this.OddEvenGroupBox.TabIndex = 27;
      this.OddEvenGroupBox.TabStop = false;
      this.OddEvenGroupBox.Text = "TX Even";
      // 
      // EvenRadioBtn
      // 
      this.EvenRadioBtn.AutoSize = true;
      this.EvenRadioBtn.Checked = true;
      this.EvenRadioBtn.Font = new System.Drawing.Font("Webdings", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
      this.EvenRadioBtn.ForeColor = System.Drawing.Color.Teal;
      this.EvenRadioBtn.Location = new System.Drawing.Point(10, 42);
      this.EvenRadioBtn.Name = "EvenRadioBtn";
      this.EvenRadioBtn.Size = new System.Drawing.Size(42, 24);
      this.EvenRadioBtn.TabIndex = 1;
      this.EvenRadioBtn.TabStop = true;
      this.EvenRadioBtn.Text = "n";
      this.EvenRadioBtn.UseVisualStyleBackColor = true;
      // 
      // OddRadioBtn
      // 
      this.OddRadioBtn.AutoSize = true;
      this.OddRadioBtn.Font = new System.Drawing.Font("Webdings", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
      this.OddRadioBtn.ForeColor = System.Drawing.Color.Olive;
      this.OddRadioBtn.Location = new System.Drawing.Point(10, 20);
      this.OddRadioBtn.Name = "OddRadioBtn";
      this.OddRadioBtn.Size = new System.Drawing.Size(42, 24);
      this.OddRadioBtn.TabIndex = 0;
      this.OddRadioBtn.Text = "n";
      this.OddRadioBtn.UseVisualStyleBackColor = true;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.OddEvenGroupBox);
      this.Name = "Form1";
      this.Text = "Form1";
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.RxSpinner)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.TxSpinner)).EndInit();
      this.OddEvenGroupBox.ResumeLayout(false);
      this.OddEvenGroupBox.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button TxToRxBtn;
    private System.Windows.Forms.Button RxToTxBtn;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.NumericUpDown RxSpinner;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.NumericUpDown TxSpinner;
    private System.Windows.Forms.GroupBox OddEvenGroupBox;
    private System.Windows.Forms.RadioButton EvenRadioBtn;
    private System.Windows.Forms.RadioButton OddRadioBtn;
  }
}