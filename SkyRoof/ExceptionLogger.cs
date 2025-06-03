using System.Diagnostics;
using System.Runtime.InteropServices;
using Serilog;
using SharpGL;
using SkyRoof;
using VE3NEA;

namespace VE3NEA
{
  public class ExceptionLogger
  {
    private static NativeSoapySdr.SoapySDRLogHandlerDelegate LogHandlerDelegate = new(SoapySdrLogHandler);

    // private static StderrInterceptor StderrInterceptor = new();
    public static void Initialize()
    {
      string fileName = Path.Combine(Utils.GetUserDataFolder(), "Logs", "log_.txt");
      Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
          .WriteTo.File(fileName, 
            rollingInterval: RollingInterval.Day, 
            fileSizeLimitBytes: 1_000_000, 
            retainedFileCountLimit: 10,
            shared: true
           ).CreateLogger();

      Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
      Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

      Log.Information($"Starting {typeof(Utils).Assembly.GetName().FullName}");

      NativeSoapySdr.SoapySDR_registerLogHandler(LogHandlerDelegate);
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      var exception = e.ExceptionObject as Exception;
      Log.Error(exception, "Worker thread exception occurred:");
    }

    private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      Log.Error(e.Exception, "UI thread exception occurred:");
      if (ShowDialog(e.Exception) == DialogResult.Abort) Application.Exit();
    }

    private static DialogResult ShowDialog(Exception e)
    {
      string errorMessage = $"Exception: {e.Message}\nModule: {e.Source} \nFunction: {e.TargetSite}";
      return MessageBox.Show(errorMessage, AppDomain.CurrentDomain.FriendlyName, MessageBoxButtons.AbortRetryIgnore,
          MessageBoxIcon.Stop);
    }

    internal static void CheckOpenglError(OpenGL gl, bool log = true)
    {
      uint err;

      while ((err = gl.GetError()) != OpenGL.GL_NO_ERROR)
        if (log)
        {
          string errorName = gl.ErrorString(err);
          if (string.IsNullOrEmpty(errorName)) errorName = $"error {err}";
          string message = $"OpenGL: {errorName}\n{new StackTrace(true)}";
          Log.Error(message);
          Debug.WriteLine(message);
        }
    }

    public static void SoapySdrLogHandler(NativeSoapySdr.SoapySDRLogLevel logLevel, IntPtr messagePtr)
    {
      try
      {
        if (messagePtr == IntPtr.Zero) return;
        string message = Marshal.PtrToStringAnsi(messagePtr)!;
        Log.Information($"[{logLevel}]: {message}");
      }
      catch { }
    }
  }
}
