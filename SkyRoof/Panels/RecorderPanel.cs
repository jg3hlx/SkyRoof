using MathNet.Numerics;
using MultimediaTimer;
using NAudio.MediaFoundation;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Serilog;
using VE3NEA;
using WeifenLuo.WinFormsUI.Docking;

namespace SkyRoof
{
  public partial class RecorderPanel : DockContent
  {
    private readonly Context ctx = null!;
    private static readonly Color waveformBackgroundColor = Color.Black;
    private static readonly Color waveformForegroundColor = Color.Aqua;
    private static readonly Color waveformAxisColor = Color.Teal;
    private const int PLAYBACK_TIMER_INTERVAL_MS = 20;
    private const int PLAYBACK_UI_INTERVAL_MS = 100;
    private const int PLAYBACK_PREBUFFER_CHUNKS = 10;
    private const int PLAYBACK_SAMPLES_PER_TICK = SdrConst.AUDIO_SAMPLING_RATE * PLAYBACK_TIMER_INTERVAL_MS / 1000;
    private bool isRecordingAudio => iqBuffer == null;
    private bool isRecording = false;
    internal bool isPlayingBack { get; private set; }

    // Recording buffers
    private const int RECORDING_MINUTES = 30;
    private const int BUFFER_SIZE = SdrConst.AUDIO_SAMPLING_RATE * 60 * RECORDING_MINUTES;

    private float[]? audioBuffer;
    private Complex32[]? iqBuffer;
    private int samplesInBuffer = 0;
    private int playbackPosition = 0;
    private bool speakerEnabledForPlayback;
    private readonly object playbackLock = new();
    private DateTime recordingStartTime;
    private System.Windows.Forms.Timer? recordingTimer;
    private System.Windows.Forms.Timer? playbackUiTimer;
    private MultimediaTimer.Timer? playbackTimer;

    //----------------------------------------------------------------------------------------------
    //                                        startup
    //----------------------------------------------------------------------------------------------
    public RecorderPanel()
    {
      InitializeComponent();
    }

    public RecorderPanel(Context ctx)
    {
      InitializeComponent();
      Utils.SetDoubleBuffered(WaveformPanel, true);

      this.ctx = ctx;
      Log.Information("Creating RecorderPanel");

      ctx.RecorderPanel = this;
      ctx.MainForm.RecorderMNU.Checked = true;

      // Initialize recording timer
      recordingTimer = new System.Windows.Forms.Timer();
      recordingTimer.Interval = 1000;
      recordingTimer.Tick += RecordingTimer_Tick;

      playbackUiTimer = new System.Windows.Forms.Timer();
      playbackUiTimer.Interval = PLAYBACK_UI_INTERVAL_MS;
      playbackUiTimer.Tick += PlaybackUiTimer_Tick;

      playbackTimer = new MultimediaTimer.Timer();
      playbackTimer.Mode = TimerMode.Periodic;
      playbackTimer.Interval = TimeSpan.FromMilliseconds(PLAYBACK_TIMER_INTERVAL_MS);
      playbackTimer.Resolution = TimeSpan.FromMilliseconds(1);
      playbackTimer.Elapsed += PlaybackTimer_Elapsed;

      ApplySettings();
    }

    private void RecorderPanel_Shown(object sender, EventArgs e)
    {
      // Restore splitter positions or other UI state from settings if needed
      // SplitContainer.SplitterDistance = ctx.Settings.Recorder.SplitterDistance;
    }

    private void RecorderPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      Log.Information("Closing RecorderPanel");

      // Stop recording if active
      if (isRecording) StopRecording();
      if (isPlayingBack) StopPlayback();

      // Dispose timer
      recordingTimer?.Stop();
      recordingTimer?.Dispose();
      playbackUiTimer?.Stop();
      playbackUiTimer?.Dispose();
      if (playbackTimer != null)
      {
        playbackTimer.Stop();
        playbackTimer.Elapsed -= PlaybackTimer_Elapsed;
        playbackTimer = null;
      }

      ctx.RecorderPanel = null;
      ctx.MainForm.RecorderMNU.Checked = false;

      // Save UI state to settings if needed
      // ctx.Settings.Recorder.SplitterDistance = SplitContainer.SplitterDistance;

