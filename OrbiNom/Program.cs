using VE3NEA;

namespace OrbiNom
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