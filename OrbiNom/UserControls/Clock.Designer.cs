namespace VE3NEA.Clock
{
  partial class Clock
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
      timeLabel = new Label();
      dateLabel = new Label();
      utcLabel = new Label();
      SuspendLayout();
      // 
      // timeLabel
      // 
      timeLabel.AutoSize = true;
      timeLabel.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
      timeLabel.ForeColor = Color.Aqua;
      timeLabel.Location = new Point(0, 3);
      timeLabel.Margin = new Padding(2, 0, 2, 0);
      timeLabel.Name = "timeLabel";
      timeLabel.RightToLeft = RightToLeft.No;
      timeLabel.Size = new Size(96, 26);
      timeLabel.TabIndex = 0;
      timeLabel.Text = "00:00:00";
      timeLabel.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // dateLabel
      // 
      dateLabel.AutoSize = true;
      dateLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
      dateLabel.ForeColor = Color.Aqua;
      dateLabel.Location = new Point(4, 30);
      dateLabel.Margin = new Padding(2, 0, 2, 0);
      dateLabel.Name = "dateLabel";
      dateLabel.RightToLeft = RightToLeft.No;
      dateLabel.Size = new Size(42, 13);
      dateLabel.TabIndex = 1;
      dateLabel.Text = "Dec 31";
      dateLabel.TextAlign = ContentAlignment.TopRight;
      // 
      // utcLabel
      // 
      utcLabel.AutoSize = true;
      utcLabel.BackColor = Color.Aqua;
      utcLabel.Cursor = Cursors.Hand;
      utcLabel.Font = new Font("Microsoft Sans Serif", 6F, FontStyle.Bold, GraphicsUnit.Point, 0);
      utcLabel.Location = new Point(62, 34);
      utcLabel.Margin = new Padding(2, 0, 2, 0);
      utcLabel.Name = "utcLabel";
      utcLabel.RightToLeft = RightToLeft.No;
      utcLabel.Size = new Size(25, 9);
      utcLabel.TabIndex = 2;
      utcLabel.Text = "UTC";
      utcLabel.Click += utcLabel_Click;
      // 
      // Clock
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.DarkBlue;
      Controls.Add(utcLabel);
      Controls.Add(dateLabel);
      Controls.Add(timeLabel);
      Margin = new Padding(2, 3, 2, 3);
      Name = "Clock";
      Size = new Size(96, 50);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label timeLabel;
    private System.Windows.Forms.Label dateLabel;
    private System.Windows.Forms.Label utcLabel;
  }
}