      // Dispose of any resources here
    }

    public void ApplySettings()
    {
      // Apply settings when the panel is created or settings are changed
      // var sett = ctx.Settings.Recorder;
    }




    //----------------------------------------------------------------------------------------------
    //                                        commands
    //----------------------------------------------------------------------------------------------
    private void RecordBtn_Click(object sender, EventArgs e)
    {
      if (isRecording) StopRecording();
      else RecordAudioMNU_Click(sender, e);
    }

    private void RecordMenuBtn_Click(object sender, EventArgs e)
    {
      RecordMenu.Show(RecordMenuBtn, new Point(0, RecordMenuBtn.Height));
    }

    private void RecordAudioMNU_Click(object sender, EventArgs e)
    {
      StartRecording(true);
    }

    private void RecordIqMNU_Click(object sender, EventArgs e)
    {
      StartRecording(false);
    }

    private void SaveBtn_Click(object sender, EventArgs e)
    {
      if (isRecordingAudio) SaveMp3MNU_Click(sender, e);
      else SaveWavMNU_Click(sender, e);
    }

    private void SaveMenuBtn_Click(object sender, EventArgs e)
    {
      SaveMp3MNU.Enabled = isRecordingAudio;
      SaveMenu.Show(SaveMenuBtn, new Point(0, SaveMenuBtn.Height));
    }

    private void SaveMp3MNU_Click(object sender, EventArgs e)
    {
      SaveRecordingWithDialog("mp3");
    }

    private void SaveWavMNU_Click(object sender, EventArgs e)
    {
      SaveRecordingWithDialog("wav");
    }

    private void LoadBtn_Click(object sender, EventArgs e)
    {
      using var dialog = new OpenFileDialog();
      dialog.Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3|All Files (*.*)|*.*";
      dialog.InitialDirectory = GetRecordingsPath();
      dialog.Title = "Load Recording";

      if (dialog.ShowDialog() == DialogResult.OK)
      {
        try
        {
          LoadRecording(dialog.FileName);
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          Log.Error(ex, "Error loading recording");
        }
      }
    }

    private void PlaybackBtn_Click(object sender, EventArgs e)
    {
      if (PlaybackBtn.BackColor == Color.LightGreen) StopPlayback();
      else StartPlayback();
    }




    //----------------------------------------------------------------------------------------------
    //                                        record
    //----------------------------------------------------------------------------------------------
    private void StartRecording(bool isAudio)
    {
      RecordBtn.BackColor = Color.LightCoral;

      if (isAudio)
      {
        audioBuffer = new float[BUFFER_SIZE];
        iqBuffer = null;
        StatusLabel.Text = "Recording Audio...";
        Log.Information("Starting audio recording");
      }
      else
      {
        iqBuffer = new Complex32[BUFFER_SIZE];
        audioBuffer = null;
        StatusLabel.Text = "Recording I/Q...";
        Log.Information("Starting I/Q recording");
      }

      samplesInBuffer = 0;
      playbackPosition = 0;
      recordingStartTime = DateTime.Now;
      StatusLabel.Text = $"Recording {(isRecordingAudio ? "Audio" : "I/Q")} 00:00";
      recordingTimer?.Start();

      // Disable controls during recording
      RecordMenuBtn.Enabled = false;
      SetSaveButtonsEnabled(false);
      LoadBtn.Enabled = false;
      PlaybackBtn.Enabled = false;
      toolTip1.SetToolTip(RecordBtn, "Stop Recording");

      isRecording = true;
      WaveformPanel.Invalidate();
    }

    private void StopRecording()
    {
      isRecording = false;

      RecordBtn.BackColor = Color.Transparent;

      recordingTimer?.Stop();

      TimeSpan duration = DateTime.Now - recordingStartTime;
      StatusLabel.Text = $"Recording stopped - Duration: {duration:mm\\:ss}";
      Log.Information($"Recording stopped - {samplesInBuffer} samples recorded");

      // Enable Save button if we have data
      SetSaveButtonsEnabled(samplesInBuffer > 0);

      // Re-enable controls
      RecordMenuBtn.Enabled = true;
      LoadBtn.Enabled = true;
      PlaybackBtn.Enabled = samplesInBuffer > 0;
      toolTip1.SetToolTip(RecordBtn, "Record Audio");

      WaveformPanel.Invalidate();
    }

