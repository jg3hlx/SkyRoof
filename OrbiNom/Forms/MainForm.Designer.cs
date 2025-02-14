namespace OrbiNom
{
  partial class MainForm
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      Toolbar = new Panel();
      SatelliteSelector = new SatelliteSelector();
      ClockPanel = new Panel();
      Clock = new VE3NEA.Clock.Clock();
      DockHost = new WeifenLuo.WinFormsUI.Docking.DockPanel();
      vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
      menuStrip1 = new MenuStrip();
      fileToolStripMenuItem = new ToolStripMenuItem();
      ExitMNU = new ToolStripMenuItem();
      GroupViewPanelMNU = new ToolStripMenuItem();
      GroupViewMNU = new ToolStripMenuItem();
      SatelliteDetailsMNU = new ToolStripMenuItem();
      TransmittersMNU = new ToolStripMenuItem();
      SatellitePassesMNU = new ToolStripMenuItem();
      WaterfallMNU = new ToolStripMenuItem();
      TimelineMNU = new ToolStripMenuItem();
      SkyViewMNU = new ToolStripMenuItem();
      EarthViewMNU = new ToolStripMenuItem();
      toolsToolStripMenuItem = new ToolStripMenuItem();
      SatelliteGroupsMNU = new ToolStripMenuItem();
      SdrDevicesMNU = new ToolStripMenuItem();
      SettingsMNU = new ToolStripMenuItem();
      toolStripMenuItem1 = new ToolStripSeparator();
      DownloadSatDataMNU = new ToolStripMenuItem();
      DownloadTleMNU = new ToolStripMenuItem();
      helpToolStripMenuItem = new ToolStripMenuItem();
      OnlineHelpMNU = new ToolStripMenuItem();
      emailTheAuthorToolStripMenuItem = new ToolStripMenuItem();
      DataFolderMNU = new ToolStripMenuItem();
      toolStripMenuItem2 = new ToolStripSeparator();
      AboutMNU = new ToolStripMenuItem();
      timer = new System.Windows.Forms.Timer(components);
      StatusStrip = new StatusStrip();
      toolStripStatusLabel2 = new ToolStripStatusLabel();
      SdrLedLabel = new ToolStripStatusLabel();
      SdrStatusLabel = new ToolStripStatusLabel();
      SoundcardLedLabel = new ToolStripStatusLabel();
      SoundcardStatusLabel = new ToolStripStatusLabel();
      VacLedLabel = new ToolStripStatusLabel();
      VacStatusLabel = new ToolStripStatusLabel();
      OmniRigLedLabel = new ToolStripStatusLabel();
      OmniRigStatusLabel = new ToolStripStatusLabel();
      SatDataLedLabel = new ToolStripStatusLabel();
      SatDataStatusLabel = new ToolStripStatusLabel();
      IqOutputLedLabel = new ToolStripStatusLabel();
      IqOutputStatusLabel = new ToolStripStatusLabel();
      NoiseFloorLabel = new ToolStripStatusLabel();
      CpuLoadlabel = new ToolStripStatusLabel();
      toolTip1 = new ToolTip(components);
      Toolbar.SuspendLayout();
      ClockPanel.SuspendLayout();
      menuStrip1.SuspendLayout();
      StatusStrip.SuspendLayout();
      SuspendLayout();
      // 
      // Toolbar
      // 
      Toolbar.BorderStyle = BorderStyle.FixedSingle;
      Toolbar.Controls.Add(SatelliteSelector);
      Toolbar.Controls.Add(ClockPanel);
      Toolbar.Dock = DockStyle.Top;
      Toolbar.Location = new Point(0, 24);
      Toolbar.Name = "Toolbar";
      Toolbar.Size = new Size(1200, 40);
      Toolbar.TabIndex = 0;
      // 
      // SatelliteSelector
      // 
      SatelliteSelector.Dock = DockStyle.Left;
      SatelliteSelector.Location = new Point(0, 0);
      SatelliteSelector.Name = "SatelliteSelector";
      SatelliteSelector.Size = new Size(619, 38);
      SatelliteSelector.TabIndex = 2;
      SatelliteSelector.SelectedGroupChanged += SatelliteSelector_SelectedGroupChanged;
      SatelliteSelector.SelectedSatelliteChanged += SatelliteSelector_SelectedSatelliteChanged;
      SatelliteSelector.ClickedSatelliteChanged += SatelliteSelector_ClickedSatelliteChanged;
      SatelliteSelector.SelectedPassChanged += SatelliteSelector_SelectedPassChanged;
      // 
      // ClockPanel
      // 
      ClockPanel.Controls.Add(Clock);
      ClockPanel.Dock = DockStyle.Right;
      ClockPanel.Location = new Point(1056, 0);
      ClockPanel.Name = "ClockPanel";
      ClockPanel.Padding = new Padding(3);
      ClockPanel.Size = new Size(142, 38);
      ClockPanel.TabIndex = 1;
      // 
      // Clock
      // 
      Clock.BackColor = Color.MidnightBlue;
      Clock.Dock = DockStyle.Fill;
      Clock.Location = new Point(3, 3);
      Clock.Margin = new Padding(2, 3, 2, 3);
      Clock.Name = "Clock";
      Clock.Size = new Size(136, 32);
      Clock.TabIndex = 1;
      Clock.UtcMode = true;
      // 
      // DockHost
      // 
      DockHost.DefaultFloatWindowSize = new Size(445, 445);
      DockHost.Dock = DockStyle.Fill;
      DockHost.DockBackColor = Color.FromArgb(238, 238, 242);
      DockHost.Location = new Point(0, 64);
      DockHost.Name = "DockHost";
      DockHost.Padding = new Padding(6);
      DockHost.ShowAutoHideContentOnHover = false;
      DockHost.Size = new Size(1200, 611);
      DockHost.TabIndex = 4;
      DockHost.Theme = vS2015LightTheme1;
      // 
      // menuStrip1
      // 
      menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, GroupViewPanelMNU, toolsToolStripMenuItem, helpToolStripMenuItem });
      menuStrip1.Location = new Point(0, 0);
      menuStrip1.Name = "menuStrip1";
      menuStrip1.Size = new Size(1200, 24);
      menuStrip1.TabIndex = 5;
      menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ExitMNU });
      fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      fileToolStripMenuItem.Size = new Size(37, 20);
      fileToolStripMenuItem.Text = "File";
      // 
      // ExitMNU
      // 
      ExitMNU.Name = "ExitMNU";
      ExitMNU.Size = new Size(93, 22);
      ExitMNU.Text = "Exit";
      ExitMNU.Click += ExitMNU_Click;
      // 
      // GroupViewPanelMNU
      // 
      GroupViewPanelMNU.DropDownItems.AddRange(new ToolStripItem[] { GroupViewMNU, SatelliteDetailsMNU, TransmittersMNU, SatellitePassesMNU, WaterfallMNU, TimelineMNU, SkyViewMNU, EarthViewMNU });
      GroupViewPanelMNU.Name = "GroupViewPanelMNU";
      GroupViewPanelMNU.Size = new Size(44, 20);
      GroupViewPanelMNU.Text = "View";
      // 
      // GroupViewMNU
      // 
      GroupViewMNU.Name = "GroupViewMNU";
      GroupViewMNU.Size = new Size(182, 22);
      GroupViewMNU.Text = "Group";
      GroupViewMNU.Click += GroupViewMNU_Click;
      // 
      // SatelliteDetailsMNU
      // 
      SatelliteDetailsMNU.Name = "SatelliteDetailsMNU";
      SatelliteDetailsMNU.Size = new Size(182, 22);
      SatelliteDetailsMNU.Text = "Satellite Details";
      SatelliteDetailsMNU.Click += SatelliteDetailsMNU_Click;
      // 
      // TransmittersMNU
      // 
      TransmittersMNU.Name = "TransmittersMNU";
      TransmittersMNU.Size = new Size(182, 22);
      TransmittersMNU.Text = "Satellite Transmitters";
      TransmittersMNU.Click += TransmittersMNU_Click;
      // 
      // SatellitePassesMNU
      // 
      SatellitePassesMNU.Name = "SatellitePassesMNU";
      SatellitePassesMNU.Size = new Size(182, 22);
      SatellitePassesMNU.Text = "Satellite Passes";
      SatellitePassesMNU.Click += SatellitePassesMNU_Click;
      // 
      // WaterfallMNU
      // 
      WaterfallMNU.Name = "WaterfallMNU";
      WaterfallMNU.Size = new Size(182, 22);
      WaterfallMNU.Text = "Waterfall";
      // 
      // TimelineMNU
      // 
      TimelineMNU.Name = "TimelineMNU";
      TimelineMNU.Size = new Size(182, 22);
      TimelineMNU.Text = "Timeline";
      TimelineMNU.Click += TimelineMNU_Click;
      // 
      // SkyViewMNU
      // 
      SkyViewMNU.Name = "SkyViewMNU";
      SkyViewMNU.Size = new Size(182, 22);
      SkyViewMNU.Text = "SkyV iew";
      SkyViewMNU.Click += SkyViewMNU_Click;
      // 
      // EarthViewMNU
      // 
      EarthViewMNU.Name = "EarthViewMNU";
      EarthViewMNU.Size = new Size(182, 22);
      EarthViewMNU.Text = "Earth View";
      EarthViewMNU.Click += EarthViewMNU_Click;
      // 
      // toolsToolStripMenuItem
      // 
      toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { SatelliteGroupsMNU, SdrDevicesMNU, SettingsMNU, toolStripMenuItem1, DownloadSatDataMNU, DownloadTleMNU });
      toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      toolsToolStripMenuItem.Size = new Size(46, 20);
      toolsToolStripMenuItem.Text = "Tools";
      // 
      // SatelliteGroupsMNU
      // 
      SatelliteGroupsMNU.Name = "SatelliteGroupsMNU";
      SatelliteGroupsMNU.Size = new Size(216, 22);
      SatelliteGroupsMNU.Text = "Satellites and Groups...";
      SatelliteGroupsMNU.Click += EditGroupsMNU_Click;
      // 
      // SdrDevicesMNU
      // 
      SdrDevicesMNU.Name = "SdrDevicesMNU";
      SdrDevicesMNU.Size = new Size(216, 22);
      SdrDevicesMNU.Text = "SDR Devices...";
      SdrDevicesMNU.Click += SdrDevicesMNU_Click;
      // 
      // SettingsMNU
      // 
      SettingsMNU.Name = "SettingsMNU";
      SettingsMNU.Size = new Size(216, 22);
      SettingsMNU.Text = "Settings...";
      SettingsMNU.Click += SettingsMNU_Click;
      // 
      // toolStripMenuItem1
      // 
      toolStripMenuItem1.Name = "toolStripMenuItem1";
      toolStripMenuItem1.Size = new Size(213, 6);
      // 
      // DownloadSatDataMNU
      // 
      DownloadSatDataMNU.Name = "DownloadSatDataMNU";
      DownloadSatDataMNU.Size = new Size(216, 22);
      DownloadSatDataMNU.Text = "Download All Satellite Data";
      DownloadSatDataMNU.Click += DownloadSatDataMNU_Click;
      // 
      // DownloadTleMNU
      // 
      DownloadTleMNU.Name = "DownloadTleMNU";
      DownloadTleMNU.Size = new Size(216, 22);
      DownloadTleMNU.Text = "Download Only TLE";
      DownloadTleMNU.Click += DownloadTleMNU_Click;
      // 
      // helpToolStripMenuItem
      // 
      helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OnlineHelpMNU, emailTheAuthorToolStripMenuItem, DataFolderMNU, toolStripMenuItem2, AboutMNU });
      helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      helpToolStripMenuItem.Size = new Size(44, 20);
      helpToolStripMenuItem.Text = "Help";
      // 
      // OnlineHelpMNU
      // 
      OnlineHelpMNU.Name = "OnlineHelpMNU";
      OnlineHelpMNU.Size = new Size(172, 22);
      OnlineHelpMNU.Text = "Online Help...";
      OnlineHelpMNU.Click += WebsiteMNU_Click;
      // 
      // emailTheAuthorToolStripMenuItem
      // 
      emailTheAuthorToolStripMenuItem.Name = "emailTheAuthorToolStripMenuItem";
      emailTheAuthorToolStripMenuItem.Size = new Size(172, 22);
      emailTheAuthorToolStripMenuItem.Text = "Email the Author...";
      emailTheAuthorToolStripMenuItem.Click += EmailTheAuthorMNU_Click;
      // 
      // DataFolderMNU
      // 
      DataFolderMNU.Name = "DataFolderMNU";
      DataFolderMNU.Size = new Size(172, 22);
      DataFolderMNU.Text = "Data Folder...";
      DataFolderMNU.Click += DataFolderMNU_Click;
      // 
      // toolStripMenuItem2
      // 
      toolStripMenuItem2.Name = "toolStripMenuItem2";
      toolStripMenuItem2.Size = new Size(169, 6);
      // 
      // AboutMNU
      // 
      AboutMNU.Name = "AboutMNU";
      AboutMNU.Size = new Size(172, 22);
      AboutMNU.Text = "About...";
      AboutMNU.Click += AboutMNU_Click;
      // 
      // timer
      // 
      timer.Enabled = true;
      timer.Interval = 250;
      timer.Tick += timer_Tick;
      // 
      // StatusStrip
      // 
      StatusStrip.ImageScalingSize = new Size(24, 24);
      StatusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel2, SdrLedLabel, SdrStatusLabel, SoundcardLedLabel, SoundcardStatusLabel, VacLedLabel, VacStatusLabel, OmniRigLedLabel, OmniRigStatusLabel, SatDataLedLabel, SatDataStatusLabel, IqOutputLedLabel, IqOutputStatusLabel, NoiseFloorLabel, CpuLoadlabel });
      StatusStrip.Location = new Point(0, 675);
      StatusStrip.Name = "StatusStrip";
      StatusStrip.ShowItemToolTips = true;
      StatusStrip.Size = new Size(1200, 35);
      StatusStrip.TabIndex = 6;
      StatusStrip.Text = "statusStrip1";
      // 
      // toolStripStatusLabel2
      // 
      toolStripStatusLabel2.AutoSize = false;
      toolStripStatusLabel2.Name = "toolStripStatusLabel2";
      toolStripStatusLabel2.Size = new Size(10, 30);
      // 
      // SdrLedLabel
      // 
      SdrLedLabel.Font = new Font("Webdings", 9F);
      SdrLedLabel.ForeColor = Color.Gray;
      SdrLedLabel.Name = "SdrLedLabel";
      SdrLedLabel.Size = new Size(21, 30);
      SdrLedLabel.Text = "n";
      SdrLedLabel.ToolTipText = "Disabled";
      SdrLedLabel.Click += SdrStatus_Click;
      SdrLedLabel.MouseEnter += StatusLabel_MouseEnter;
      SdrLedLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // SdrStatusLabel
      // 
      SdrStatusLabel.Font = new Font("Segoe UI", 10F);
      SdrStatusLabel.Name = "SdrStatusLabel";
      SdrStatusLabel.Size = new Size(42, 30);
      SdrStatusLabel.Text = "SDR  ";
      SdrStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      SdrStatusLabel.ToolTipText = "Disabled";
      SdrStatusLabel.Click += SdrStatus_Click;
      SdrStatusLabel.MouseEnter += StatusLabel_MouseEnter;
      SdrStatusLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // SoundcardLedLabel
      // 
      SoundcardLedLabel.Font = new Font("Webdings", 9F);
      SoundcardLedLabel.ForeColor = Color.Gray;
      SoundcardLedLabel.Name = "SoundcardLedLabel";
      SoundcardLedLabel.Size = new Size(21, 30);
      SoundcardLedLabel.Text = "n";
      SoundcardLedLabel.Visible = false;
      // 
      // SoundcardStatusLabel
      // 
      SoundcardStatusLabel.Font = new Font("Segoe UI", 10F);
      SoundcardStatusLabel.Name = "SoundcardStatusLabel";
      SoundcardStatusLabel.Size = new Size(82, 30);
      SoundcardStatusLabel.Text = "Soundcard  ";
      SoundcardStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      SoundcardStatusLabel.Visible = false;
      // 
      // VacLedLabel
      // 
      VacLedLabel.Font = new Font("Webdings", 9F);
      VacLedLabel.ForeColor = Color.Gray;
      VacLedLabel.Name = "VacLedLabel";
      VacLedLabel.Size = new Size(21, 30);
      VacLedLabel.Text = "n";
      VacLedLabel.Visible = false;
      // 
      // VacStatusLabel
      // 
      VacStatusLabel.Font = new Font("Segoe UI", 10F);
      VacStatusLabel.Name = "VacStatusLabel";
      VacStatusLabel.Size = new Size(43, 30);
      VacStatusLabel.Text = "VAC  ";
      VacStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      VacStatusLabel.Visible = false;
      // 
      // OmniRigLedLabel
      // 
      OmniRigLedLabel.Font = new Font("Webdings", 9F);
      OmniRigLedLabel.ForeColor = Color.Gray;
      OmniRigLedLabel.Name = "OmniRigLedLabel";
      OmniRigLedLabel.Size = new Size(21, 30);
      OmniRigLedLabel.Text = "n";
      OmniRigLedLabel.Visible = false;
      // 
      // OmniRigStatusLabel
      // 
      OmniRigStatusLabel.Font = new Font("Segoe UI", 10F);
      OmniRigStatusLabel.Name = "OmniRigStatusLabel";
      OmniRigStatusLabel.Size = new Size(70, 30);
      OmniRigStatusLabel.Text = "OmniRig  ";
      OmniRigStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      OmniRigStatusLabel.Visible = false;
      // 
      // SatDataLedLabel
      // 
      SatDataLedLabel.Font = new Font("Webdings", 9F);
      SatDataLedLabel.ForeColor = Color.Gray;
      SatDataLedLabel.Name = "SatDataLedLabel";
      SatDataLedLabel.Size = new Size(21, 30);
      SatDataLedLabel.Text = "n";
      // 
      // SatDataStatusLabel
      // 
      SatDataStatusLabel.Font = new Font("Segoe UI", 10F);
      SatDataStatusLabel.Name = "SatDataStatusLabel";
      SatDataStatusLabel.Size = new Size(89, 30);
      SatDataStatusLabel.Text = "Satellite Data";
      SatDataStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      SatDataStatusLabel.ToolTipText = "Network";
      // 
      // IqOutputLedLabel
      // 
      IqOutputLedLabel.Font = new Font("Webdings", 9F);
      IqOutputLedLabel.ForeColor = Color.Gray;
      IqOutputLedLabel.Name = "IqOutputLedLabel";
      IqOutputLedLabel.Size = new Size(21, 30);
      IqOutputLedLabel.Text = "n";
      IqOutputLedLabel.Visible = false;
      // 
      // IqOutputStatusLabel
      // 
      IqOutputStatusLabel.Font = new Font("Segoe UI", 10F);
      IqOutputStatusLabel.Name = "IqOutputStatusLabel";
      IqOutputStatusLabel.Size = new Size(86, 30);
      IqOutputStatusLabel.Text = "I/Q Output  ";
      IqOutputStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      IqOutputStatusLabel.Visible = false;
      // 
      // NoiseFloorLabel
      // 
      NoiseFloorLabel.Name = "NoiseFloorLabel";
      NoiseFloorLabel.Size = new Size(119, 30);
      NoiseFloorLabel.Text = "Noise Floor: -100 dB  ";
      NoiseFloorLabel.TextAlign = ContentAlignment.MiddleLeft;
      NoiseFloorLabel.Visible = false;
      // 
      // CpuLoadlabel
      // 
      CpuLoadlabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
      CpuLoadlabel.Name = "CpuLoadlabel";
      CpuLoadlabel.Size = new Size(96, 30);
      CpuLoadlabel.Text = "CPU Load: 00.0%";
      CpuLoadlabel.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // MainForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(1200, 710);
      Controls.Add(DockHost);
      Controls.Add(Toolbar);
      Controls.Add(menuStrip1);
      Controls.Add(StatusStrip);
      MainMenuStrip = menuStrip1;
      Name = "MainForm";
      Text = "Form1";
      FormClosing += MainForm_FormClosing;
      Load += MainForm_Load;
      Toolbar.ResumeLayout(false);
      ClockPanel.ResumeLayout(false);
      menuStrip1.ResumeLayout(false);
      menuStrip1.PerformLayout();
      StatusStrip.ResumeLayout(false);
      StatusStrip.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Panel Toolbar;
    public WeifenLuo.WinFormsUI.Docking.DockPanel DockHost;
    private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme1;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem ExitMNU;
    private ToolStripMenuItem GroupViewPanelMNU;
    private ToolStripMenuItem toolsToolStripMenuItem;
    private ToolStripMenuItem SdrDevicesMNU;
    private ToolStripMenuItem SettingsMNU;
    private ToolStripMenuItem helpToolStripMenuItem;
    private ToolStripMenuItem OnlineHelpMNU;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem AboutMNU;
    private Panel ClockPanel;
    private VE3NEA.Clock.Clock Clock;
    private ToolStripMenuItem SatelliteGroupsMNU;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem DownloadSatDataMNU;
    private ToolStripMenuItem DataFolderMNU;
    public ToolStripMenuItem WaterfallMNU;
    public ToolStripMenuItem GroupViewMNU;
    public ToolStripMenuItem SatelliteDetailsMNU;
    private SatelliteSelector SatelliteSelector;
    public ToolStripMenuItem SatellitePassesMNU;
    private System.Windows.Forms.Timer timer;
    public ToolStripMenuItem TimelineMNU;
    public ToolStripMenuItem SkyViewMNU;
    public ToolStripMenuItem EarthViewMNU;
    private ToolStripMenuItem emailTheAuthorToolStripMenuItem;
    private StatusStrip StatusStrip;
    private ToolStripStatusLabel toolStripStatusLabel2;
    private ToolStripStatusLabel SdrLedLabel;
    private ToolStripStatusLabel SdrStatusLabel;
    private ToolStripStatusLabel SoundcardLedLabel;
    private ToolStripStatusLabel SoundcardStatusLabel;
    private ToolStripStatusLabel VacLedLabel;
    private ToolStripStatusLabel VacStatusLabel;
    private ToolStripStatusLabel OmniRigLedLabel;
    private ToolStripStatusLabel OmniRigStatusLabel;
    private ToolStripStatusLabel SatDataLedLabel;
    private ToolStripStatusLabel SatDataStatusLabel;
    private ToolStripStatusLabel IqOutputLedLabel;
    private ToolStripStatusLabel IqOutputStatusLabel;
    private ToolStripStatusLabel NoiseFloorLabel;
    private ToolStripStatusLabel CpuLoadlabel;
    private ToolTip toolTip1;
    private ToolStripMenuItem DownloadTleMNU;
    public ToolStripMenuItem TransmittersMNU;
  }
}
