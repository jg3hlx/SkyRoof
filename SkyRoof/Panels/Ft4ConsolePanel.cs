using System.Windows.Forms;
using Serilog;
using VE3NEA;
using WeifenLuo.WinFormsUI.Docking;
using static SkyRoof.Ft4MessageListWidget;

namespace SkyRoof
{
  public partial class Ft4ConsolePanel : DockContent
  {
    private readonly Context ctx;
    private InputSoundcard<float> Soundcard = new();
    public Ft4Decoder Ft4Decoder = new();
    public Ft4Sender Ft4Sender = new();
    public WsjtxUdpSender WsjtxUdpSender;
    public int TxCountdown;
    private string CurrentMessage = "CQ VE3NEA FN03";

    public Ft4ConsolePanel()
    {
      InitializeComponent();
    }

    public Ft4ConsolePanel(Context ctx)
    {
      InitializeComponent();

      this.ctx = ctx;
      Log.Information("Creating Ft4ConsolePanel");

      ctx.Ft4ConsolePanel = this;
      ctx.MainForm.Ft4ConsoleMNU.Checked = true;

      Ft4Decoder.SlotDecoded += Ft4Decoder_SlotDecoded;
      Soundcard.SamplesAvailable += (s, a) => AddSamplesFromSoundcard(a);
      Soundcard.Retry = true;

      AudioWaterfall.MouseDown += AudioWaterfall_MouseDown;

      AudioWaterfall.Ft4Decoder = Ft4Decoder;

      MessageListWidget.MessageHover += MessageListWidget_MessageHover;

      WsjtxUdpSender = new(ctx);
      WsjtxUdpSender.HighlightCallsignReceived += WsjtxUdpSender_HighlightCallsignReceived;

      Ft4Sender.BeforeTransmit += Ft4Sender_BeforeTransmit;
      Ft4Sender.AfterTransmit += Ft4Sender_AfterTransmit;

      ApplySettings();

      // ignore mousewheel on the spinners
      RxSpinner.MouseWheel += (o, e) => ((HandledMouseEventArgs)e).Handled = true;
      TxSpinner.MouseWheel += (o, e) => ((HandledMouseEventArgs)e).Handled = true;
    }

    private void Ft4ConsolePanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      Log.Information("Closing Ft4ConsolePanel");
      ctx.Ft4ConsolePanel = null;
      ctx.MainForm.Ft4ConsoleMNU.Checked = false;
      Soundcard?.Dispose();
      Soundcard = null;
      Ft4Decoder?.Dispose();
      Ft4Decoder = null;
      WsjtxUdpSender?.Dispose();
      WsjtxUdpSender = null;

      ctx.Settings.Ft4Console.SplitterDistance = SplitContainer.SplitterDistance;

      Ft4Sender?.Dispose();
      Ft4Sender = null;
    }

    public void ApplySettings()
    {
      var sett = ctx.Settings.Ft4Console;

      Soundcard.Enabled = false;
      Soundcard.SetDeviceId(sett.RxSoundcard);
      Soundcard.Enabled = sett.AudioSource == Ft4AudioSource.Soundcard;

      AudioWaterfall.Brightness = sett.Waterfall.Brightness;
      AudioWaterfall.Contrast = sett.Waterfall.Contrast;
      AudioWaterfall.Bandwidth = sett.Waterfall.Bandwidth;
      AudioWaterfall.SetBandwidth(); // {!} do not call in design mode

      MessageListWidget.ApplySettings(ctx.Settings.Ft4Console.Messages);
      foreach (DecodedItem item in MessageListWidget.listBox.Items)
        item.SetColors(ctx.Settings.Ft4Console.Messages);
      MessageListWidget.listBox.Refresh();

      WsjtxUdpSender.Port = ctx.Settings.Ft4Console.UdpSender.Port;
      WsjtxUdpSender.Host = ctx.Settings.Ft4Console.UdpSender.Host;
      WsjtxUdpSender.SetEnabled(ctx.Settings.Ft4Console.UdpSender.Enabled);

      Ft4Decoder.MyCall = ctx.Settings.User.Call;
      Ft4Decoder.CutoffFrequency = sett.Waterfall.Bandwidth - 100;

      Ft4Sender.Soundcard.SetDeviceId(sett.TxSoundcard);
      Ft4Sender.Soundcard.Volume = Dsp.FromDb2(sett.TxGain);
      SetButtonColors();
    }

