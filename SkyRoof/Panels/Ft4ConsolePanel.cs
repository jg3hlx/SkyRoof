using Newtonsoft.Json.Linq;
using Serilog;
using VE3NEA;
using WeifenLuo.WinFormsUI.Docking;
using WsjtxUtils.WsjtxMessages.Messages;
using static SkyRoof.DecodedItem;
using static SkyRoof.Ft4MessageListWidget;

namespace SkyRoof
{
  public partial class Ft4ConsolePanel : DockContent
  {
    private readonly Context ctx;
    private InputSoundcard<float> Soundcard = new();
    public Ft4Decoder Decoder = new();
    public Ft4Sender Sender = new();
    private Ft4QsoSequencer Sequencer;
    public WsjtxUdpSender WsjtxUdpSender;
    public int TxCountdown;

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

      Decoder.Decode(); // prime the decoding engine
      Decoder.SlotDecoded += Ft4Decoder_SlotDecoded;
      Soundcard.SamplesAvailable += (s, a) => AddSamplesFromSoundcard(a);
      Soundcard.Retry = true;

      AudioWaterfall.Ft4Decoder = Decoder;
      AudioWaterfall.MouseDown += AudioWaterfall_MouseDown;

      MessageListWidget.MessageHover += MessageListWidget_MessageHover;

      WsjtxUdpSender = new(ctx);
      WsjtxUdpSender.HighlightCallsignReceived += WsjtxUdpSender_HighlightCallsignReceived;

      Sender.BeforeTransmit += Ft4Sender_BeforeTransmit;
      Sender.AfterTransmit += Ft4Sender_AfterTransmit;

      Sequencer = new(ctx.Settings.User.Call, ctx.Settings.User.Square);

      ApplySettings();

      // disable mousewheel on the spinners
      RxSpinner.MouseWheel += (o, e) => ((HandledMouseEventArgs)e).Handled = true;
      TxSpinner.MouseWheel += (o, e) => ((HandledMouseEventArgs)e).Handled = true;
    }

    private void Ft4ConsolePanel_Shown(object sender, EventArgs e)
    {
      SplitContainer.SplitterDistance = ctx.Settings.Ft4Console.SplitterDistance;
    }

    private void Ft4ConsolePanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      Log.Information("Closing Ft4ConsolePanel");
      ctx.Ft4ConsolePanel = null;
      ctx.MainForm.Ft4ConsoleMNU.Checked = false;
      Soundcard?.Dispose();
      Soundcard = null;
      Decoder?.Dispose();
      Decoder = null;
      WsjtxUdpSender?.Dispose();
      WsjtxUdpSender = null;

      ctx.Settings.Ft4Console.SplitterDistance = SplitContainer.SplitterDistance;

      Sender?.Dispose();
      Sender = null;
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

      MessageListWidget.ApplySettings(ctx.Settings);
      foreach (DecodedItem item in MessageListWidget.listBox.Items)
        item.SetColors(ctx.Settings.Ft4Console.Messages);
      MessageListWidget.listBox.Refresh();

      WsjtxUdpSender.Port = ctx.Settings.Ft4Console.UdpSender.Port;
      WsjtxUdpSender.Host = ctx.Settings.Ft4Console.UdpSender.Host;
      WsjtxUdpSender.SetEnabled(ctx.Settings.Ft4Console.UdpSender.Enabled);

      Decoder.MyCall = ctx.Settings.User.Call;
      Decoder.CutoffFrequency = sett.Waterfall.Bandwidth - 50;

      Sender.Soundcard.SetDeviceId(sett.TxSoundcard);
      Sender.Soundcard.Volume = Dsp.FromDb2(sett.TxGain);

      Sequencer.MyCall = ctx.Settings.User.Call;
      Sequencer.MySquare = ctx.Settings.User.Square;
      TxMessageLabel.Text = $"{Sequencer.Message}";
      Sender.SetMessage(Sequencer.Message!);
      UpdateMessageButtons();

