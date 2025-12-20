using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SkyRoof
{
  /// <summary>
  /// Pinned ANSI buffer for native decoder output.
  /// Allocation-free during steady state.
  /// </summary>
  public sealed class PinnedDecodedMessageBuffer : IDisposable
  {
    private readonly byte[] buffer;
    private readonly GCHandle handle;
    private bool disposed;

    public int Capacity { get; }
    public IntPtr Pointer { get; }

    public PinnedDecodedMessageBuffer(int capacity)
    {
      if (capacity <= 0)
        throw new ArgumentOutOfRangeException(nameof(capacity));

      Capacity = capacity;
      buffer = new byte[capacity];

      handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
      Pointer = handle.AddrOfPinnedObject();
    }

    /// <summary>
    /// Clears the buffer (writes zeros).
    /// </summary>
    public void Clear()
    {
      Array.Clear(buffer, 0, buffer.Length);
    }

    /// <summary>
    /// Converts the null-terminated ANSI buffer to a managed string.
    /// Allocates exactly one string.
    /// </summary>
    public string ToStringAndClear()
    {
      int len = Array.IndexOf(buffer, (byte)0);
      if (len < 0) len = buffer.Length;

      string result = Encoding.ASCII.GetString(buffer, 0, len);
      Clear();
      return result;
    }

    /// <summary>
    /// Returns true if the buffer contains a non-empty message.
    /// </summary>
    public bool HasData()
    {
      return buffer[0] != 0;
    }

    public void Dispose()
    {
      if (disposed) return;
      disposed = true;

      if (handle.IsAllocated)
        handle.Free();
    }
  }
}