    private void SetSaveButtonsEnabled(bool enabled)
    {
      SaveBtn.Enabled = enabled;
      SaveMenuBtn.Enabled = enabled;
    }




    //----------------------------------------------------------------------------------------------
    //                                        play back
    //----------------------------------------------------------------------------------------------
    private void StartPlayback()
    {
      if (samplesInBuffer <= 0) return;

      playbackTimer?.Stop();

      if (audioBuffer != null && !ctx.SpeakerSoundcard.Enabled)
      {
        ctx.SpeakerSoundcard.Enabled = true;
        speakerEnabledForPlayback = true;
      }

      if (playbackPosition >= samplesInBuffer) playbackPosition = 0;
      isPlayingBack = true;
      PlaybackBtn.BackColor = Color.LightGreen;
      toolTip1.SetToolTip(PlaybackBtn, "Stop Playback");
      UpdatePlaybackStatus();
      Log.Information("Starting playback");

      RecordBtn.Enabled = false;
      RecordMenuBtn.Enabled = false;
      LoadBtn.Enabled = false;
      SetSaveButtonsEnabled(false);

      PreparePlaybackOutputs();
      PrebufferPlayback();

      if (playbackPosition >= samplesInBuffer)
      {
        StopPlayback(true);
        return;
      }

      playbackUiTimer?.Start();
      playbackTimer?.Start();
    }

    private void StopPlayback(bool resetPosition = false)
    {
      isPlayingBack = false;
      playbackUiTimer?.Stop();
      playbackTimer?.Stop();
      if (resetPosition) playbackPosition = 0;

      if (speakerEnabledForPlayback)
      {
        ctx.SpeakerSoundcard.Enabled = false;
        speakerEnabledForPlayback = false;
      }

      PlaybackBtn.BackColor = Color.Transparent;
      toolTip1.SetToolTip(PlaybackBtn, "Start Playback");
      StatusLabel.Text = "Playback stopped";
      Log.Information("Playback stopped");

      RecordBtn.Enabled = true;
      RecordMenuBtn.Enabled = true;
      LoadBtn.Enabled = true;
      SetSaveButtonsEnabled(samplesInBuffer > 0);
      WaveformPanel.Invalidate();
    }

    private void PlaybackTimer_Elapsed(object? sender, EventArgs e)
    {
      if (!PumpPlaybackChunk())
      {
        BeginInvoke(() => StopPlayback(true));
        return;
      }
    }

    private void PlaybackUiTimer_Tick(object? sender, EventArgs e)
    {
      if (!isPlayingBack) return;

      UpdatePlaybackStatus();
      WaveformPanel.Invalidate();
    }

    private void PrebufferPlayback()
    {
      for (int i = 0; i < PLAYBACK_PREBUFFER_CHUNKS; i++)
        if (!PumpPlaybackChunk()) break;

      UpdatePlaybackStatus();
      WaveformPanel.Invalidate();
    }

    private bool PumpPlaybackChunk()
    {
      lock (playbackLock)
      {
        int samplesRemaining = samplesInBuffer - playbackPosition;
        if (samplesRemaining <= 0) return false;

        int count = Math.Min(PLAYBACK_SAMPLES_PER_TICK, samplesRemaining);
        if (audioBuffer != null)
          ctx.MainForm.RoutePlaybackAudio(audioBuffer, playbackPosition, count);
        else if (iqBuffer != null)
          ctx.MainForm.RoutePlaybackIq(iqBuffer, playbackPosition, count);

        playbackPosition += count;
        return playbackPosition < samplesInBuffer;
      }
    }

    private void SeekPlaybackToPosition(int position)
    {
      if (!isPlayingBack || samplesInBuffer <= 0) return;

      playbackTimer?.Stop();

      lock (playbackLock)
        playbackPosition = Math.Clamp(position, 0, Math.Max(0, samplesInBuffer - 1));

      PreparePlaybackOutputs();
      PrebufferPlayback();
      UpdatePlaybackStatus();
      WaveformPanel.Invalidate();

      if (playbackPosition >= samplesInBuffer) StopPlayback(true);
      else playbackTimer?.Start();
    }

