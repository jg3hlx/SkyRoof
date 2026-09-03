namespace SkyRoof
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      Toolbar = new Panel();
      RotatorWidget = new RotatorWidget();
      panel7 = new Panel();
      GainWidget = new GainWidget();
      panel3 = new Panel();
      panel6 = new Panel();
      FrequencyWidget = new FrequencyWidget();
      panel1 = new Panel();
      SatellitePhotoWidget = new SatellitePhotoWidget();
      ClockPanel = new Panel();
      Clock = new VE3NEA.Clock.ClockWidget();
      panel5 = new Panel();
      SatellitePhotoSeparator = new Panel();
      SatelliteSelecionWidget = new SatelliteSelectorWidget();
      panel2 = new Panel();
      DockHost = new WeifenLuo.WinFormsUI.Docking.DockPanel();
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
      QsoEntryMNU = new ToolStripMenuItem();
      Ft4ConsoleMNU = new ToolStripMenuItem();
      RecorderMNU = new ToolStripMenuItem();
      QsoSchedulerMNU = new ToolStripMenuItem();
      TelemetryMNU = new ToolStripMenuItem();
      AutoSelectionMNU = new ToolStripMenuItem();
      toolStripMenuItem4 = new ToolStripSeparator();
      ResetWindowLayoutMNU = new ToolStripMenuItem();
      toolsToolStripMenuItem = new ToolStripMenuItem();
      SatelliteGroupsMNU = new ToolStripMenuItem();
      SdrDevicesMNU = new ToolStripMenuItem();
      SettingsMNU = new ToolStripMenuItem();
      ThemeMNU = new ToolStripMenuItem();
      ThemeSystemMNU = new ToolStripMenuItem();
      ThemeLightMNU = new ToolStripMenuItem();
      ThemeDarkMNU = new ToolStripMenuItem();
      toolStripMenuItem1 = new ToolStripSeparator();
      DownloadSatDataMNU = new ToolStripMenuItem();
      DownloadTleMNU = new ToolStripMenuItem();
      DownloadAmsatMNU = new ToolStripMenuItem();
      toolStripMenuItem3 = new ToolStripSeparator();
      LoadTleMNU = new ToolStripMenuItem();
      helpToolStripMenuItem = new ToolStripMenuItem();
      OnlineHelpMNU = new ToolStripMenuItem();
      SupportGroupMNU = new ToolStripMenuItem();
      DataFolderMNU = new ToolStripMenuItem();
      toolStripMenuItem2 = new ToolStripSeparator();
      AboutMNU = new ToolStripMenuItem();
      timer = new System.Windows.Forms.Timer(components);
      StatusStrip = new StatusStrip();
      toolStripStatusLabel2 = new ToolStripStatusLabel();
      SatDataLedLabel = new ToolStripStatusLabel();
      SatDataStatusLabel = new ToolStripStatusLabel();
      SdrLedLabel = new ToolStripStatusLabel();
      SdrStatusLabel = new ToolStripStatusLabel();
      SoundcardLedLabel = new ToolStripStatusLabel();
      SoundcardStatusLabel = new ToolStripStatusLabel();
      SoundcardDropdownBtn = new ToolStripDropDownButton();
      VacLedLabel = new ToolStripStatusLabel();
      VacStatusLabel = new ToolStripStatusLabel();
      RxCatLedLabel = new ToolStripStatusLabel();
      RxCatStatusLabel = new ToolStripStatusLabel();
      TxCatLedLabel = new ToolStripStatusLabel();
      TxCatStatusLabel = new ToolStripStatusLabel();
      IqOutputLedLabel = new ToolStripStatusLabel();
      IqOutputStatusLabel = new ToolStripStatusLabel();
      RotatorLedLabel = new ToolStripStatusLabel();
      RotatorStatusLabel = new ToolStripStatusLabel();
      RotatorDropdownBtn = new ToolStripDropDownButton();
      EnableRotatorMNU = new ToolStripMenuItem();
      TrackRotatorMNU = new ToolStripMenuItem();
      NoiseFloorLabel = new ToolStripStatusLabel();
      CpuLoadlabel = new ToolStripStatusLabel();
      UpdateLabel = new ToolStripStatusLabel();
      toolTip1 = new VE3NEA.ToolTipEx(components);
      panel4 = new Panel();
      Toolbar.SuspendLayout();
      panel3.SuspendLayout();
      ClockPanel.SuspendLayout();
      menuStrip1.SuspendLayout();
      StatusStrip.SuspendLayout();
      SuspendLayout();
      // 
      // Toolbar
      // 
      Toolbar.Controls.Add(RotatorWidget);
      Toolbar.Controls.Add(panel7);
      Toolbar.Controls.Add(GainWidget);
      Toolbar.Controls.Add(panel3);
      Toolbar.Controls.Add(FrequencyWidget);
      Toolbar.Controls.Add(panel1);
      Toolbar.Controls.Add(SatellitePhotoWidget);
      Toolbar.Controls.Add(ClockPanel);
      Toolbar.Controls.Add(panel5);
      Toolbar.Controls.Add(SatellitePhotoSeparator);
      Toolbar.Controls.Add(SatelliteSelecionWidget);
      Toolbar.Controls.Add(panel2);
      Toolbar.Dock = DockStyle.Top;
      Toolbar.Location = new Point(0, 24);
      Toolbar.Name = "Toolbar";
      Toolbar.Size = new Size(1834, 78);
      Toolbar.TabIndex = 0;
      Toolbar.Resize += Toolbar_Resize;
      // 
      // RotatorWidget
      // 
      RotatorWidget.BorderStyle = BorderStyle.FixedSingle;
      RotatorWidget.Dock = DockStyle.Left;
      RotatorWidget.Location = new Point(1342, 0);
      RotatorWidget.Name = "RotatorWidget";
      RotatorWidget.Size = new Size(210, 78);
      RotatorWidget.TabIndex = 8;
      // 
      // panel7
      // 
      panel7.Dock = DockStyle.Left;
      panel7.Location = new Point(1338, 0);
      panel7.Name = "panel7";
      panel7.Size = new Size(4, 78);
      panel7.TabIndex = 10;
      // 
      // GainWidget
      // 
      GainWidget.BorderStyle = BorderStyle.FixedSingle;
      GainWidget.Dock = DockStyle.Left;
      GainWidget.Location = new Point(1128, 0);
      GainWidget.Name = "GainWidget";
      GainWidget.Size = new Size(210, 78);
      GainWidget.TabIndex = 7;
      // 
      // panel3
      // 
      panel3.Controls.Add(panel6);
      panel3.Dock = DockStyle.Left;
      panel3.Location = new Point(1124, 0);
      panel3.Name = "panel3";
      panel3.Size = new Size(4, 78);
      panel3.TabIndex = 6;
      // 
      // panel6
      // 
      panel6.Dock = DockStyle.Left;
      panel6.Location = new Point(0, 0);
      panel6.Name = "panel6";
      panel6.Size = new Size(4, 78);
      panel6.TabIndex = 7;
      // 
      // FrequencyWidget
      // 
      FrequencyWidget.BorderStyle = BorderStyle.FixedSingle;
      FrequencyWidget.Dock = DockStyle.Left;
      FrequencyWidget.Location = new Point(442, 0);
      FrequencyWidget.Name = "FrequencyWidget";
      FrequencyWidget.Size = new Size(682, 78);
      FrequencyWidget.TabIndex = 3;
      // 
      // panel1
      // 
      panel1.Dock = DockStyle.Left;
      panel1.Location = new Point(438, 0);
      panel1.Name = "panel1";
      panel1.Size = new Size(4, 78);
      panel1.TabIndex = 4;
      // 
      // SatellitePhotoWidget
      // 
      SatellitePhotoWidget.BorderStyle = BorderStyle.FixedSingle;
      SatellitePhotoWidget.Dock = DockStyle.Left;
      SatellitePhotoWidget.Location = new Point(348, 0);
      SatellitePhotoWidget.Name = "SatellitePhotoWidget";
      SatellitePhotoWidget.Size = new Size(90, 78);
      SatellitePhotoWidget.TabIndex = 11;
      // 
      // ClockPanel
      // 
      ClockPanel.BorderStyle = BorderStyle.FixedSingle;
      ClockPanel.Controls.Add(Clock);
      ClockPanel.Dock = DockStyle.Right;
      ClockPanel.Location = new Point(1713, 0);
      ClockPanel.Name = "ClockPanel";
      ClockPanel.Padding = new Padding(3);
      ClockPanel.Size = new Size(117, 78);
      ClockPanel.TabIndex = 1;
      // 
      // Clock
      // 
      Clock.BackColor = Color.MidnightBlue;
      Clock.BorderStyle = BorderStyle.FixedSingle;
      Clock.Dock = DockStyle.Fill;
      Clock.Location = new Point(3, 3);
      Clock.Margin = new Padding(5);
      Clock.Name = "Clock";
      Clock.Size = new Size(109, 70);
      Clock.TabIndex = 1;
      Clock.UtcMode = true;
      // 
      // panel5
      // 
      panel5.Dock = DockStyle.Right;
      panel5.Location = new Point(1830, 0);
      panel5.Name = "panel5";
      panel5.Size = new Size(4, 78);
      panel5.TabIndex = 9;
      // 
      // SatellitePhotoSeparator
      // 
      SatellitePhotoSeparator.Dock = DockStyle.Left;
      SatellitePhotoSeparator.Location = new Point(344, 0);
      SatellitePhotoSeparator.Name = "SatellitePhotoSeparator";
      SatellitePhotoSeparator.Size = new Size(4, 78);
      SatellitePhotoSeparator.TabIndex = 12;
      // 
      // SatelliteSelecionWidget
      // 
      SatelliteSelecionWidget.BorderStyle = BorderStyle.FixedSingle;
      SatelliteSelecionWidget.Dock = DockStyle.Left;
      SatelliteSelecionWidget.Location = new Point(4, 0);
      SatelliteSelecionWidget.Name = "SatelliteSelecionWidget";
      SatelliteSelecionWidget.Size = new Size(340, 78);
      SatelliteSelecionWidget.TabIndex = 2;
      SatelliteSelecionWidget.SelectedGroupChanged += SatelliteSelector_SelectedGroupChanged;
      SatelliteSelecionWidget.SelectedSatelliteChanged += SatelliteSelector_SelectedSatelliteChanged;
      SatelliteSelecionWidget.SelectedTransmitterChanged += SatelliteSelector_SelectedTransmitterChanged;
      SatelliteSelecionWidget.SelectedPassChanged += SatelliteSelector_SelectedPassChanged;
      // 
      // panel2
      // 
      panel2.Dock = DockStyle.Left;
      panel2.Location = new Point(0, 0);
      panel2.Name = "panel2";
      panel2.Size = new Size(4, 78);
      panel2.TabIndex = 5;
      // 
      // DockHost
      // 
      DockHost.DefaultFloatWindowSize = new Size(445, 445);
      DockHost.Dock = DockStyle.Fill;
      DockHost.Location = new Point(0, 102);
      DockHost.Name = "DockHost";
      DockHost.ShowAutoHideContentOnHover = false;
      DockHost.Size = new Size(1834, 824);
      DockHost.TabIndex = 4;
      // 
      // menuStrip1
      // 
      menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, GroupViewPanelMNU, toolsToolStripMenuItem, helpToolStripMenuItem });
      menuStrip1.Location = new Point(0, 0);
      menuStrip1.Name = "メニュー1";
      menuStrip1.Size = new Size(1834, 24);
      menuStrip1.TabIndex = 5;
      menuStrip1.Text = "メニュー1";
      // 
      // fileToolStripMenuItem
      // 
      fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ExitMNU });
      fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      fileToolStripMenuItem.Size = new Size(53, 20);
      fileToolStripMenuItem.Text = "ファイル";
      // 
      // ExitMNU
      // 
      ExitMNU.Name = "ExitMNU";
      ExitMNU.Size = new Size(180, 22);
      ExitMNU.Text = "終了";
      ExitMNU.Click += ExitMNU_Click;
      // 
      // GroupViewPanelMNU
      // 
      GroupViewPanelMNU.DropDownItems.AddRange(new ToolStripItem[] { GroupViewMNU, SatelliteDetailsMNU, TransmittersMNU, SatellitePassesMNU, WaterfallMNU, TimelineMNU, SkyViewMNU, EarthViewMNU, QsoEntryMNU, Ft4ConsoleMNU, RecorderMNU, QsoSchedulerMNU, TelemetryMNU, AutoSelectionMNU, toolStripMenuItem4, ResetWindowLayoutMNU });
      GroupViewPanelMNU.Name = "GroupViewPanelMNU";
      GroupViewPanelMNU.Size = new Size(44, 20);
      GroupViewPanelMNU.Text = "表示";
      // 
      // GroupViewMNU
      // 
      GroupViewMNU.Name = "GroupViewMNU";
      GroupViewMNU.Size = new Size(188, 22);
      GroupViewMNU.Text = "グループ";
      GroupViewMNU.Click += GroupViewMNU_Click;
      // 
      // SatelliteDetailsMNU
      // 
      SatelliteDetailsMNU.Name = "SatelliteDetailsMNU";
      SatelliteDetailsMNU.Size = new Size(188, 22);
      SatelliteDetailsMNU.Text = "衛星詳細";
      SatelliteDetailsMNU.Click += SatelliteDetailsMNU_Click;
      // 
      // TransmittersMNU
      // 
      TransmittersMNU.Name = "TransmittersMNU";
      TransmittersMNU.Size = new Size(188, 22);
      TransmittersMNU.Text = "衛星トランスミッター";
      TransmittersMNU.Click += TransmittersMNU_Click;
      // 
      // SatellitePassesMNU
      // 
      SatellitePassesMNU.Name = "SatellitePassesMNU";
      SatellitePassesMNU.Size = new Size(188, 22);
      SatellitePassesMNU.Text = "衛星パス";
      SatellitePassesMNU.Click += SatellitePassesMNU_Click;
      // 
      // WaterfallMNU
      // 
      WaterfallMNU.Name = "WaterfallMNU";
      WaterfallMNU.Size = new Size(188, 22);
      WaterfallMNU.Text = "ウォータフォール";
      WaterfallMNU.Click += WaterfallMNU_Click;
      // 
      // TimelineMNU
      // 
      TimelineMNU.Name = "TimelineMNU";
      TimelineMNU.Size = new Size(188, 22);
      TimelineMNU.Text = "タイムライン";
      TimelineMNU.Click += TimelineMNU_Click;
      // 
      // SkyViewMNU
      // 
      SkyViewMNU.Name = "SkyViewMNU";
      SkyViewMNU.Size = new Size(188, 22);
      SkyViewMNU.Text = "スカイビュー";
      SkyViewMNU.Click += SkyViewMNU_Click;
      // 
      // EarthViewMNU
      // 
      EarthViewMNU.Name = "EarthViewMNU";
      EarthViewMNU.Size = new Size(188, 22);
      EarthViewMNU.Text = "アースビュー";
      EarthViewMNU.Click += EarthViewMNU_Click;
      // 
      // QsoEntryMNU
      // 
      QsoEntryMNU.Name = "QsoEntryMNU";
      QsoEntryMNU.Size = new Size(188, 22);
      QsoEntryMNU.Text = "&QSO入力";
      QsoEntryMNU.Click += QsoEntryMNU_Click;
      // 
      // Ft4ConsoleMNU
      // 
      Ft4ConsoleMNU.Name = "Ft4ConsoleMNU";
      Ft4ConsoleMNU.Size = new Size(188, 22);
      Ft4ConsoleMNU.Text = "&FT4コンソール";
      Ft4ConsoleMNU.Click += Ft4ConsoleMNU_Click;
      // 
      // RecorderMNU
      // 
      RecorderMNU.Name = "RecorderMNU";
      RecorderMNU.Size = new Size(188, 22);
      RecorderMNU.Text = "録音";
      RecorderMNU.Click += RecorderMNU_Click;
      // 
      // QsoSchedulerMNU
      // 
      QsoSchedulerMNU.Name = "QsoSchedulerMNU";
      QsoSchedulerMNU.Size = new Size(188, 22);
      QsoSchedulerMNU.Text = "スケジュールQSO";
      QsoSchedulerMNU.Click += QsoSchedulerMNU_Click;
      // 
      // TelemetryMNU
      // 
      TelemetryMNU.Name = "TelemetryMNU";
      TelemetryMNU.Size = new Size(188, 22);
      TelemetryMNU.Text = "テレメトリー";
      TelemetryMNU.Click += TelemetryMNU_Click;
      // 
      // AutoSelectionMNU
      // 
      AutoSelectionMNU.Name = "AutoSelectionMNU";
      AutoSelectionMNU.Size = new Size(188, 22);
      AutoSelectionMNU.Text = "自動選択";
      AutoSelectionMNU.Click += AutoSelectionMNU_Click;
      // 
      // toolStripMenuItem4
      // 
      toolStripMenuItem4.Name = "toolStripMenuItem4";
      toolStripMenuItem4.Size = new Size(185, 6);
      // 
      // ResetWindowLayoutMNU
      // 
      ResetWindowLayoutMNU.Name = "ResetWindowLayoutMNU";
      ResetWindowLayoutMNU.Size = new Size(188, 22);
      ResetWindowLayoutMNU.Text = "ウィンドウ レイアウトのリセット";
      ResetWindowLayoutMNU.Click += ResetWindowLayoutMNU_Click;
      // 
      // toolsToolStripMenuItem
      // 
      toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { SatelliteGroupsMNU, SdrDevicesMNU, SettingsMNU, ThemeMNU, toolStripMenuItem1, DownloadSatDataMNU, DownloadTleMNU, DownloadAmsatMNU, toolStripMenuItem3, LoadTleMNU });
      toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      toolsToolStripMenuItem.Size = new Size(46, 20);
      toolsToolStripMenuItem.Text = "ツール";
      // 
      // SatelliteGroupsMNU
      // 
      SatelliteGroupsMNU.Name = "SatelliteGroupsMNU";
      SatelliteGroupsMNU.Size = new Size(216, 22);
      SatelliteGroupsMNU.Text = "衛星とグループ...";
      SatelliteGroupsMNU.Click += EditGroupsMNU_Click;
      // 
      // SdrDevicesMNU
      // 
      SdrDevicesMNU.Name = "SdrDevicesMNU";
      SdrDevicesMNU.Size = new Size(216, 22);
      SdrDevicesMNU.Text = "SDR機器...";
      SdrDevicesMNU.Click += SdrDevicesMNU_Click;
      // 
      // SettingsMNU
      // 
      SettingsMNU.Name = "SettingsMNU";
      SettingsMNU.Size = new Size(216, 22);
      SettingsMNU.Text = "設定...";
      SettingsMNU.Click += SettingsMNU_Click;
      // 
      // ThemeMNU
      // 
      ThemeMNU.DropDownItems.AddRange(new ToolStripItem[] { ThemeSystemMNU, ThemeLightMNU, ThemeDarkMNU });
      ThemeMNU.Name = "ThemeMNU";
      ThemeMNU.Size = new Size(216, 22);
      ThemeMNU.Text = "テーマ";
      // 
      // ThemeSystemMNU
      // 
      ThemeSystemMNU.Name = "ThemeSystemMNU";
      ThemeSystemMNU.Size = new Size(111, 22);
      ThemeSystemMNU.Text = "システム";
      ThemeSystemMNU.Click += ThemeSystemMNU_Click;
      // 
      // ThemeLightMNU
      // 
      ThemeLightMNU.Name = "ThemeLightMNU";
      ThemeLightMNU.Size = new Size(111, 22);
      ThemeLightMNU.Text = "ライト";
      ThemeLightMNU.Click += ThemeLightMNU_Click;
      // 
      // ThemeDarkMNU
      // 
      ThemeDarkMNU.Name = "ThemeDarkMNU";
      ThemeDarkMNU.Size = new Size(111, 22);
      ThemeDarkMNU.Text = "ダーク";
      ThemeDarkMNU.Click += ThemeDarkMNU_Click;
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
      DownloadSatDataMNU.Text = "全衛星データのダウンロード";
      DownloadSatDataMNU.Click += DownloadSatDataMNU_Click;
      // 
      // DownloadTleMNU
      // 
      DownloadTleMNU.Name = "DownloadTleMNU";
      DownloadTleMNU.Size = new Size(216, 22);
      DownloadTleMNU.Text = "TLEのダウンロード";
      DownloadTleMNU.Click += DownloadTleMNU_Click;
      // 
      // DownloadAmsatMNU
      // 
      DownloadAmsatMNU.Name = "DownloadAmsatMNU";
      DownloadAmsatMNU.Size = new Size(216, 22);
      DownloadAmsatMNU.Text = "AMSATステータスのダウンロード";
      DownloadAmsatMNU.Click += DownloadAmsatMNU_Click;
      // 
      // toolStripMenuItem3
      // 
      toolStripMenuItem3.Name = "toolStripMenuItem3";
      toolStripMenuItem3.Size = new Size(213, 6);
      // 
      // LoadTleMNU
      // 
      LoadTleMNU.Name = "LoadTleMNU";
      LoadTleMNU.Size = new Size(216, 22);
      LoadTleMNU.Text = "ファイルからTLEを読み込む...";
      LoadTleMNU.Click += LoadTleMNU_Click;
      // 
      // helpToolStripMenuItem
      // 
      helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OnlineHelpMNU, SupportGroupMNU, DataFolderMNU, toolStripMenuItem2, AboutMNU });
      helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      helpToolStripMenuItem.Size = new Size(44, 20);
      helpToolStripMenuItem.Text = "ヘルプ";
      // 
      // OnlineHelpMNU
      // 
      OnlineHelpMNU.Name = "OnlineHelpMNU";
      OnlineHelpMNU.Size = new Size(161, 22);
      OnlineHelpMNU.Text = "Webヘルプ...";
      OnlineHelpMNU.Click += WebsiteMNU_Click;
      // 
      // SupportGroupMNU
      // 
      SupportGroupMNU.Name = "SupportGroupMNU";
      SupportGroupMNU.Size = new Size(161, 22);
      SupportGroupMNU.Text = "支援グループ...";
      SupportGroupMNU.Click += SupportGroupMNU_Click;
      // 
      // DataFolderMNU
      // 
      DataFolderMNU.Name = "DataFolderMNU";
      DataFolderMNU.Size = new Size(161, 22);
      DataFolderMNU.Text = "データフォルダー...";
      DataFolderMNU.Click += DataFolderMNU_Click;
      // 
      // toolStripMenuItem2
      // 
      toolStripMenuItem2.Name = "toolStripMenuItem2";
      toolStripMenuItem2.Size = new Size(158, 6);
      // 
      // AboutMNU
      // 
      AboutMNU.Name = "AboutMNU";
      AboutMNU.Size = new Size(161, 22);
      AboutMNU.Text = "概要...";
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
      StatusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel2, SatDataLedLabel, SatDataStatusLabel, SdrLedLabel, SdrStatusLabel, SoundcardLedLabel, SoundcardStatusLabel, SoundcardDropdownBtn, VacLedLabel, VacStatusLabel, RxCatLedLabel, RxCatStatusLabel, TxCatLedLabel, TxCatStatusLabel, IqOutputLedLabel, IqOutputStatusLabel, RotatorLedLabel, RotatorStatusLabel, RotatorDropdownBtn, NoiseFloorLabel, CpuLoadlabel, UpdateLabel });
      StatusStrip.Location = new Point(0, 926);
      StatusStrip.Name = "StatusStrip";
      StatusStrip.ShowItemToolTips = true;
      StatusStrip.Size = new Size(1834, 35);
      StatusStrip.TabIndex = 6;
      StatusStrip.Text = "ステータス1";
      // 
      // toolStripStatusLabel2
      // 
      toolStripStatusLabel2.AutoSize = false;
      toolStripStatusLabel2.Name = "toolStripStatusLabel2";
      toolStripStatusLabel2.Size = new Size(10, 30);
      // 
      // SatDataLedLabel
      // 
      SatDataLedLabel.Font = new Font("Webdings", 9F);
      SatDataLedLabel.ForeColor = SystemColors.GrayText;
      SatDataLedLabel.Name = "SatDataLedLabel";
      SatDataLedLabel.Size = new Size(21, 30);
      SatDataLedLabel.Text = "n";
      // 
      // SatDataStatusLabel
      // 
      SatDataStatusLabel.Font = new Font("Segoe UI", 10F);
      SatDataStatusLabel.Name = "SatDataStatusLabel";
      SatDataStatusLabel.Size = new Size(89, 30);
      SatDataStatusLabel.Text = "衛星データ";
      SatDataStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      SatDataStatusLabel.ToolTipText = "Network";
      // 
      // SdrLedLabel
      // 
      SdrLedLabel.Font = new Font("Webdings", 9F);
      SdrLedLabel.ForeColor = SystemColors.GrayText;
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
      SoundcardLedLabel.ForeColor = SystemColors.GrayText;
      SoundcardLedLabel.Name = "SoundcardLedLabel";
      SoundcardLedLabel.Size = new Size(21, 30);
      SoundcardLedLabel.Text = "n";
      SoundcardLedLabel.Click += SoundcardLabel_Click;
      SoundcardLedLabel.MouseEnter += StatusLabel_MouseEnter;
      SoundcardLedLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // SoundcardStatusLabel
      // 
      SoundcardStatusLabel.Font = new Font("Segoe UI", 10F);
      SoundcardStatusLabel.Name = "SoundcardStatusLabel";
      SoundcardStatusLabel.Size = new Size(82, 30);
      SoundcardStatusLabel.Text = "サウンドカード ";
      SoundcardStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      SoundcardStatusLabel.Click += SoundcardLabel_Click;
      SoundcardStatusLabel.MouseEnter += StatusLabel_MouseEnter;
      SoundcardStatusLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // SoundcardDropdownBtn
      // 
      SoundcardDropdownBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
      SoundcardDropdownBtn.Name = "SoundcardDropdownBtn";
      SoundcardDropdownBtn.Size = new Size(13, 33);
      SoundcardDropdownBtn.ToolTipText = "オーディオ出力用サウンドカード";
      SoundcardDropdownBtn.DropDownOpening += SoundcardDropdownBtn_DropDownOpening;
      SoundcardDropdownBtn.MouseEnter += StatusLabel_MouseEnter;
      SoundcardDropdownBtn.MouseLeave += StatusLabel_MouseLeave;
      // 
      // VacLedLabel
      // 
      VacLedLabel.Font = new Font("Webdings", 9F);
      VacLedLabel.ForeColor = SystemColors.GrayText;
      VacLedLabel.Name = "VacLedLabel";
      VacLedLabel.Size = new Size(21, 30);
      VacLedLabel.Text = "n";
      VacLedLabel.Click += VacLabel_Click;
      VacLedLabel.MouseEnter += StatusLabel_MouseEnter;
      VacLedLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // VacStatusLabel
      // 
      VacStatusLabel.Font = new Font("Segoe UI", 10F);
      VacStatusLabel.Name = "VacStatusLabel";
      VacStatusLabel.Size = new Size(101, 30);
      VacStatusLabel.Text = "出力ストリーム";
      VacStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      VacStatusLabel.Click += VacLabel_Click;
      VacStatusLabel.MouseEnter += StatusLabel_MouseEnter;
      VacStatusLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // RxCatLedLabel
      // 
      RxCatLedLabel.Font = new Font("Webdings", 9F);
      RxCatLedLabel.ForeColor = SystemColors.GrayText;
      RxCatLedLabel.Name = "RxCatLedラベル";
      RxCatLedLabel.Size = new Size(21, 30);
      RxCatLedLabel.Text = "n";
      RxCatLedLabel.Click += RxCatLabel_Click;
      RxCatLedLabel.MouseEnter += StatusLabel_MouseEnter;
      RxCatLedLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // RxCatStatusLabel
      // 
      RxCatStatusLabel.Font = new Font("Segoe UI", 10F);
      RxCatStatusLabel.Name = "RxCatStatusLabel";
      RxCatStatusLabel.Size = new Size(53, 30);
      RxCatStatusLabel.Text = "RX CAT";
      RxCatStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      RxCatStatusLabel.Click += RxCatLabel_Click;
      RxCatStatusLabel.MouseEnter += StatusLabel_MouseEnter;
      RxCatStatusLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // TxCatLedLabel
      // 
      TxCatLedLabel.Font = new Font("Webdings", 9F);
      TxCatLedLabel.ForeColor = SystemColors.GrayText;
      TxCatLedLabel.Name = "TxCatLedLabel";
      TxCatLedLabel.Size = new Size(21, 30);
      TxCatLedLabel.Text = "n";
      TxCatLedLabel.Click += TxCatLabel_Click;
      TxCatLedLabel.MouseEnter += StatusLabel_MouseEnter;
      TxCatLedLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // TxCatStatusLabel
      // 
      TxCatStatusLabel.Font = new Font("Segoe UI", 10F);
      TxCatStatusLabel.Name = "TxCatStatusLabel";
      TxCatStatusLabel.Size = new Size(52, 30);
      TxCatStatusLabel.Text = "TX CAT";
      TxCatStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      TxCatStatusLabel.Click += TxCatLabel_Click;
      TxCatStatusLabel.MouseEnter += StatusLabel_MouseEnter;
      TxCatStatusLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // IqOutputLedLabel
      // 
      IqOutputLedLabel.Font = new Font("Webdings", 9F);
      IqOutputLedLabel.ForeColor = SystemColors.GrayText;
      IqOutputLedLabel.Name = "IqOutputLedLabel";
      IqOutputLedLabel.Size = new Size(21, 30);
      IqOutputLedLabel.Text = "n";
      IqOutputLedLabel.Visible = false;
      // 
      // IqOutputStatusLabel
      // 
      IqOutputStatusLabel.Font = new Font("Segoe UI", 10F);
      IqOutputStatusLabel.Name = "IqOutputStatusLabel";
      IqOutputStatusLabel.Size = new Size(78, 30);
      IqOutputStatusLabel.Text = "I/Q 出力";
      IqOutputStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      IqOutputStatusLabel.Visible = false;
      // 
      // RotatorLedLabel
      // 
      RotatorLedLabel.Font = new Font("Webdings", 9F);
      RotatorLedLabel.ForeColor = SystemColors.GrayText;
      RotatorLedLabel.Name = "RotatorLedLabel";
      RotatorLedLabel.Size = new Size(21, 30);
      RotatorLedLabel.Text = "n";
      RotatorLedLabel.Click += RotLedLabel_Click;
      RotatorLedLabel.MouseEnter += StatusLabel_MouseEnter;
      RotatorLedLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // RotatorStatusLabel
      // 
      RotatorStatusLabel.Font = new Font("Segoe UI", 10F);
      RotatorStatusLabel.Name = "RotatorStatusLabel";
      RotatorStatusLabel.Size = new Size(55, 30);
      RotatorStatusLabel.Text = "ローテータ";
      RotatorStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      RotatorStatusLabel.Click += RotLedLabel_Click;
      RotatorStatusLabel.MouseEnter += StatusLabel_MouseEnter;
      RotatorStatusLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // RotatorDropdownBtn
      // 
      RotatorDropdownBtn.AutoSize = false;
      RotatorDropdownBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
      RotatorDropdownBtn.DropDownItems.AddRange(new ToolStripItem[] { EnableRotatorMNU, TrackRotatorMNU });
      RotatorDropdownBtn.Font = new Font("Segoe UI", 9F);
      RotatorDropdownBtn.ImageTransparentColor = Color.Magenta;
      RotatorDropdownBtn.Name = "RotatorDropdownBtn";
      RotatorDropdownBtn.Size = new Size(13, 33);
      RotatorDropdownBtn.Text = "toolStripDropDownButton1";
      // 
      // EnableRotatorMNU
      // 
      EnableRotatorMNU.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
      EnableRotatorMNU.Name = "EnableRotatorMNU";
      EnableRotatorMNU.Size = new Size(110, 22);
      EnableRotatorMNU.Text = "有効";
      EnableRotatorMNU.Click += RotLedLabel_Click;
      // 
      // TrackRotatorMNU
      // 
      TrackRotatorMNU.Name = "TrackRotatorMNU";
      TrackRotatorMNU.Size = new Size(110, 22);
      TrackRotatorMNU.Text = "追跡";
      TrackRotatorMNU.Click += RotatorTrackMNU_CheckedChanged;
      // 
      // NoiseFloorLabel
      // 
      NoiseFloorLabel.Name = "NoiseFloorLabel";
      NoiseFloorLabel.Size = new Size(119, 30);
      NoiseFloorLabel.Text = "ノイズフロア: -100 dB  ";
      NoiseFloorLabel.TextAlign = ContentAlignment.MiddleLeft;
      NoiseFloorLabel.Visible = false;
      // 
      // CpuLoadlabel
      // 
      CpuLoadlabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
      CpuLoadlabel.Name = "CpuLoadlabel";
      CpuLoadlabel.Size = new Size(95, 30);
      CpuLoadlabel.Text = "CPU負荷: 00.0%";
      CpuLoadlabel.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // UpdateLabel
      // 
      UpdateLabel.BackColor = Color.Lime;
      UpdateLabel.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
      UpdateLabel.ForeColor = Color.Blue;
      UpdateLabel.Margin = new Padding(10, 6, 10, 4);
      UpdateLabel.Name = "UpdateLabel";
      UpdateLabel.Padding = new Padding(10, 0, 10, 0);
      UpdateLabel.Size = new Size(81, 25);
      UpdateLabel.Text = "ダウンロード";
      UpdateLabel.Visible = false;
      UpdateLabel.Click += UpdateLabel_Click;
      UpdateLabel.MouseEnter += StatusLabel_MouseEnter;
      UpdateLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // toolTip1
      // 
      toolTip1.OwnerDraw = true;
      // 
      // panel4
      // 
      panel4.Dock = DockStyle.Left;
      panel4.Location = new Point(0, 102);
      panel4.Name = "panel4";
      panel4.Size = new Size(4, 824);
      panel4.TabIndex = 7;
      // 
      // MainForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(1834, 961);
      Controls.Add(panel4);
      Controls.Add(DockHost);
      Controls.Add(Toolbar);
      Controls.Add(menuStrip1);
      Controls.Add(StatusStrip);
      Icon = (Icon)resources.GetObject("$this.Icon");
      MainMenuStrip = menuStrip1;
      Name = "MainForm";
      Text = "Form1";
      FormClosing += MainForm_FormClosing;
      Load += MainForm_Load;
      Toolbar.ResumeLayout(false);
      panel3.ResumeLayout(false);
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
    private MenuStrip menuStrip1;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem ExitMNU;
    private ToolStripMenuItem GroupViewPanelMNU;
    private ToolStripMenuItem toolsToolStripMenuItem;
    private ToolStripMenuItem SdrDevicesMNU;
    private ToolStripMenuItem SettingsMNU;
    private ToolStripMenuItem ThemeMNU;
    private ToolStripMenuItem ThemeSystemMNU;
    private ToolStripMenuItem ThemeLightMNU;
    private ToolStripMenuItem ThemeDarkMNU;
    private ToolStripMenuItem helpToolStripMenuItem;
    private ToolStripMenuItem OnlineHelpMNU;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem AboutMNU;
    private Panel ClockPanel;
    private VE3NEA.Clock.ClockWidget Clock;
    private ToolStripMenuItem SatelliteGroupsMNU;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem DownloadSatDataMNU;
    private ToolStripMenuItem DataFolderMNU;
    public ToolStripMenuItem WaterfallMNU;
    public ToolStripMenuItem GroupViewMNU;
    public ToolStripMenuItem SatelliteDetailsMNU;
    public ToolStripMenuItem SatellitePassesMNU;
    private System.Windows.Forms.Timer timer;
    public ToolStripMenuItem TimelineMNU;
    public ToolStripMenuItem SkyViewMNU;
    public ToolStripMenuItem EarthViewMNU;
    private ToolStripMenuItem SupportGroupMNU;
    private StatusStrip StatusStrip;
    private ToolStripStatusLabel toolStripStatusLabel2;
    private ToolStripStatusLabel SdrLedLabel;
    private ToolStripStatusLabel SdrStatusLabel;
    private ToolStripStatusLabel SoundcardLedLabel;
    private ToolStripStatusLabel SoundcardStatusLabel;
    private ToolStripStatusLabel VacLedLabel;
    private ToolStripStatusLabel VacStatusLabel;
    private ToolStripStatusLabel TxCatLedLabel;
    private ToolStripStatusLabel TxCatStatusLabel;
    private ToolStripStatusLabel SatDataLedLabel;
    private ToolStripStatusLabel SatDataStatusLabel;
    private ToolStripStatusLabel IqOutputLedLabel;
    private ToolStripStatusLabel IqOutputStatusLabel;
    private ToolStripStatusLabel NoiseFloorLabel;
    private ToolStripStatusLabel CpuLoadlabel;
    private VE3NEA.ToolTipEx toolTip1;
    private ToolStripMenuItem DownloadTleMNU;
    public ToolStripMenuItem TransmittersMNU;
    public SatelliteSelectorWidget SatelliteSelecionWidget;
    private Panel panel1;
    private Panel panel2;
    private ToolStripDropDownButton SoundcardDropdownBtn;
    private Panel panel3;
    private GainWidget GainWidget;
    private ToolStripStatusLabel RxCatLedLabel;
    private ToolStripStatusLabel RxCatStatusLabel;
    private RotatorWidget RotatorWidget;
    public SatellitePhotoWidget SatellitePhotoWidget;
    private Panel panel5;
    private Panel panel4;
    private ToolStripStatusLabel RotatorLedLabel;
    private ToolStripStatusLabel RotatorStatusLabel;
    private Panel panel7;
    private Panel panel6;
    private ToolStripMenuItem LoadTleMNU;
    private ToolStripStatusLabel UpdateLabel;
    private ToolStripMenuItem DownloadAmsatMNU;
    private ToolStripSeparator toolStripMenuItem3;
    private ToolStripSeparator toolStripMenuItem4;
    private ToolStripMenuItem ResetWindowLayoutMNU;
    public ToolStripMenuItem QsoEntryMNU;
    private ToolStripDropDownButton RotatorDropdownBtn;
    private ToolStripMenuItem EnableRotatorMNU;
    private ToolStripMenuItem TrackRotatorMNU;
    public ToolStripMenuItem Ft4ConsoleMNU;
    public ToolStripMenuItem RecorderMNU;
    public ToolStripMenuItem AutoSelectionMNU;
    public ToolStripMenuItem QsoSchedulerMNU;
    public ToolStripMenuItem TelemetryMNU;
    public FrequencyWidget FrequencyWidget;
    private Panel SatellitePhotoSeparator;
  }
}
