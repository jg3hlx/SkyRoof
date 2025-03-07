using CSCore;
using VE3NEA;

namespace VE3NEA
{
  public class WaveSource : IWaveSource
  {
    public const int AUDIO_SAMPLING_RATE = 48_000;

    private readonly WaveFormat format;
    private RingBuffer<float> ringBuffer = new(AUDIO_SAMPLING_RATE);

    public WaveSource(int? samplingRate = null)
    {
      format = new WaveFormat(samplingRate ?? AUDIO_SAMPLING_RATE, 32, 1, AudioEncoding.IeeeFloat);
    }

  public void AddSamples(float[] samples, int offset = 0, int? count = null)
    {
      ringBuffer.Write(samples, offset, count ?? samples.Length);
    }

    public void ClearBuffer()
    {
      ringBuffer.Clear(); 
    }




    //------------------------------------------------------------------------------------------------------------------
    //                                             IWaveSource
    //------------------------------------------------------------------------------------------------------------------
    public bool CanSeek => false;
    public WaveFormat WaveFormat => format;
    public long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public long Length => throw new NotImplementedException();
    public int Read(byte[] buffer, int offset, int count)
    {
      ringBuffer.ReadBytes(buffer, offset, count);
      return count;
    }




    //------------------------------------------------------------------------------------------------------------------
    //                                             IDisposable
    //------------------------------------------------------------------------------------------------------------------
    public void Dispose()
    {
      // nothing to dispose of
    }
  }
}
