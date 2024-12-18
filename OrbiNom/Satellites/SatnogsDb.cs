using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;
using VE3NEA;

namespace OrbiNom
{
  public class SatnogsDb
  {
    private readonly string DataFolder, DownloadsFolder;
    private Dictionary<string, SatnogsDbSatellite> SatelliteList = new();
    private readonly HttpClient DownloadHttpClient = new();
    private CancellationTokenSource cts;

    public IEnumerable<SatnogsDbSatellite> Satellites { get => SatelliteList.Values; }
    public bool Loaded { get => SatelliteList.Count > 0; }

    public event EventHandler<ProgressChangedEventArgs>? DownloadProgress;
    public event EventHandler? TleUpdated;
    public event EventHandler? ListUpdated;


    public SatnogsDb()
    {
      DataFolder = Utils.GetUserDataFolder();
      DownloadsFolder = Path.Combine(DataFolder, "Downloads");
      Directory.CreateDirectory(DownloadsFolder);
    }

    public void LoadFromFile()
    {
      SatelliteList.Clear();

      try
      {
        string path = Path.Combine(DataFolder, "Satellites.json");
        string json = File.ReadAllText(path);
        var satellites = JsonConvert.DeserializeObject<SatnogsDbSatelliteList>(json);
        SatelliteList = satellites.ToDictionary(s => s.sat_id);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Failed to load satellite list");
        SatelliteList.Clear();
      }
    }

    public void SaveToFile()
    {
      string path = Path.Combine(DataFolder, "Satellites.json");
      File.WriteAllText(path, JsonConvert.SerializeObject(Satellites));
    }

    internal void UpdateSatelliteList(SatnogsDb db)
    {
      SatelliteList = db.SatelliteList;
      ListUpdated?.Invoke(this, EventArgs.Empty);
    }




    //----------------------------------------------------------------------------------------------
    //                              download satellite data
    //----------------------------------------------------------------------------------------------
    public void AbortDownload()
    {
      cts.Cancel();
    }

    public async Task DownloadAll()
    {
      cts = new CancellationTokenSource();

      await Download("satellites");
      DownloadProgress?.Invoke(this, new(20, null));

      await Download("transmitters");
      DownloadProgress?.Invoke(this, new(40, null));

      await Download("tle");
      DownloadProgress?.Invoke(this, new(60, null));

      await DownloadJE9PEL();
      DownloadProgress?.Invoke(this, new(80, null));

      await DownloadLotwSatList();
      DownloadProgress?.Invoke(this, new(100, null));
    }

    public async void DownloadTle()
    {
      cts = new CancellationTokenSource();
      await Download("tle");
     
      try
      {
        ImportSatnogsTle();
        TleUpdated?.Invoke(this, EventArgs.Empty);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "TLE import failed");
        throw;
      }
    }

    private async Task Download(string name)
    {
      string url = $"https://db.satnogs.org/api/{name}/?format=json";
      string json = await DownloadHttpClient.GetStringAsync(url, cts.Token);
      cts.Token.ThrowIfCancellationRequested();

      File.WriteAllText(Path.Combine(DownloadsFolder, $"{name}.json"), json);
    }

    private async Task DownloadJE9PEL()
    {
      string url = "http://www.ne.jp/asahi/hamradio/je9pel/satslist.csv";
      string csv = await DownloadHttpClient.GetStringAsync(url, cts.Token);
      cts.Token.ThrowIfCancellationRequested();

      File.WriteAllText(Path.Combine(DownloadsFolder, "JE9PEL.csv"), csv);
    }

    private async Task DownloadLotwSatList()
    {
      string url = "https://lotw.arrl.org/lotw-help/frequently-asked-questions";
      string html = await DownloadHttpClient.GetStringAsync(url, cts.Token);
      cts.Token.ThrowIfCancellationRequested();

      File.WriteAllText(Path.Combine(DownloadsFolder, "LoTW_FAQ.html"), html);
    }




