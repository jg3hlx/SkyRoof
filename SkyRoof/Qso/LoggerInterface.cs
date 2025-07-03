using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace SkyRoof
{
  public class QsoInfo
  {
    public DateTime Utc { get; set; } = DateTime.UtcNow;
    public string Band { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string Sat { get; set; } = string.Empty;
    public string Call { get; set; } = string.Empty;
    public string Grid { get; set; }= string.Empty;
    public string State { get; set; } = string.Empty;
    public string Sent { get; set; }= string.Empty;
    public string Recv { get; set; }= string.Empty;
    public string Name { get; set; } = string.Empty;

    public string StatusString = string.Empty;
    public string BackColor = "#FFFFFF";
    public string ForeColor = "#000000";
  }

  public static class LoggerInterfaceDll
  {
    public const string DllName = "LoggerInterface.dll";

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Init();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern string SaveQso(string json);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern string Augment(string json);
  }

  public class LoggerInterface
  {
    private bool DllAvailable;
    private AdifLogger? AdifLogger;

    public LoggerInterface()
    {
      try
      {
        LoggerInterfaceDll.Init();
        DllAvailable = true;
        Log.Information("LoggerInterface DLL loaded successfully.");
      }
      catch (Exception ex)
      {
        Log.Warning(ex, "Unable to load LoggerInterface.dll. Using AdifLogger.");
        DllAvailable = false;
        AdifLogger = new();
      }
    }

    public void SaveQso(QsoInfo qso)
    {
      string json = System.Text.Json.JsonSerializer.Serialize(qso);
      Log.Information($"Saving QSO: {json}");

      string result = DllAvailable ? SaveQsoUsingDll(json) : SaveQsoToAdif(qso);

      if (string.IsNullOrEmpty(result))
        Log.Information("QSO Saved.");
      else
        MessageBox.Show("Save QSO failed: " + result, "Save QSO", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public QsoInfo Augment(QsoInfo qso)
    {
      if (string.IsNullOrEmpty(qso.Call) || qso.Call.Trim().Length < 3) return qso;

      string json = System.Text.Json.JsonSerializer.Serialize(qso);

      try
      {
        if (DllAvailable)
        {
          json = LoggerInterfaceDll.Augment(json);
          return System.Text.Json.JsonSerializer.Deserialize<QsoInfo>(json)!;
        }
        else
          return AdifLogger!.Augment(qso);
      }
      catch (Exception ex)
      {
        Log.Error(ex, $"Error augmenting QSO {json}.");
        return qso;
      }
    }

    private string SaveQsoUsingDll(string json)
    {
      try
      {
        return LoggerInterfaceDll.SaveQso(json);
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    private string SaveQsoToAdif(QsoInfo qso)
    {
      try
      {
        AdifLogger!.SaveQso(qso);
        return string.Empty;
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
  }
}