      UpdateTxButtons();
    }

    public void AddSamplesFromSdr(DataEventArgs<float> e)
    {
      if (ctx.Settings.Ft4Console.AudioSource == Ft4AudioSource.SDR)
      {
        Decoder.StartProcessing(e);
        if (AudioWaterfall.CanProcess) AudioWaterfall.SpectrumAnalyzer.StartProcessing(e);
      }
    }

    public void AddSamplesFromSoundcard(DataEventArgs<float> e)
    {
      if (ctx.Settings.Ft4Console.AudioSource == Ft4AudioSource.Soundcard)
      {
        Decoder?.StartProcessing(e);
        if (AudioWaterfall.CanProcess) AudioWaterfall.SpectrumAnalyzer?.StartProcessing(e);
      }
    }

    private void Ft4Decoder_SlotDecoded(object? sender, DataEventArgs<string> e)
    {
      MessageListWidget.BeginInvoke(() =>
      {
        MessageListWidget.BeginUpdateItems();

        MessageListWidget.AddSeparator(Decoder.DecodedSlotNumber,
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
      item.SlotNumber = Decoder.DecodedSlotNumber;
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

      if (Sender.Mode == Ft4Sender.SenderMode.Sending && TxCountdown > 0)
      {
        TxCountdown = 0;
        if (Sender.SenderPhase == Ft4Sender.SendingStage.Idle) Sender.Stop();
      }
      else
      {
        Sender.StartSending();
        if (Sender.Mode == Ft4Sender.SenderMode.Sending) // no sending if was tuning 
          TxCountdown = ctx.Settings.Ft4Console.TxWatchDog * 4;
      }

      UpdateTxButtons();
    }

    private void HaltTxBtn_MouseDown(object sender, MouseEventArgs e)
    {
      Sender.Stop();
      UpdateTxButtons();
    }

    private void TuneBtn_MouseDown(object sender, MouseEventArgs e)
    {
      if (!CheckTxEnabled()) return;

      if (Sender.Mode == Ft4Sender.SenderMode.Tuning)
        Sender.Stop();
      else
        Sender.StartTuning();

      UpdateTxButtons();
    }

    private void UpdateTxButtons()
    {
      bool tuning = Sender.Mode == Ft4Sender.SenderMode.Tuning;
      bool sendEnabled = Sender.Mode == Ft4Sender.SenderMode.Sending && TxCountdown > 0;
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
      Ft4TimeBar1.Transmitting = Sender.Mode == Ft4Sender.SenderMode.Sending;
      BeginInvoke(() =>
      {
        AddTxMessageToList();
        UpdateTxButtons();
      });
      Console.Beep();
    }

    private void Ft4Sender_AfterTransmit(object? sender, EventArgs e)
    {
      Ft4TimeBar1.Transmitting = false;
      BeginInvoke(() =>
      {
        TxCountdown--;
        if (TxCountdown <= 0) Sender.Stop();
        UpdateTxButtons();
      });
      Console.Beep();
    }

    private void TxSpinner_ValueChanged(object sender, EventArgs e)
    {
      if (TxSpinner.Value != Sender.TxAudioFrequency)
      {
        AudioWaterfall.TxAudioFrequency = Sender.TxAudioFrequency = (int)TxSpinner.Value;
        Sender.SetMessage(Sequencer.Message!);
      }
    }

    private void RxSpinner_ValueChanged(object sender, EventArgs e)
    {
      if (RxSpinner.Value != Decoder.RxAudioFrequency)
        AudioWaterfall.RxAudioFrequency = Decoder.RxAudioFrequency = (int)RxSpinner.Value;
    }

    private void TxToRxBtn_Click(object sender, EventArgs e)
    {
      RxSpinner.Value = TxSpinner.Value;
    }

    private void RxToTxBtn_Click(object sender, EventArgs e)
    {
      TxSpinner.Value = RxSpinner.Value;
    }

    private void OddRadioBtn_CheckedChanged(object sender, EventArgs e)
    {
      Sender.TxOdd = OddRadioBtn.Checked;
      OddEvenGroupBox.Text = Sender.TxOdd ? "TX Odd" : "TX Even";
    }

    private Ft4MessageType GetButtonMessage(object btn)
    {
      return (Ft4MessageType)int.Parse((string)((Button)btn).Tag!);
    }

    private void MessageBtn_Click(object sender, EventArgs e)
    {
      var msg = GetButtonMessage(sender);
      Sequencer.ForceMessage(msg);
      TxMessageLabel.Text = $"{Sequencer.Message}";
      Sender.SetMessage(Sequencer.Message!);
    }

    public void UpdateMessageButtons()
    {
      foreach (Button btn in TxMessagesPanel.Controls)
      {
        var msg = GetButtonMessage(btn);
        btn.Enabled = Sequencer.IsMessageAvailable(msg);
        btn.BackColor = msg == Sequencer.MessageType ? Color.LightCoral : SystemColors.Control;
      }

      TxMessageLabel.Text = Sequencer.Message;
    }

    private void MessageListWidget_MessageClick(object sender, Ft4MessageEventArgs e)
    {
      Sequencer.ReplyToMessage(e.Item);
      Sender.SetMessage(Sequencer.Message!);
    }


    private void AddTxMessageToList()
    {
      Sender.Slot.Utc = DateTime.UtcNow;

      var item = new DecodedItem();
      item.Type = DecodedItemType.TxMessage;
      item.Utc = DateTime.UtcNow;
      item.SlotNumber = Sender.Slot.SlotNumber;

      item.Tokens = [
        new DisplayToken(" TX "),
        new DisplayToken($"{Sender.Slot.SecondsIntoSlot,4:F1}"),
        new DisplayToken($"{Sender.TxAudioFrequency,4:D}")
      ];
      item.Tokens.AddRange(DecodedItem.SplitWords(Sequencer.Message ?? ""));
      foreach (var token in item.Tokens) token.fgBrush = Brushes.White;

      MessageListWidget.AddSeparator(Sender.Slot.SlotNumber,
          ctx.SatelliteSelector.SelectedSatellite.name, ctx.FrequencyControl.GetBandName(false));

      MessageListWidget.AddSentMessage(item);
    }
  }
}