    private void PreparePlaybackOutputs()
    {
      if (audioBuffer != null)
      {
        ctx.SpeakerSoundcard.Buffer.Clear();
        if (ctx.Settings.OutputStream.Type == DataStreamType.AudioToVac)
          ctx.AudioVacSoundcard.Buffer.Clear();
      }
      else if (iqBuffer != null && ctx.Settings.OutputStream.Type == DataStreamType.IqToVac)
      {
        ctx.IqVacSoundcard.Buffer.Clear();
      }
    }

    private void UpdatePlaybackStatus()
    {
      TimeSpan elapsed = TimeSpan.FromSeconds(playbackPosition / (double)SdrConst.AUDIO_SAMPLING_RATE);
      TimeSpan total = TimeSpan.FromSeconds(samplesInBuffer / (double)SdrConst.AUDIO_SAMPLING_RATE);
      int percent = samplesInBuffer <= 0 ? 0 : (int)Math.Round(100.0 * playbackPosition / samplesInBuffer);
      percent = Math.Clamp(percent, 0, 100);
      StatusLabel.Text = $"Playing back {(isRecordingAudio ? "Audio" : "I/Q")} {elapsed:mm\\:ss} of {total:mm\\:ss} ({percent}%)";
    }

    //----------------------------------------------------------------------------------------------
    //                                        data in
    //----------------------------------------------------------------------------------------------
    internal void AddIqSamples(DataEventArgs<Complex32> e)
    {
      if (!isRecording || iqBuffer == null || isRecordingAudio) return;

      AddSamples(e.Data, iqBuffer);
    }

    internal void AddAudioSamples(DataEventArgs<float> e)
    {
      if (!isRecording || audioBuffer == null || !isRecordingAudio) return;

      AddSamples(e.Data, audioBuffer);
    }

    private void AddSamples<T>(T[] source, T[] buffer)
    {
      // Copy data to buffer
      int samplesToWrite = Math.Min(source.Length, buffer.Length - samplesInBuffer);
      if (samplesToWrite <= 0) return;

      Array.Copy(source, 0, buffer, samplesInBuffer, samplesToWrite);
      samplesInBuffer += samplesToWrite;

      // Check if buffer is full
      if (samplesInBuffer < buffer.Length) return;

      isRecording = false;
      BeginInvoke(HandleBufferFull);
    }

    private void HandleBufferFull()
    {
      StopRecording();
      StatusLabel.Text = "Recording stopped - Buffer full (30 min)";
    }




    //----------------------------------------------------------------------------------------------
    //                                        read file
    //----------------------------------------------------------------------------------------------
    private void LoadRecording_old(string filename)
    {
      string ext = Path.GetExtension(filename).ToLowerInvariant();
      bool isMp3 = ext == ".mp3";

      using var reader = new AudioFileReader(filename);
      if (reader.WaveFormat.SampleRate != SdrConst.AUDIO_SAMPLING_RATE)
        throw new InvalidOperationException($"Unsupported sample rate: {reader.WaveFormat.SampleRate} Hz");

      if (isMp3 || reader.WaveFormat.Channels == 1)
      {
        audioBuffer = ReadAudioSamples(reader);
        iqBuffer = null;
      }
      else if (reader.WaveFormat.Channels == 2)
      {
        iqBuffer = ReadIqSamples(reader);
        audioBuffer = null;
      }
      else
      {
        throw new InvalidOperationException($"Unsupported channel count: {reader.WaveFormat.Channels}");
      }

      // Enable Save and Playback buttons
      bool hasData = samplesInBuffer > 0;
      SetSaveButtonsEnabled(hasData);
      PlaybackBtn.Enabled = hasData;

      StatusLabel.Text = $"Loaded {Path.GetFileName(filename)}";
      Log.Information($"Recording loaded from {filename}");
      WaveformPanel.Invalidate();
    }