    public void AddSamplesFromSdr(DataEventArgs<float> e)
    {
      if (ctx.Settings.Ft4Console.AudioSource == Ft4AudioSource.SDR)
      {
        Ft4Decoder.StartProcessing(e);
        if (AudioWaterfall.CanProcess) AudioWaterfall.SpectrumAnalyzer.StartProcessing(e);
      }
    }

    public void AddSamplesFromSoundcard(DataEventArgs<float> e)
    {
      if (ctx.Settings.Ft4Console.AudioSource == Ft4AudioSource.Soundcard)
      {
        Ft4Decoder?.StartProcessing(e);
        if (AudioWaterfall.CanProcess) AudioWaterfall.SpectrumAnalyzer?.StartProcessing(e);
      }
    }

    private void Ft4Decoder_SlotDecoded(object? sender, DataEventArgs<string> e)
    {
      MessageListWidget.BeginInvoke(() =>
      {
        MessageListWidget.BeginUpdateItems();

        MessageListWidget.AddSeparator(Ft4Decoder.DecodedSlotNumber,
          ctx.SatelliteSelector.SelectedSatellite.name, ctx.FrequencyControl.GetBandName(false));

        if (e.Data.Length >= 0)
        {
          double frequency = ctx.FrequencyControl.RadioLink.CorrectedDownlinkFrequency;
          string satellite = ctx.SatelliteSelector.SelectedSatellite.name;

          // to listbox
          var messages = e.Data.Select(s => MakeDecodedItem(s, e.Utc)).ToList();
          MessageListWidget.AddItems(messages);

          // to udp sender
          WsjtxUdpSender.SendDecodedMessages(messages, frequency);

          // to file
          SaveToFile(messages, frequency, satellite);
        }

        MessageListWidget.EndUpdateItems();
      });
    }

    private void SaveToFile(IEnumerable<DecodedItem> messages, double frequency, string satellite)
    {
      //if (!ctx.Settings.Ft4Console.ArchiveToFile) return;

      string archiveFolder = Path.Combine(Utils.GetUserDataFolder(), "FT4");
      Directory.CreateDirectory(archiveFolder);
      string filePath = Path.Combine(archiveFolder, DateTime.Today.ToString("yyyy") + ".txt");

      string text = string.Join('\n', messages.Select(msg => msg.ToArchiveString(frequency, satellite)).ToArray());

      using (StreamWriter writer = File.AppendText(filePath)) writer.WriteLine(text);

    }

    private DecodedItem MakeDecodedItem(string s, DateTime utc)
    {
      DecodedItem item = new();
      item.ParseMessage(s, utc);

      item.Type = DecodedItemType.RxMessage;
      item.Utc = utc;
      item.SlotNumber = Ft4Decoder.DecodedSlotNumber;
      item.ToMe = item.Parse.DXCallsign == ctx.Settings.User.Call;
      item.FromMe = item.Parse.DECallsign == ctx.Settings.User.Call;
      item.SetColors(ctx.Settings.Ft4Console.Messages);

      // callsign color from logger if not receiving wsjtx colors
      if (!WsjtxUdpSender.Active)
        item.SetCallsignColors(ctx.LoggerInterface);

      return item;
    }

    private void WsjtxUdpSender_HighlightCallsignReceived(object? sender, HighlightCallsignEventArgs e)
    {
      MessageListWidget.HighlightCallsign(e);
    }

    private void AudioWaterfall_MouseDown(object? sender, MouseEventArgs e)
    {
      AudioWaterfall.SetFrequenciesFromMouseClick(e);
      RxSpinner.Value = AudioWaterfall.RxAudioFrequency;
      TxSpinner.Value = AudioWaterfall.TxAudioFrequency;
    }

    private void AudioWaterfall_MouseMove(object sender, MouseEventArgs e)
    {
      AudioWaterfall.ShowLeftBarTooltip(e.Location);

      if (e.X >= AudioWaterfallWidget.LEFT_BAR_WIDTH && e.Y > AudioWaterfallWidget.TOP_BAR_HEIGHT)
      {
        (int slotNumber, int audioFreq) = AudioWaterfall.GetSlotAndFreq(e.Location);
        var item = MessageListWidget.FindItem(slotNumber, audioFreq);
        AudioWaterfall.ShowCallsign(item);
      }
    }

    private void MessageListWidget_MessageHover(object? sender, Ft4MessageEventArgs? e)
    {
      AudioWaterfall.ShowCallsign(MessageListWidget.HotItem);
    }

