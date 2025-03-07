using System.Diagnostics;
using System.Runtime.InteropServices;
using MathNet.Numerics;
using VE3NEA;


namespace OrbiNom
{
  public unsafe class Slicer : ThreadedProcessor<Complex32>
  {
    public enum Mode { USB, LSB, USB_D, LSB_D, CW, FM }
    private static readonly int[] Bandwidths = [1500, 1500, 2000, 2000, 250, 8000];
    private static readonly int[] ModeOffsets = [1500, -1500, 2000, -2000, 600, 0];

    private const int STOPBAND_REJECTION_DB = 80;
    private const float USEFUL_BANDWIDTH = 0.95f;
    private const int FILTER_DELAY = 50; // the default in LiquidDsp is 15
    public const int OUTPUT_SAMPLING_RATE = 48000;

    private int OctaveDecimationFactor, RationalInterpolationFactor, RationalDecimationFactor;
    private NativeLiquidDsp.nco_crcf* FirstMixer, SecondMixer;
    private NativeLiquidDsp.msresamp2_crcf* msresamp2;
    private NativeLiquidDsp.rresamp_crcf* rresamp;

    private FifoBuffer<Complex32> InputBuffer = new();
    private FifoBuffer<Complex32> OctaveResamplerInputBuffer = new();
    private FifoBuffer<Complex32> RationalResamplerInputBuffer = new();
    private FifoBuffer<Complex32> RationalResamplerOutputBuffer = new();
    private DataEventArgsPool<float> ArgsPool = new();
    private double RationalResamplerInputRate;
    private double offset;
    private Mode mode;


    public double InputRate { get; private set; }
    public Mode CurrentMode { get => mode; set => SetMode(value); }
    public double Bandwidth { get => Bandwidths[(int)CurrentMode]; }
    public double FrequencyOffset { get => offset; set => SetOffset(value); }

    public event EventHandler<DataEventArgs<float>>? DataAvailable;


    public Slicer(double inputRate, double frequencyOffset = 0, Mode mode = Mode.CW)
    {
      InputRate = inputRate;

      FirstMixer = NativeLiquidDsp.nco_crcf_create(NativeLiquidDsp.LiquidNcoType.LIQUID_NCO);
      SecondMixer = NativeLiquidDsp.nco_crcf_create(NativeLiquidDsp.LiquidNcoType.LIQUID_NCO);

      FrequencyOffset = frequencyOffset;

      RationalResamplerInputRate = CreateOctaveResampler();
      CurrentMode = mode;
    }


    public void SetOffset(double offset)
    {
      this.offset = offset;
      NativeLiquidDsp.nco_crcf_set_frequency(FirstMixer, (float)(Geo.TwoPi * offset / InputRate));
    }

    private void SetMode(Mode mode)
    {
      this.mode = mode;
      ModeChanged = true;
    }

    private double CreateOctaveResampler()
    {
      int octaveStageCount = (int)Math.Ceiling(Math.Log2(InputRate / OUTPUT_SAMPLING_RATE)) - 1;
      OctaveDecimationFactor = 1 << octaveStageCount;

      if (OctaveDecimationFactor == 1) return InputRate;

      msresamp2 = NativeLiquidDsp.msresamp2_crcf_create(
        NativeLiquidDsp.LiquidResampType.LIQUID_RESAMP_DECIM,
        (uint)octaveStageCount,
        // todo: we only need (16/2)kHz/48kHz
        0.5f * USEFUL_BANDWIDTH,
        0,
        STOPBAND_REJECTION_DB
        );

      return InputRate / OctaveDecimationFactor;
    }

    private void CreateRationalResampler()
    {
      // find the ratio
      double rationalResamplingFactor = OUTPUT_SAMPLING_RATE / RationalResamplerInputRate;
      Debug.Assert(rationalResamplingFactor >= 0.5 && rationalResamplingFactor < 1);
      (RationalInterpolationFactor, RationalDecimationFactor) = Dsp.ApproximateRatio(rationalResamplingFactor, 1e-4);

      // design lowpass filter
      float bandwidth = Bandwidths[(int)CurrentMode] / (float)OUTPUT_SAMPLING_RATE;
      float fc = bandwidth / RationalInterpolationFactor;

      int filterLength = 2 * FILTER_DELAY * RationalInterpolationFactor + 1;
      var filter = NativeLiquidDsp.firfilt_crcf_create_kaiser((uint)filterLength, fc, STOPBAND_REJECTION_DB, 0);
      var coeffPointer = NativeLiquidDsp.firfilt_crcf_get_coefficients(filter);

      // create resampler
      rresamp = NativeLiquidDsp.rresamp_crcf_create(
        (uint)RationalInterpolationFactor,
        (uint)RationalDecimationFactor,
        FILTER_DELAY,
        coeffPointer
        );

      //byte[] coeffBytes = new byte[filterLength * sizeof(float)];
      //Marshal.Copy((nint)coeffPointer, coeffBytes, 0, coeffBytes.Length);
      //string base64filt = Convert.ToBase64String(coeffBytes);

      // the filter itself is no longer needed
      NativeLiquidDsp.firfilt_crcf_destroy(filter);
    }