    private float[] ReadAudioSamples(ISampleProvider reader)
    {
      int channels = reader.WaveFormat.Channels;
      float[] samples = new float[BUFFER_SIZE];
      float[] buffer = new float[4096 * channels];
      samplesInBuffer = 0;

      int samplesRead;
      while ((samplesRead = reader.Read(buffer, 0, buffer.Length)) > 0 && samplesInBuffer < BUFFER_SIZE)
      {
        int framesRead = samplesRead / channels;
        int framesToCopy = Math.Min(framesRead, BUFFER_SIZE - samplesInBuffer);

        if (channels == 1)
        {
          Array.Copy(buffer, 0, samples, samplesInBuffer, framesToCopy);
        }
        else
        {
          for (int i = 0; i < framesToCopy; i++)
          {
            float sum = 0;
            for (int channel = 0; channel < channels; channel++)
              sum += buffer[i * channels + channel];

            samples[samplesInBuffer + i] = sum / channels;
          }
        }

        samplesInBuffer += framesToCopy;
      }

      return samples;
    }

    private void LoadRecording(string filename)
    {
      string ext = Path.GetExtension(filename).ToLowerInvariant();
      bool isMp3 = ext == ".mp3";

      using var reader = new AudioFileReader(filename);

      iqBuffer = null;
      audioBuffer = null;


      if (isMp3)
      {
        // MP3 speech recordings are mono and may be stored at reduced sample rate.
        ISampleProvider source = reader;
        if (reader.WaveFormat.SampleRate != SdrConst.AUDIO_SAMPLING_RATE)
          source = new WdlResamplingSampleProvider(source, SdrConst.AUDIO_SAMPLING_RATE);
        audioBuffer = ReadAudioSamples(source);
      }
      else
      {
        if (reader.WaveFormat.SampleRate != SdrConst.AUDIO_SAMPLING_RATE)
          throw new InvalidOperationException($"Unsupported sample rate: {reader.WaveFormat.SampleRate} Hz");

        if (reader.WaveFormat.Channels == 1) audioBuffer = ReadAudioSamples(reader);
        else if (reader.WaveFormat.Channels == 2) iqBuffer = ReadIqSamples(reader);
        else throw new InvalidOperationException($"Unsupported channel count: {reader.WaveFormat.Channels}");
      }

      playbackPosition = 0;

      bool hasData = samplesInBuffer > 0;
      SetSaveButtonsEnabled(hasData);
      PlaybackBtn.Enabled = hasData;

      StatusLabel.Text = $"Loaded {Path.GetFileName(filename)}";
      Log.Information($"Recording loaded from {filename}");
      WaveformPanel.Invalidate();
    }

    private Complex32[] ReadIqSamples(ISampleProvider reader)
    {
      Complex32[] samples = new Complex32[BUFFER_SIZE];
      float[] buffer = new float[4096 * 2];
      samplesInBuffer = 0;

      int samplesRead;
      while ((samplesRead = reader.Read(buffer, 0, buffer.Length)) > 0 && samplesInBuffer < BUFFER_SIZE)
      {
        int framesRead = samplesRead / 2;
        int framesToCopy = Math.Min(framesRead, BUFFER_SIZE - samplesInBuffer);

        for (int i = 0; i < framesToCopy; i++)
          samples[samplesInBuffer + i] = new Complex32(buffer[2 * i], buffer[2 * i + 1]);

        samplesInBuffer += framesToCopy;
      }

      return samples;
    }



    //----------------------------------------------------------------------------------------------
    //                                        write file
    //----------------------------------------------------------------------------------------------
    private void SaveRecordingWithDialog(string ext)
    {
      bool isMp3 = ext == "mp3";
      bool isIqWav = ext == "wav" && !isRecordingAudio;

      using var dialog = new SaveFileDialog();
      dialog.Filter = isMp3
        ? "MP3 Files (*.mp3)|*.mp3|All Files (*.*)|*.*"
        : isIqWav
          ? "I/Q WAV Files (*.iq.wav)|*.iq.wav|WAV Files (*.wav)|*.wav|All Files (*.*)|*.*"
          : "WAV Files (*.wav)|*.wav|All Files (*.*)|*.*";
      dialog.DefaultExt = isIqWav ? "iq.wav" : ext;
      dialog.InitialDirectory = GetRecordingsPath();
      dialog.FileName = isIqWav
        ? $"{DateTime.Now:yyyy-MM-dd_HH_mm_ss}.iq.wav"
        : $"{DateTime.Now:yyyy-MM-dd_HH_mm_ss}.{ext}";

      if (dialog.ShowDialog() != DialogResult.OK) return;

      string fileName = isIqWav ? EnsureIqWavExtension(dialog.FileName) : dialog.FileName;

      try
      {
        if (isMp3) SaveRecordingAsMp3(fileName);
        else SaveRecordingAsWav(fileName);

        StatusLabel.Text = $"Saved to {Path.GetFileName(fileName)}";
        Log.Information($"Recording saved to {fileName}");
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Log.Error(ex, "Error saving recording");
      }
    }

