using System.Diagnostics;
using NAudio.Wave;
using VE3NEA;

namespace SkyRoof
{
  public unsafe class Ft4Decoder : ThreadedProcessor<float>
  {
    private float[] Samples = [];
    private int SampleCount = 0;
    DateTime DataUtc;
    private float[] Slot = new float[NativeFT4Coder.DECODE_SAMPLE_COUNT];
    private Ft4DecodeBuffers? Buffers = new ();

    public const int FT4_SIGNAL_BANDWIDTH = 83; // Hz
    public int RxAudioFrequency = 1500;
    public int TxAudioFrequency = 1500;
    public int CutoffFrequency = 4000;
    public string MyCall = " ";
    public string TheirCall = " ";

    public int CurrentSlotNumber { get; private set; }
    public int DecodedSlotNumber { get; private set; }

    public event EventHandler<DataEventArgs<string>>? SlotDecoded;

    protected override void Process(DataEventArgs<float> args)
    {
      AppendData(args);

      if (GetSlotToDecode()) Decode(Slot);
    }

    private void AppendData(DataEventArgs<float> args)
    {
      int neededLength = SampleCount + args.Count;
      if (Samples.Length < neededLength) Array.Resize(ref Samples, neededLength);

      Array.Copy(args.Data, 0, Samples, SampleCount, args.Count);
      SampleCount += args.Count;

      DataUtc = args.Utc;
    }

    //{!}
    int balance = 0;
    private bool GetSlotToDecode()
    {
      // find slot boundaries
      double secondsSinceMidnight = (DataUtc - DataUtc.Date).TotalSeconds;
      CurrentSlotNumber = (int)Math.Truncate(secondsSinceMidnight / NativeFT4Coder.TIMESLOT_SECONDS);

      double secondsIntoSlot = secondsSinceMidnight - (CurrentSlotNumber * NativeFT4Coder.TIMESLOT_SECONDS);
      int samplesIntoSlot = (int)(secondsIntoSlot * NativeFT4Coder.SAMPLING_RATE);

      int slotStartIndex = SampleCount - samplesIntoSlot;
      int slotEndIndex = slotStartIndex + NativeFT4Coder.DECODE_SAMPLE_COUNT;

      // is new slot available?
      bool slotAvailable = slotStartIndex >= 0 && slotEndIndex <= SampleCount;
      bool slotDecoded = CurrentSlotNumber == DecodedSlotNumber;
      if (!slotAvailable || slotDecoded) return false;

      // extract slot
      Array.Copy(Samples, slotStartIndex, Slot, 0, NativeFT4Coder.DECODE_SAMPLE_COUNT);

      // DEBUG
      int samplesDiff = slotEndIndex - (int)(7.5f * NativeFT4Coder.SAMPLING_RATE);
      int msDiff = DateTime.UtcNow.Subtract(DataUtc).Milliseconds;
      balance += samplesDiff;
      Debug.WriteLine($"------------------------------------------------------------------------------------------- samplesDiff: {samplesDiff}, msDiff: {msDiff} balance: {balance}");

      // dump used samples
      int samplesToKeep = SampleCount - slotEndIndex;
      Array.Copy(Samples, slotEndIndex, Samples, 0, samplesToKeep);
      SampleCount = samplesToKeep;

      return true;
    }

    private void Decode(float[] samples)
    {
      // scale to -1..1
      float max = samples.Max(Math.Abs);
      if (max > 0)
        for (int i = 0; i < samples.Length; i++)
          samples[i] /= max;

      int stage = (int)NativeFT4Coder.QsoStage.CALLING;
      Buffers.SetMyCall(MyCall);
      Buffers.ClearDecoded();

      NativeFT4Coder.decode_ft4_f(samples, ref stage, ref RxAudioFrequency, ref CutoffFrequency, 
        Buffers.MyCall, Buffers.HisCall, Buffers.DecodedChars);

      string[] messages = Buffers.GetDecodedMessages();

      var messageUtc = DataUtc.AddSeconds(-((SampleCount + NativeFT4Coder.DECODE_SAMPLE_COUNT) / (double)NativeFT4Coder.SAMPLING_RATE));
      SlotDecoded?.Invoke(this, new DataEventArgs<string>(messages, messageUtc));

      DecodedSlotNumber = CurrentSlotNumber;
    }


    public override void Dispose()
    {
      base.Dispose();
      Buffers?.Dispose();
      Buffers = null;
    }


    public void SaveSamples()
    {
      WriteWav(Slot);
    }

    public void PlayBackSamples()
    {
      Decode(ReadWav());
    }

    private static float[] ReadWav()
    {
      float[] samples;
      string filePath = Path.Combine(Utils.GetUserDataFolder(), "samples.wav");

      using (var reader = new WaveFileReader(filePath))
      {
        Debug.Assert(reader.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat);
        Debug.Assert(reader.WaveFormat.SampleRate == NativeFT4Coder.SAMPLING_RATE);
        Debug.Assert(reader.WaveFormat.Channels == 1);
        Debug.Assert(reader.Length == NativeFT4Coder.DECODE_SAMPLE_COUNT * sizeof(float));

        var buffer = new byte[reader.Length];
        int bytesRead = reader.Read(buffer, 0, buffer.Length);
        samples = new float[bytesRead / 4];
        Buffer.BlockCopy(buffer, 0, samples, 0, bytesRead);
      }

      return samples;
    }

    private static void WriteWav(float[] samples)
    {
      // float to bytes
      byte[] buffer = new byte[samples.Length * 4];
      Buffer.BlockCopy(samples, 0, buffer, 0, buffer.Length);

      string filePath = Path.Combine(Utils.GetUserDataFolder(), "samples.wav");
      var format = WaveFormat.CreateIeeeFloatWaveFormat(NativeFT4Coder.SAMPLING_RATE, 1);

      using var writer = new WaveFileWriter(filePath, format);
      writer.Write(buffer, 0, buffer.Length);
    }
  }
}
