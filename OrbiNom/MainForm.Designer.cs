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
      Toolbar = new Panel();
      SatelliteSelector = new SatelliteSelector();
      ClockPanel = new Panel();
      Clock = new VE3NEA.Clock.Clock();
      StatusBar = new StatusStrip();
      DockHost = new WeifenLuo.WinFormsUI.Docking.DockPanel();
      vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
      menuStrip1 = new MenuStrip();
      fileToolStripMenuItem = new ToolStripMenuItem();
      ExitMNU = new ToolStripMenuItem();
      GroupViewPanelMNU = new ToolStripMenuItem();
      GroupViewMNU = new ToolStripMenuItem();
      SatelliteDetailsMNU = new ToolStripMenuItem();
      WaterfallMNU = new ToolStripMenuItem();
      toolsToolStripMenuItem = new ToolStripMenuItem();
      SatelliteGroupsMNU = new ToolStripMenuItem();
      SdrDevicesMNU = new ToolStripMenuItem();
      SettingsMNU = new ToolStripMenuItem();
      toolStripMenuItem1 = new ToolStripSeparator();
      UpdateSatelliteListMNU = new ToolStripMenuItem();
      helpToolStripMenuItem = new ToolStripMenuItem();
      OnlineHelpMNU = new ToolStripMenuItem();
      DataFolderMNU = new ToolStripMenuItem();
      toolStripMenuItem2 = new ToolStripSeparator();
      AboutMNU = new ToolStripMenuItem();
      Toolbar.SuspendLayout();
      ClockPanel.SuspendLayout();
      menuStrip1.SuspendLayout();
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
      // StatusBar
      // 
      StatusBar.Location = new Point(0, 688);
      StatusBar.Name = "StatusBar";
      StatusBar.Size = new Size(1200, 22);
      StatusBar.TabIndex = 1;
      StatusBar.Text = "statusStrip1";
      // 
      // DockHost
      // 
      DockHost.Dock = DockStyle.Fill;
      DockHost.DockBackColor = Color.FromArgb(238, 238, 242);
      DockHost.Location = new Point(0, 64);
      DockHost.Name = "DockHost";
      DockHost.Padding = new Padding(6);
      DockHost.ShowAutoHideContentOnHover = false;
      DockHost.Size = new Size(1200, 624);
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
      GroupViewPanelMNU.DropDownItems.AddRange(new ToolStripItem[] { GroupViewMNU, SatelliteDetailsMNU, WaterfallMNU });
      GroupViewPanelMNU.Name = "GroupViewPanelMNU";
      GroupViewPanelMNU.Size = new Size(44, 20);
      GroupViewPanelMNU.Text = "View";
      // 
      // GroupViewMNU
      // 
      GroupViewMNU.Name = "GroupViewMNU";
      GroupViewMNU.Size = new Size(153, 22);
      GroupViewMNU.Text = "Group";
      GroupViewMNU.Click += GroupViewMNU_Click;
      // 
      // SatelliteDetailsMNU
      // 
      SatelliteDetailsMNU.Name = "SatelliteDetailsMNU";
      SatelliteDetailsMNU.Size = new Size(153, 22);
      SatelliteDetailsMNU.Text = "Satellite Details";
      SatelliteDetailsMNU.Click += SatelliteDetailsMNU_Click;
      // 
      // WaterfallMNU
      // 
      WaterfallMNU.Name = "WaterfallMNU";
      WaterfallMNU.Size = new Size(153, 22);
      WaterfallMNU.Text = "Waterfall";
      // 
      // toolsToolStripMenuItem
      // 
      toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { SatelliteGroupsMNU, SdrDevicesMNU, SettingsMNU, toolStripMenuItem1, UpdateSatelliteListMNU });
      toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      toolsToolStripMenuItem.Size = new Size(46, 20);
      toolsToolStripMenuItem.Text = "Tools";
      // 
      // SatelliteGroupsMNU
      // 
      SatelliteGroupsMNU.Name = "SatelliteGroupsMNU";
      SatelliteGroupsMNU.Size = new Size(193, 22);
      SatelliteGroupsMNU.Text = "Satellites and Groups...";
      SatelliteGroupsMNU.Click += EditGroupsMNU_Click;
      // 
      // SdrDevicesMNU
      // 
      SdrDevicesMNU.Name = "SdrDevicesMNU";
      SdrDevicesMNU.Size = new Size(193, 22);
      SdrDevicesMNU.Text = "SDR Devices...";
      // 
      // SettingsMNU
      // 
      SettingsMNU.Name = "SettingsMNU";
      SettingsMNU.Size = new Size(193, 22);
      SettingsMNU.Text = "Settings...";
      // 
      // toolStripMenuItem1
      // 
      toolStripMenuItem1.Name = "toolStripMenuItem1";
      toolStripMenuItem1.Size = new Size(190, 6);
      // 
      // UpdateSatelliteListMNU
      // 
      UpdateSatelliteListMNU.Name = "UpdateSatelliteListMNU";
      UpdateSatelliteListMNU.Size = new Size(193, 22);
      UpdateSatelliteListMNU.Text = "Update Satellite Data...";
      UpdateSatelliteListMNU.Click += UpdateSatelliteListMNU_Click;
      // 
      // helpToolStripMenuItem
      // 
      helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OnlineHelpMNU, DataFolderMNU, toolStripMenuItem2, AboutMNU });
      helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      helpToolStripMenuItem.Size = new Size(44, 20);
      helpToolStripMenuItem.Text = "Help";
      // 
      // OnlineHelpMNU
      // 
      OnlineHelpMNU.Name = "OnlineHelpMNU";
      OnlineHelpMNU.Size = new Size(146, 22);
      OnlineHelpMNU.Text = "Online Help...";
      // 
      // DataFolderMNU
      // 
      DataFolderMNU.Name = "DataFolderMNU";
      DataFolderMNU.Size = new Size(146, 22);
      DataFolderMNU.Text = "Data Folder...";
      DataFolderMNU.Click += DataFolderMNU_Click;
      // 
      // toolStripMenuItem2
      // 
      toolStripMenuItem2.Name = "toolStripMenuItem2";
      toolStripMenuItem2.Size = new Size(143, 6);
      // 
      // AboutMNU
      // 
      AboutMNU.Name = "AboutMNU";
      AboutMNU.Size = new Size(146, 22);
      AboutMNU.Text = "About...";
      // 
      // MainForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(1200, 710);
      Controls.Add(DockHost);
      Controls.Add(StatusBar);
      Controls.Add(Toolbar);
      Controls.Add(menuStrip1);
      MainMenuStrip = menuStrip1;
      Name = "MainForm";
      Text = "Form1";
      FormClosing += MainForm_FormClosing;
      Load += MainForm_Load;
      Toolbar.ResumeLayout(false);
      ClockPanel.ResumeLayout(false);
      menuStrip1.ResumeLayout(false);
      menuStrip1.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Panel Toolbar;
    private StatusStrip StatusBar;
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
    private ToolStripMenuItem UpdateSatelliteListMNU;
    private ToolStripMenuItem DataFolderMNU;
    public ToolStripMenuItem WaterfallMNU;
    public ToolStripMenuItem GroupViewMNU;
    public ToolStripMenuItem SatelliteDetailsMNU;
    private SatelliteSelector SatelliteSelector;
  }
}