    private void SaveRecordingAsWav(string fileName)
    {
      if (audioBuffer != null)
      {
        // write only recorded frames
        WriteWav(fileName, audioBuffer, 1, samplesInBuffer);
        return;
      }

      if (iqBuffer == null)
        throw new InvalidOperationException("No recording to save");

      float[] interleaved = new float[samplesInBuffer * 2];
      for (int i = 0; i < samplesInBuffer; i++)
      {
        interleaved[2 * i] = iqBuffer[i].Real;
        interleaved[2 * i + 1] = iqBuffer[i].Imaginary;
      }

      // framesCount is bufferPosition (each frame has two floats)
      WriteWav(fileName, interleaved, 2, samplesInBuffer);
    }

    private void SaveRecordingAsMp3_old(string filename)
    {
      if (audioBuffer == null)
        throw new InvalidOperationException("No audio data available for MP3 export");

      short[] pcm = new short[samplesInBuffer];
      for (int i = 0; i < samplesInBuffer; i++)
      {
        float sample = Math.Clamp(audioBuffer[i], -1f, 1f);
        pcm[i] = (short)(sample * short.MaxValue);
      }

      byte[] buffer = new byte[pcm.Length * sizeof(short)];
      Buffer.BlockCopy(pcm, 0, buffer, 0, buffer.Length);

      var format = new WaveFormat(SdrConst.AUDIO_SAMPLING_RATE, 16, 1);
      using var stream = new MemoryStream(buffer, writable: false);
      using var source = new RawSourceWaveStream(stream, format);
      MediaFoundationEncoder.EncodeToMp3(source, filename);
    }

    private void SaveRecordingAsMp3(string filename)
    {
      if (audioBuffer == null)
        throw new InvalidOperationException("No audio data available for MP3 export");

      const int targetSampleRate = 16000;
      const int targetBitrate = 24000;   // 24 kbps

      short[] pcm = new short[samplesInBuffer];
      for (int i = 0; i < samplesInBuffer; i++)
        pcm[i] = (short)(Math.Clamp(audioBuffer[i], -1f, 1f) * short.MaxValue);

      byte[] inputBytes = new byte[pcm.Length * sizeof(short)];
      Buffer.BlockCopy(pcm, 0, inputBytes, 0, inputBytes.Length);

      var inputFormat = new WaveFormat(SdrConst.AUDIO_SAMPLING_RATE, 16, 1);

      using var inputStream = new MemoryStream(inputBytes, writable: false);
      using var rawSource = new RawSourceWaveStream(inputStream, inputFormat);
      using var resampled = new MediaFoundationResampler(rawSource, targetSampleRate)
      {
        ResamplerQuality = 60
      };

      MediaFoundationEncoder.EncodeToMp3(resampled, filename, targetBitrate);
    }

    private void WriteWav(string fileName, float[] samples, int channels, int framesCount)
    {
      if (framesCount <= 0) return;

      // number of floats to write = framesCount * channels
      int floatsToWrite = framesCount * channels;
      int bytesToWrite = floatsToWrite * sizeof(float);

      byte[] buffer = new byte[bytesToWrite];
      Buffer.BlockCopy(samples, 0, buffer, 0, bytesToWrite);

      WaveFormat format = WaveFormat.CreateIeeeFloatWaveFormat(SdrConst.AUDIO_SAMPLING_RATE, channels);
      using var writer = new WaveFileWriter(fileName, format);
      writer.Write(buffer, 0, buffer.Length);
    }

