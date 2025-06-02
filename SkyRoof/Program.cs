using Serilog;
using VE3NEA;

namespace SkyRoof
{
  internal static class Program
  {
    [STAThread]
    static void Main()
    {
      using (new SingleInstanceEnforcer())
      {
        try
        {
          ExceptionLogger.Initialize();
          ApplicationConfiguration.Initialize();
          Application.Run(new MainForm());
        }
        catch (Exception ex)
        {
          Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
          Log.Information("Application is shutting down");
          Log.CloseAndFlush();
        }
      }
    }
  }
}