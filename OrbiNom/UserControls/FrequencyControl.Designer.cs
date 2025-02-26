namespace OrbiNom.UserControls
{
  partial class FrequencyControl
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
      label1 = new Label();
      label3 = new Label();
      checkBox1 = new CheckBox();
      checkBox2 = new CheckBox();
      numericUpDown1 = new NumericUpDown();
      numericUpDown2 = new NumericUpDown();
      label2 = new Label();
      ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
      ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
      SuspendLayout();
      // 
      // label1
      // 
      label1.Location = new Point(21, 5);
      label1.Name = "label1";
      label1.Size = new Size(156, 15);
      label1.TabIndex = 0;
      label1.Text = "SONATE-2";
      // 
      // label3
      // 
      label3.BackColor = Color.Black;
      label3.Font = new Font("Microsoft Sans Serif", 16F);
      label3.ForeColor = Color.Aqua;
      label3.Location = new Point(7, 23);
      label3.Name = "label3";
      label3.Size = new Size(170, 34);
      label3.TabIndex = 2;
      label3.Text = "435,000,000";
      label3.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // checkBox1
      // 
      checkBox1.AutoSize = true;
      checkBox1.Location = new Point(190, 9);
      checkBox1.Name = "checkBox1";
      checkBox1.Size = new Size(68, 19);
      checkBox1.TabIndex = 3;
      checkBox1.Text = "Doppler";
      checkBox1.UseVisualStyleBackColor = true;
      // 
      // checkBox2
      // 
      checkBox2.AutoSize = true;
      checkBox2.Location = new Point(190, 38);
      checkBox2.Name = "checkBox2";
      checkBox2.Size = new Size(66, 19);
      checkBox2.TabIndex = 4;
      checkBox2.Text = "Manual";
      checkBox2.UseVisualStyleBackColor = true;
      // 
      // numericUpDown1
      // 
      numericUpDown1.DecimalPlaces = 3;
      numericUpDown1.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
      numericUpDown1.Location = new Point(264, 8);
      numericUpDown1.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      numericUpDown1.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      numericUpDown1.Name = "numericUpDown1";
      numericUpDown1.Size = new Size(68, 23);
      numericUpDown1.TabIndex = 5;
      numericUpDown1.Value = new decimal(new int[] { 20, 0, 0, 0 });
      // 
      // numericUpDown2
      // 
      numericUpDown2.DecimalPlaces = 3;
      numericUpDown2.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
      numericUpDown2.Location = new Point(264, 37);
      numericUpDown2.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
      numericUpDown2.Minimum = new decimal(new int[] { 25, 0, 0, int.MinValue });
      numericUpDown2.Name = "numericUpDown2";
      numericUpDown2.Size = new Size(68, 23);
      numericUpDown2.TabIndex = 6;
      numericUpDown2.Value = new decimal(new int[] { 20, 0, 0, int.MinValue });
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Font = new Font("Source Code Pro", 14F, FontStyle.Bold);
      label2.Location = new Point(3, -2);
      label2.Name = "label2";
      label2.Size = new Size(21, 24);
      label2.TabIndex = 7;
      label2.Text = "↓";
      // 
      // FrequencyControl1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BorderStyle = BorderStyle.FixedSingle;
      Controls.Add(label2);
      Controls.Add(numericUpDown2);
      Controls.Add(numericUpDown1);
      Controls.Add(checkBox2);
      Controls.Add(checkBox1);
      Controls.Add(label3);
      Controls.Add(label1);
      Name = "FrequencyControl1";
      Size = new Size(342, 66);
      ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
      ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Label label1;
    private Label label3;
    private CheckBox checkBox1;
    private CheckBox checkBox2;
    private NumericUpDown numericUpDown1;
    private NumericUpDown numericUpDown2;
    private Label label2;
  }
}