    //----------------------------------------------------------------------------------------------
    //                                import satellite data
    //----------------------------------------------------------------------------------------------
    private bool CheckFilesPresent()
    {
      return
        File.Exists(Path.Combine(DownloadsFolder, "satellites.json")) &&
        File.Exists(Path.Combine(DownloadsFolder, "transmitters.json")) &&
        File.Exists(Path.Combine(DownloadsFolder, "tle.json")) &&
        File.Exists(Path.Combine(DownloadsFolder, "JE9PEL.csv")) &&
        File.Exists(Path.Combine(DownloadsFolder, "LoTW_FAQ.html"));
    }
    public void ImportAll()
    {
      try
      {
        if (!CheckFilesPresent()) throw new Exception("Satellite download(s) missing");

        SatelliteList.Clear();

        ImportSatnogsSatellites();
        ImportSatnogsTransmitters();
        ImportSatnogsTle();
        ImportJE9PEL();

        foreach (var sat in Satellites) sat.BuildAllNames();

        ParseLotwSatList();
        //again, with lotw added
        foreach (var sat in Satellites) sat.BuildAllNames();

        foreach (var sat in Satellites) sat.SetFlags();

        SaveToFile();
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Error importing satellite list");
        throw;
      }
    }

    private void ImportSatnogsSatellites()
    {
      string json = File.ReadAllText(Path.Combine(DownloadsFolder, "satellites.json"));
      SatnogsDbSatelliteList satellites = JsonConvert.DeserializeObject<SatnogsDbSatelliteList>(json);
      SatelliteList = satellites.ToDictionary(s => s.sat_id);
    }

    private void ImportSatnogsTransmitters()
    {
      string json = File.ReadAllText(Path.Combine(DownloadsFolder, "transmitters.json"));
      SatnogsDbTransmitterList transmitters = JsonConvert.DeserializeObject<SatnogsDbTransmitterList>(json);
      foreach (SatnogsDbTransmitter t in transmitters) SatelliteList.GetValueOrDefault(t.sat_id)?.Transmitters?.Add(t);
    }

    private void ImportSatnogsTle()
    {
      string json = File.ReadAllText(Path.Combine(DownloadsFolder, "tle.json"));
      SatnogsDbTleList tles = JsonConvert.DeserializeObject<SatnogsDbTleList>(json);

      foreach (SatnogsDbTle tle in tles)
        if (SatelliteList.TryGetValue(tle.sat_id, out SatnogsDbSatellite sat))
          sat.Tle = tle;
    }

    // example: RS-44;44909;145.935-145.995;435.670-435.610;435.605 ;SSB CW;RS44;Operational
    private void ImportJE9PEL()
    {
      Dictionary<int, List<JE9PELtransmitter>> dict = new();

      foreach (string line in File.ReadAllLines(Path.Combine(DownloadsFolder, "JE9PEL.csv")))
      {
        JE9PELtransmitter tx = new(line);
        if (tx.NoradId == 0) continue;
        dict.TryAdd(tx.NoradId, new());
        dict[tx.NoradId].Add(tx);
      }

      // insert in satnogs db
      foreach (var sat in Satellites)
        if (sat.norad_cat_id != null)
          if (dict.ContainsKey((int)sat.norad_cat_id))
          {
            sat.JE9PELtransmitters = dict[(int)sat.norad_cat_id];
            sat.JE9PEL_Callsigns = sat.JE9PELtransmitters.SelectMany(t => t.Call.Split(' ')).Where(c => c != "").Distinct().ToList();
            sat.JE9PEL_Names = sat.JE9PELtransmitters.SelectMany(t => t.Name.Split(' ')).Where(c => c != "").Distinct().ToList();
          }
    }



    static RegexOptions RegexOptions = RegexOptions.Singleline | RegexOptions.Compiled;
    Regex TableRegex = new Regex(@"(?:<tbody>.*?){2}(.*?)<\/tbody>", RegexOptions);
    Regex RowRegex = new Regex(@"(?:<tr>.*?<td>(.*?)<\/td>.*?<td>(.*?)<\/td>.*?<\/tr>)", RegexOptions);

    private void ParseLotwSatList()
    {
      string html = File.ReadAllText(Path.Combine(DownloadsFolder, "LoTW_FAQ.html"));
      string table = TableRegex.Match(html).Groups[1].Value;
      var matches = RowRegex.Matches(table);
      
      foreach (Match m in matches)
      {
        string name = m.Groups[1].Value;
        
        var sat = Satellites.FirstOrDefault(s => s.AllNames.Contains(name));
        if (sat == null) continue;

        sat.LotwName = sat.name = name;
        sat.names += ", " + m.Groups[2].Value;
      }
    }

    internal void Customize(Dictionary<string, SatelliteCustomization> satelliteCustomizations)
    {
      foreach (var cust in satelliteCustomizations.Values)
      {
        if (!SatelliteList.TryGetValue(cust.sat_id, out SatnogsDbSatellite? sat)) return;

        if (!string.IsNullOrEmpty(cust.Name))
        {
          sat.name = cust.Name;
          sat.BuildAllNames();
        }
      }
    }
  }
}