    protected override void Process(DataEventArgs<Complex32> args)
    {
      CheckModeChange();

      InputBuffer.Data = args.Data;
      InputBuffer.Count = args.Data.Length;

      // mix down to baseband
      fixed (Complex32* pData = InputBuffer.Data)
      {
        NativeLiquidDsp.nco_crcf_mix_block_down(FirstMixer, pData, pData, (uint)InputBuffer.Count);
      }

      // downsample and lowpass-filter
      if (msresamp2 == null)
        ApplyRationalResampler(InputBuffer);
      else
      {
        ApplyOctaveResampler(InputBuffer);
        ApplyRationalResampler(RationalResamplerInputBuffer);
      }

      // mix up to mode-dependent pitch
      if (Bandwidths[(int)CurrentMode] != 0)
        fixed (Complex32* pData = RationalResamplerOutputBuffer.Data)
        {
          NativeLiquidDsp.nco_crcf_mix_block_up(SecondMixer, pData, pData, (uint)RationalResamplerOutputBuffer.Count);
        }


      // return the real part
      int outputCount = RationalResamplerOutputBuffer.Count;
      var outputArgs = ArgsPool.Rent(outputCount);
      for (int i = 0; i < outputCount; i++) outputArgs.Data[i] = RationalResamplerOutputBuffer.Data[i].Real * 1000;
      RationalResamplerOutputBuffer.Count = 0;
      DataAvailable?.Invoke(this, outputArgs);
      ArgsPool.Return(outputArgs);
    }

    private bool ModeChanged = false;

    private void CheckModeChange()
    {
      if (!ModeChanged) return;
      ModeChanged = false;

      if (rresamp != null) NativeLiquidDsp.rresamp_crcf_destroy(rresamp);
      CreateRationalResampler();

      float offset = ModeOffsets[(int)CurrentMode];
      NativeLiquidDsp.nco_crcf_set_frequency(SecondMixer, (float)(Geo.TwoPi * offset / OUTPUT_SAMPLING_RATE));      
    }

    public void ApplyOctaveResampler(FifoBuffer<Complex32> buffer)
    {
      OctaveResamplerInputBuffer.Append(buffer);
      int blockCount = OctaveResamplerInputBuffer.Count / OctaveDecimationFactor;
      if (blockCount == 0) return;
      RationalResamplerInputBuffer.EnsureExtraSpace(blockCount);

      fixed (Complex32* pInBuffer = OctaveResamplerInputBuffer.Data)
      fixed (Complex32* pOutBuffer = RationalResamplerInputBuffer.Data)
      {
        Complex32* pBlock = pInBuffer;
        Complex32* pOut = pOutBuffer + RationalResamplerInputBuffer.Count;

        for (int blockNo = 0; blockNo < blockCount; blockNo++)
        {
          int rc = NativeLiquidDsp.msresamp2_crcf_execute(msresamp2, pBlock, out pOut[blockNo]);
          if (rc != 0) throw new Exception($"LiquidDsp error {rc}");
          pBlock += OctaveDecimationFactor;
        }
      }

      OctaveResamplerInputBuffer.Dump(blockCount * OctaveDecimationFactor);
      RationalResamplerInputBuffer.Count += blockCount;
    }


    private void ApplyRationalResampler(FifoBuffer<Complex32> buffer)
    {
      int blockCount = buffer.Count / RationalDecimationFactor;
      if (blockCount == 0) return;

      int consumedCount = blockCount * RationalDecimationFactor;
      int outputCount = blockCount * RationalInterpolationFactor;
      RationalResamplerOutputBuffer.EnsureExtraSpace(outputCount);

      fixed (Complex32* pInBuffer = buffer.Data)
      fixed (Complex32* pOutBuffer = RationalResamplerOutputBuffer.Data)
      {
        Complex32* pIn = pInBuffer;
        Complex32* pOut = pOutBuffer;

        for (int blockNo = 0; blockNo < blockCount; blockNo++)
        {
          int rc = NativeLiquidDsp.rresamp_crcf_execute(rresamp, pIn, pOut);
          // rc is never zero yet resampling works correctly
          //if (rc != 0) throw new Exception($"LiquidDsp error {rc}");
          pIn += RationalDecimationFactor;
          pOut += RationalInterpolationFactor;
        }
      }

      buffer.Dump(consumedCount);
      RationalResamplerOutputBuffer.Count += outputCount;
    }

    public override void Dispose()
    {
      base.Dispose();

      if (FirstMixer != null) NativeLiquidDsp.nco_crcf_destroy(FirstMixer);
      if (msresamp2 != null) NativeLiquidDsp.msresamp2_crcf_destroy(msresamp2);
      if (rresamp != null) NativeLiquidDsp.rresamp_crcf_destroy(rresamp);
      if (SecondMixer != null) NativeLiquidDsp.nco_crcf_destroy(SecondMixer);

      FirstMixer = null;
      msresamp2 = null;
      rresamp = null;
      SecondMixer = null;
    }
  }
}
