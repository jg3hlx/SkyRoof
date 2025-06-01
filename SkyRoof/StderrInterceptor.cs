using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace SkyRoof
{
  // Generated with ChatGPT!

  internal class StderrInterceptor : IDisposable
  {
    public event EventHandler<string>? StderrReceived;

    private IntPtr readPipe;
    private IntPtr writePipe;
    private FileStream? readStream;
    private Thread? readerThread;
    private bool disposed;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CreatePipe(out IntPtr hReadPipe, out IntPtr hWritePipe,
        IntPtr lpPipeAttributes, uint nSize);

    [DllImport("kernel32.dll")]
    private static extern bool SetStdHandle(int nStdHandle, IntPtr handle);

    [DllImport("msvcrt.dll", SetLastError = true)]
    private static extern int _open_osfhandle(IntPtr osHandle, int flags);

    [DllImport("msvcrt.dll", SetLastError = true)]
    private static extern int _dup2(int oldfd, int newfd);

    private const int STD_ERROR_HANDLE = -12;
    private const int O_TEXT = 0x4000;
    private const int _FILENO_STDERR = 2;

    public void RedirectStderr()
    {
      if (!CreatePipe(out readPipe, out writePipe, IntPtr.Zero, 0))
        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "CreatePipe failed.");

      if (!SetStdHandle(STD_ERROR_HANDLE, writePipe))
        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "SetStdHandle failed.");

      int crtFd = _open_osfhandle(writePipe, O_TEXT);
      if (crtFd == -1)
        throw new InvalidOperationException("Failed to open CRT file descriptor from pipe handle.");

      if (_dup2(crtFd, _FILENO_STDERR) != 0)
        throw new InvalidOperationException("Failed to redirect CRT stderr.");

      // Begin reading from the read pipe
      var safeHandle = new SafeFileHandle(readPipe, ownsHandle: true);
      readStream = new FileStream(safeHandle, FileAccess.Read, bufferSize: 4096, isAsync: false);

      readerThread = new Thread(() =>
      {
        try
        {
          using var reader = new StreamReader(readStream, Encoding.UTF8);
          string? line;
          while ((line = reader.ReadLine()) != null)
          {
            StderrReceived?.Invoke(this, line);
          }
        }
        catch (Exception ex)
        {
          StderrReceived?.Invoke(this, $"[stderr read error: {ex.Message}]");
        }
      })
      {
        IsBackground = true
      };

      readerThread.Start();
    }

    public void Dispose()
    {
      if (disposed) return;
      disposed = true;

      readStream?.Dispose();
      readerThread?.Join(500);
    }
  }
}
