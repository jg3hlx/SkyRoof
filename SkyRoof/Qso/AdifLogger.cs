using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ADIFLib;
using VE3NEA;

namespace SkyRoof
{
  public class AdifLogger
  {
    internal void SaveQso(QsoInfo qso)
    {
      string path = Path.Combine(Utils.GetUserDataFolder(), "Adif");
      Directory.CreateDirectory(path);
      path = Path.Combine(path, $"{qso.Utc:yyyy-MM-dd}.adif");

      if (!File.Exists(path)) File.WriteAllText(path, "<PROGRAMID:7>SkyRoof\n" +
        $"<PROGRAMVERSION:{Utils.GetVersionNumber().Length}>{Utils.GetVersionNumber()}\n" +
        "<EOH>\n");

      File.AppendAllText(path, BuildQsoString(qso));
    }

    private string BuildQsoString(QsoInfo qso)
    {
      ADIFQSO adif = new()
      {
        new("QSO_DATE", qso.Utc.ToString("yyyyMMdd"), true) ,
        new("TIME_ON", qso.Utc.ToString("HHmmss"), true),
        new("CALL", qso.Call, true),
        new("BAND", qso.Band, true),
        new("MODE", qso.Mode, true)
      };

      if (qso.Sent != "") adif.Add(new("RST_SENT", qso.Sent, true));
      if (qso.Recv != "") adif.Add(new("RST_RCVD", qso.Recv, true));
      if (qso.Grid != "") adif.Add(new("GRIDSQUARE", qso.Grid, true));
      if (qso.State != "") adif.Add(new("STATE", qso.State, true));
      if (qso.Name != "") adif.Add(new("NAME", qso.Name, true));
      if (qso.Sat != "") adif.Add(new("SAT_NAME", qso.Sat, true));
      if (qso.Sat != "") adif.Add(new("PROP_MODE", "SAT", true));
          
      string adifString = adif.ToString().Replace(" <", "<").Replace("<eor>","<EOR>");
      return $"{adifString}\n";
    }

    internal QsoInfo Augment(QsoInfo qso)
    {
      if (qso.Call == "VE3NEA")
      {
        qso.Grid = "FN03";
        qso.Name = "Alex";
        qso.State = "WA";
        qso.BackColor = "#CCCCCC";
        qso.StatusString = "Duplicate";
      }
      else
      {
        qso.Grid = "";
        qso.Name = "";
        qso.State = "";
      }

      return qso;
    }
  }
}
