using System.Runtime.InteropServices;
using System.Text;
using VE3NEA;

public sealed class Ft4DecodeBuffers : IDisposable
{
  public readonly float[] AudioSamples;
  public readonly byte[] MyCall;
  public readonly byte[] HisCall;

  private readonly GCHandle hAudio;
  private readonly GCHandle hMyCall;
  private readonly GCHandle hHisCall;

  public Ft4DecodeBuffers()
  {
    AudioSamples = new float[NativeFT4Coder.DECODE_SAMPLE_COUNT];
    MyCall = new byte[NativeFT4Coder.MAX_CALL_LENGTH + 1];
    HisCall = new byte[NativeFT4Coder.MAX_CALL_LENGTH + 1];

    hAudio = GCHandle.Alloc(AudioSamples, GCHandleType.Pinned);
    hMyCall = GCHandle.Alloc(MyCall, GCHandleType.Pinned);
    hHisCall = GCHandle.Alloc(HisCall, GCHandleType.Pinned);
  }

  public void SetMyCall(string call)
  {
    WriteCString(call, MyCall);
  }

  public void SetHisCall(string call)
  {
    WriteCString(call, HisCall);
  }

  private static void WriteCString(string s, byte[] buffer)
  {
    Array.Clear(buffer, 0, buffer.Length);

    int len = 0;
    if (!string.IsNullOrEmpty(s))
    {
      len = Math.Min(s.Length, buffer.Length - 1);
      Encoding.ASCII.GetBytes(s, 0, len, buffer, 0);
    }
    buffer[len] = 0;
  }

  public void Dispose()
  {
    hAudio.Free();
    hMyCall.Free();
    hHisCall.Free();
  }
}
