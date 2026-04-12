using MathNet.Numerics;
using NAudio.MediaFoundation;
using NAudio.Wave;
using Serilog;
using VE3NEA;
using WeifenLuo.WinFormsUI.Docking;

namespace SkyRoof
{
  public partial class RecorderPanel : DockContent
  {
    private readonly Context ctx;
    private bool isRecordingAudio => iqBuffer == null;
    private bool isRecording = false;

    // Recording buffers
    private const int RECORDING_MINUTES = 30;
    private const int BUFFER_SIZE = SdrConst.AUDIO_SAMPLING_RATE * 60 * RECORDING_MINUTES;

    private float[]? audioBuffer;
    private Complex32[]? iqBuffer;
    private int bufferPosition = 0;
    private DateTime recordingStartTime;
    private System.Windows.Forms.Timer? recordingTimer;

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

      this.ctx = ctx;
      Log.Information("Creating RecorderPanel");

      ctx.RecorderPanel = this;
      ctx.MainForm.RecorderMNU.Checked = true;

      // Initialize recording timer
      recordingTimer = new System.Windows.Forms.Timer();
      recordingTimer.Interval = 1000; 
      recordingTimer.Tick += RecordingTimer_Tick;

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

      // Dispose timer
      recordingTimer?.Stop();
      recordingTimer?.Dispose();

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
      StatusLabel.Text = isAudio ? "Recording Audio..." : "Recording I/Q...";


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

      bufferPosition = 0;
      recordingStartTime = DateTime.Now;
      RecordTimeLabel.Visible = true;
      RecordTimeLabel.Text = isRecordingAudio ? "Audio 00:00" : "I/Q 00:00";
      recordingTimer?.Start();

      // Disable controls during recording
      RecordMenuBtn.Enabled = false;
      SetSaveButtonsEnabled(false);
      LoadBtn.Enabled = false;
      PlaybackBtn.Enabled = false;
      toolTip1.SetToolTip(RecordBtn, "Stop Recording");

      isRecording = true;
    }

    private void StopRecording()
    {
      isRecording = false;

      RecordBtn.BackColor = Color.Transparent;

      recordingTimer?.Stop();
      RecordTimeLabel.Visible = false;

      TimeSpan duration = DateTime.Now - recordingStartTime;
      StatusLabel.Text = $"Recording stopped - Duration: {duration:mm\\:ss}";
      Log.Information($"Recording stopped - {bufferPosition} samples recorded");

      // Enable Save button if we have data
      SetSaveButtonsEnabled(bufferPosition > 0);

      // Re-enable controls
      RecordMenuBtn.Enabled = true;
      LoadBtn.Enabled = true;
      PlaybackBtn.Enabled = bufferPosition > 0;
      toolTip1.SetToolTip(RecordBtn, "Record Audio");
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
      PlaybackBtn.BackColor = Color.LightGreen;
      toolTip1.SetToolTip(PlaybackBtn, "Stop Playback");
      StatusLabel.Text = "Playing back...";
      Log.Information("Starting playback");

      // TODO: Implement playback
    }

    private void StopPlayback()
    {
      PlaybackBtn.BackColor = Color.Transparent;
      toolTip1.SetToolTip(PlaybackBtn, "Start Playback");
      StatusLabel.Text = "Playback stopped";
      Log.Information("Playback stopped");

      // TODO: Stop playback
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
      int samplesToWrite = Math.Min(source.Length, buffer.Length - bufferPosition);
      if (samplesToWrite <= 0) return;

      Array.Copy(source, 0, buffer, bufferPosition, samplesToWrite);
      bufferPosition += samplesToWrite;

      // Check if buffer is full
      if (bufferPosition < buffer.Length) return;

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
    private void LoadRecording(string filename)
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
      bool hasData = bufferPosition > 0;
      SetSaveButtonsEnabled(hasData);
      PlaybackBtn.Enabled = hasData;

      StatusLabel.Text = $"Loaded {Path.GetFileName(filename)}";
      Log.Information($"Recording loaded from {filename}");
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

    private void SaveRecordingAsWav(string fileName)
    {
      if (audioBuffer != null)
      {
        // write only recorded frames
        WriteWav(fileName, audioBuffer, 1, bufferPosition);
        return;
      }

      if (iqBuffer == null)
        throw new InvalidOperationException("No recording to save");

      float[] interleaved = new float[bufferPosition * 2];
      for (int i = 0; i < bufferPosition; i++)
      {
        interleaved[2 * i] = iqBuffer[i].Real;
        interleaved[2 * i + 1] = iqBuffer[i].Imaginary;
      }

      // framesCount is bufferPosition (each frame has two floats)
      WriteWav(fileName, interleaved, 2, bufferPosition);
    }

    private void SaveRecordingAsMp3(string filename)
    {
      if (audioBuffer == null)
        throw new InvalidOperationException("No audio data available for MP3 export");

      short[] pcm = new short[bufferPosition];
      for (int i = 0; i < bufferPosition; i++)
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

    private float[] ReadAudioSamples(ISampleProvider reader)
    {
      int channels = reader.WaveFormat.Channels;
      float[] samples = new float[BUFFER_SIZE];
      float[] buffer = new float[4096 * channels];
      bufferPosition = 0;

      int samplesRead;
      while ((samplesRead = reader.Read(buffer, 0, buffer.Length)) > 0 && bufferPosition < BUFFER_SIZE)
      {
        int framesRead = samplesRead / channels;
        int framesToCopy = Math.Min(framesRead, BUFFER_SIZE - bufferPosition);

        if (channels == 1)
        {
          Array.Copy(buffer, 0, samples, bufferPosition, framesToCopy);
        }
        else
        {
          for (int i = 0; i < framesToCopy; i++)
          {
            float sum = 0;
            for (int channel = 0; channel < channels; channel++)
              sum += buffer[i * channels + channel];

            samples[bufferPosition + i] = sum / channels;
          }
        }

        bufferPosition += framesToCopy;
      }

      return samples;
    }

    private Complex32[] ReadIqSamples(ISampleProvider reader)
    {
      Complex32[] samples = new Complex32[BUFFER_SIZE];
      float[] buffer = new float[4096 * 2];
      bufferPosition = 0;

      int samplesRead;
      while ((samplesRead = reader.Read(buffer, 0, buffer.Length)) > 0 && bufferPosition < BUFFER_SIZE)
      {
        int framesRead = samplesRead / 2;
        int framesToCopy = Math.Min(framesRead, BUFFER_SIZE - bufferPosition);

        for (int i = 0; i < framesToCopy; i++)
          samples[bufferPosition + i] = new Complex32(buffer[2 * i], buffer[2 * i + 1]);

        bufferPosition += framesToCopy;
      }

      return samples;
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



    //----------------------------------------------------------------------------------------------
    //                                        show info
    //----------------------------------------------------------------------------------------------
    private void WaveformPanel_Paint(object sender, PaintEventArgs e)
    {
      // Example:

      Graphics g = e.Graphics;
      g.Clear(Color.White);
      g.DrawString("Waveform Display", Font, Brushes.Black, 10, 10);
    }

    private void GainSlider_ValueChanged(object sender, EventArgs e)
    {
      // TODO: Update playback or recording gain
      // int gain = GainSlider.Value;
    }

    private void RecordingTimer_Tick(object? sender, EventArgs e)
    {
      if (isRecording)
      {
        TimeSpan elapsed = DateTime.Now - recordingStartTime;
        string prefix = isRecordingAudio ? "Audio" : "I/Q";
        RecordTimeLabel.Text = $"{prefix} {elapsed:mm\\:ss}";
      }
    }

  }
}
