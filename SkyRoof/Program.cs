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
        ExceptionLogger.Initialize(); ExceptionLogger.Initialize();
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
      }
    }
  }
}