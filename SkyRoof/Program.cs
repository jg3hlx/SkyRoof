using VE3NEA;

namespace SkyRoof
{
  internal static class Program
  {
    [STAThread]
    static void Main()
    {
      ExceptionLogger.Initialize();
      ApplicationConfiguration.Initialize();
      Application.Run(new MainForm());
    }
  }
}