    private string EnsureIqWavExtension(string fileName)
    {
      if (fileName.EndsWith(".iq.wav", StringComparison.OrdinalIgnoreCase)) return fileName;
      if (fileName.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
        return fileName[..^4] + ".iq.wav";

      return fileName + ".iq.wav";
    }

    private string GetRecordingsPath()
    {
      string path = Path.Combine(Utils.GetUserDataFolder(), "Recordings");
      Directory.CreateDirectory(path);
      return path;
    }



    //----------------------------------------------------------------------------------------------
    //                                        draw
    //----------------------------------------------------------------------------------------------
    private void WaveformPanel_Paint(object sender, PaintEventArgs e)
    {
      var waveformRect = DrawWaveformBackground(e.Graphics, WaveformPanel.ClientRectangle);
      DrawWaveformSamples(e.Graphics, waveformRect, WaveformPanel.ClientRectangle);
      DrawPlaybackPosition(g: e.Graphics, waveformRect);
      DrawTimeScale(e.Graphics, waveformRect, WaveformPanel.ClientRectangle);
    }

    private void DrawPlaybackPosition(Graphics g, Rectangle waveformRect)
    {
      if (samplesInBuffer <= 0 || waveformRect.Width <= 0) return;
      if (!isPlayingBack && playbackPosition <= 0) return;

      int x = waveformRect.Left + (int)Math.Round(((double)waveformRect.Width - 1) * playbackPosition / samplesInBuffer);
      x = Math.Clamp(x, waveformRect.Left, waveformRect.Right - 1);

      using var pen = new Pen(Color.Red);
      if (!isPlayingBack) pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
      g.DrawLine(pen, x, waveformRect.Top, x, waveformRect.Bottom - 1);
    }

    private Rectangle DrawWaveformBackground(Graphics g, Rectangle bounds)
    {
      int textHeight = TextRenderer.MeasureText("0", Font, Size, TextFormatFlags.NoPadding).Height;
      int scaleHeight = textHeight * 2 + 6;
      Rectangle waveformRect = new Rectangle(bounds.Left, bounds.Top, bounds.Width, Math.Max(1, bounds.Height - scaleHeight));

      int midY = waveformRect.Top + waveformRect.Height / 2;
      using var axisPen = new Pen(waveformAxisColor);
      g.DrawLine(axisPen, waveformRect.Left, midY, waveformRect.Right - 1, midY);
      g.DrawLine(axisPen, waveformRect.Left, waveformRect.Top, waveformRect.Left, waveformRect.Bottom - 1);

      return waveformRect;
    }

    private void DrawTimeScale(Graphics g, Rectangle waveformRect, Rectangle bounds)
    {
      double totalSeconds = (isRecording ? BUFFER_SIZE : Math.Max(samplesInBuffer, 0)) / (double)SdrConst.AUDIO_SAMPLING_RATE;
      if (totalSeconds <= 0 || waveformRect.Width <= 10) return;

      double[] StepsSec = { 1, 2, 5, 10, 20, 30, 60, 120, 300, 600, 1200, 1800, 3600 };
      double[] SmallStepsSec = { 1.0 / 6.0, 0.5, 1, 2, 5, 10, 30, 60, 150, 300, 600, 900, 1800 };

      double pixelsPerSecond = waveformRect.Width / Math.Max(1.0, totalSeconds);

      double step = 0, smallStep = 0;
      for (int i = 0; i < StepsSec.Length; i++)
        if (StepsSec[i] * pixelsPerSecond > 59)
        {
          step = StepsSec[i];
          smallStep = SmallStepsSec[i];
          break;
        }
      if (step == 0) return;

      g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
      int scaleShift = 8; // moved up 2 px earlier
      int scaleTopY = Math.Min(bounds.Bottom - 1, waveformRect.Bottom + scaleShift);

      // draw horizontal line along upper ends of ticks
      using var scalePen = new Pen(waveformAxisColor);
      g.DrawLine(scalePen, waveformRect.Left, scaleTopY, waveformRect.Right - 1, scaleTopY);

      // draw small ticks (downwards from horizontal line)
      double t = 0;
      int smallLen = 4;
      while (t <= totalSeconds)
      {
        int x = waveformRect.Left + (int)Math.Round(t * pixelsPerSecond);
        g.DrawLine(scalePen, x, scaleTopY, x, scaleTopY + smallLen);
        t += smallStep;
      }

      // draw major ticks and labels
      t = 0;
      int majorLen = 8;
      while (t <= totalSeconds)
      {
        int x = waveformRect.Left + (int)Math.Round(t * pixelsPerSecond);
        g.DrawLine(scalePen, x, scaleTopY, x, scaleTopY + majorLen);

        TimeSpan ts = TimeSpan.FromSeconds(t);
        string label = ts.TotalHours >= 1 ? $"{(int)ts.TotalHours:D2}:{ts:mm\\:ss}" : $"{ts:mm\\:ss}";
        var size = TextRenderer.MeasureText(label, Font, Size, TextFormatFlags.NoPadding);
        g.DrawString(label, Font, new SolidBrush(waveformAxisColor), x - size.Width / 2, scaleTopY + majorLen + 2);

        t += step;
      }
    }

    private void DrawWaveformSamples(Graphics g, Rectangle waveformRect, Rectangle bounds)
    {
      int recordedSamples = Math.Max(samplesInBuffer, 0);
      if (recordedSamples == 0) return;

      int visibleSamples = isRecording ? BUFFER_SIZE : recordedSamples;
      if (visibleSamples <= 0) return;

      float halfHeight = Math.Max(1, (waveformRect.Height - 2) / 2f);
      double gain = 1.0;
      if (GainSlider != null)
      {
        double db = GainSlider.Value;
        gain = 0.1 * Dsp.FromDb2((float)db);
      }

      using var waveformPen = new Pen(waveformForegroundColor);
      int pixelWidth = Math.Max(1, waveformRect.Width);

      for (int x = 0; x < pixelWidth; x++)
      {
        int start = (int)((long)x * visibleSamples / pixelWidth);
        int end = (int)((long)(x + 1) * visibleSamples / pixelWidth);
        if (end <= start) end = Math.Min(visibleSamples, start + 1);
        if (start >= recordedSamples) break;
        if (end > recordedSamples) end = recordedSamples;

        float maxMagnitude = 0;
        if (audioBuffer != null)
        {
          for (int i = start; i < end; i++)
            maxMagnitude = Math.Max(maxMagnitude, Math.Abs(audioBuffer[i]));
        }
        else if (iqBuffer != null)
        {
          for (int i = start; i < end; i++)
            maxMagnitude = Math.Max(maxMagnitude, iqBuffer[i].Magnitude);
        }

        int amplitude = Math.Min((int)Math.Round(maxMagnitude * gain * halfHeight), (int)halfHeight);
        if (amplitude <= 0) continue;

        int pixelX = waveformRect.Left + x;
        g.DrawLine(waveformPen, pixelX, waveformRect.Top + waveformRect.Height / 2 - amplitude, pixelX, waveformRect.Top + waveformRect.Height / 2 + amplitude);
      }
    }

    private void GainSlider_ValueChanged(object sender, EventArgs e)
    {
      double db = GainSlider.Value;
      toolTip1.SetToolTip(GainSlider, $"Gain {db:F0} dB");
      WaveformPanel.Invalidate();
    }

    private void RecordingTimer_Tick(object? sender, EventArgs e)
    {
      if (isRecording)
      {
        TimeSpan elapsed = DateTime.Now - recordingStartTime;
        StatusLabel.Text = $"Recording {(isRecordingAudio ? "Audio" : "I/Q")} {elapsed:mm\\:ss}";
        WaveformPanel.Invalidate();
      }
    }

    private void WaveformPanel_Resize(object? sender, EventArgs e)
    {
      WaveformPanel.Invalidate();
    }

    private void WaveformPanel_MouseDown(object sender, MouseEventArgs e)
    {
      if (!isPlayingBack || samplesInBuffer <= 0 || WaveformPanel.ClientRectangle.Width <= 0) return;

      int position = (int)Math.Round(e.X * (double)samplesInBuffer / WaveformPanel.ClientRectangle.Width);
      SeekPlaybackToPosition(position);
    }
  }
}