    private void EnableTxBtn_MouseDown(object sender, MouseEventArgs e)
    {
      if (!CheckTxEnabled()) return;

      if (Ft4Sender.SenderMode == Ft4Sender.Mode.Sending && TxCountdown > 0)
        TxCountdown = 0;
      else
      {
        TxCountdown = 20;  // stop after 20 transmissions
        Ft4Sender.StartSending();
      }

      SetButtonColors();
    }

    private void HaltTxBtn_MouseDown(object sender, MouseEventArgs e)
    {
      Ft4Sender.Stop();
      SetButtonColors();
    }

    private void TuneBtn_MouseDown(object sender, MouseEventArgs e)
    {
      if (!CheckTxEnabled()) return;

      if (Ft4Sender.SenderMode == Ft4Sender.Mode.Tuning)
        Ft4Sender.Stop();
      else
        Ft4Sender.StartTuning();

      SetButtonColors();
    }

    private void SetButtonColors()
    {
      bool tuning = Ft4Sender.SenderMode == Ft4Sender.Mode.Tuning;
      bool sendEnabled = Ft4Sender.SenderMode == Ft4Sender.Mode.Sending && TxCountdown > 0;
      //bool sending = Ft4Sender.stage == Ft4Sender.SendStage.Sending;


      EnableTxBtn.BackColor = sendEnabled ? Color.LightCoral : Color.Transparent;
      //HaltTxBtn.BackColor = sending ? Color.Red : Color.Transparent;
      TuneBtn.BackColor = tuning ? Color.LightCoral : Color.Transparent;

      TuneBtn.Refresh();
      EnableTxBtn.Refresh();
      HaltTxBtn.Refresh();
    }

    private bool CheckTxEnabled()
    {
      if (ctx.Settings.Ft4Console.EnableTransmit) return true;

      MessageBox.Show("FT4 transmit is not enabled in Settings.", "SkyRoof", MessageBoxButtons.OK, MessageBoxIcon.Information);
      return false;
    }

    private void Ft4Sender_BeforeTransmit(object? sender, EventArgs e)
    {
      ft4TimeBar1.Transmitting = Ft4Sender.SenderMode == Ft4Sender.Mode.Sending;
      TxCountdown--;
      BeginInvoke(SetButtonColors);
      Console.Beep();
    }

    private void Ft4Sender_AfterTransmit(object? sender, EventArgs e)
    {
      ft4TimeBar1.Transmitting = false;
      BeginInvoke(() =>
      {
        if (TxCountdown <= 0) Ft4Sender.Stop();
        SetButtonColors();
      });
      Console.Beep();
    }

    private void Spinner_ValueChanged(object sender, EventArgs e)
    {
      if (RxSpinner.Value != Ft4Decoder.RxAudioFrequency)
      {
        AudioWaterfall.RxAudioFrequency = Ft4Decoder.RxAudioFrequency = (int)RxSpinner.Value;
      }


      if (TxSpinner.Value != Ft4Sender.TxAudioFrequency)
      {
        AudioWaterfall.TxAudioFrequency = Ft4Sender.TxAudioFrequency = (int)TxSpinner.Value;
        Ft4Sender.SetMessage(CurrentMessage);
      }
    }

    private void TxSpinner_ValueChanged(object sender, EventArgs e)
    {
      if (TxSpinner.Value != Ft4Sender.TxAudioFrequency)
      {
        AudioWaterfall.TxAudioFrequency = Ft4Sender.TxAudioFrequency = (int)TxSpinner.Value;
        Ft4Sender.SetMessage(CurrentMessage);
      }
    }

    private void RxSpinner_ValueChanged(object sender, EventArgs e)
    {
      if (RxSpinner.Value != Ft4Decoder.RxAudioFrequency)
        AudioWaterfall.RxAudioFrequency = Ft4Decoder.RxAudioFrequency = (int)RxSpinner.Value;
    }

    private void TxToRxBtn_Click(object sender, EventArgs e)
    {
      RxSpinner.Value = TxSpinner.Value;
    }

    private void RxToTxBtn_Click(object sender, EventArgs e)
    {
      TxSpinner.Value = RxSpinner.Value;
    }

    private void Ft4ConsolePanel_Shown(object sender, EventArgs e)
    {
      SplitContainer.SplitterDistance = ctx.Settings.Ft4Console.SplitterDistance;
    }
  }
}
