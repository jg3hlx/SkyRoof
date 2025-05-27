using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;
using VE3NEA;

namespace SkyRoof
{
  public class SatnogsDb
  {
    private readonly string DataFolder, DownloadsFolder;
    private Dictionary<string, SatnogsDbSatellite> SatelliteList = new();
    private readonly HttpClient DownloadHttpClient = new();
    private CancellationTokenSource cts;

    public IEnumerable<SatnogsDbSatellite> Satellites { get => SatelliteList.Values; }
    public bool Loaded { get => loaded; }
    private bool loaded;

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

        foreach(var sat in satellites)
          foreach (var tx in sat.Transmitters)
            tx.Satellite = sat;

        loaded = SatelliteList.Count > 0;
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

    internal void ReplaceSatelliteList(SatnogsDb db)
    {
      SatelliteList = db.SatelliteList;
      loaded = SatelliteList.Count > 0;
      ListUpdated?.Invoke(this, EventArgs.Empty);
    }

    public SatnogsDbSatellite? GetSatellite(string satId)
    {
      return SatelliteList.GetValueOrDefault(satId);
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
      Log.Information("satellites downloaded");
      DownloadProgress?.Invoke(this, new(33, null));

      await Download("transmitters");
      Log.Information("transmitters downloaded");
      DownloadProgress?.Invoke(this, new(66, null));

      await Download("tle");
      Log.Information("tle downloaded");
      DownloadProgress?.Invoke(this, new(99, null));

      await DownloadJE9PEL();
      Log.Information("JE9PEL downloaded");
      DownloadProgress?.Invoke(this, new(99, null));
    }

    public async Task DownloadTle()
    {
      cts = new CancellationTokenSource();
      await Download("tle");
     
      try
      {
        ImportSatnogsTle();
        SaveToFile();
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




    //----------------------------------------------------------------------------------------------
    //                                import satellite data
    //----------------------------------------------------------------------------------------------
    private bool CheckFilesPresent()
    {
      return
        File.Exists(Path.Combine(DownloadsFolder, "satellites.json")) &&
        File.Exists(Path.Combine(DownloadsFolder, "transmitters.json")) &&
        File.Exists(Path.Combine(DownloadsFolder, "tle.json")) &&
        File.Exists(Path.Combine(DownloadsFolder, "JE9PEL.csv"));
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

        var satNames = new SatelliteNames();

        foreach (var sat in Satellites)
        {
          if (sat.norad_cat_id != null)
          {
            sat.AmsatEntries = satNames.Amsat.GetValueOrDefault(sat.norad_cat_id.Value) ?? new();
            sat.LotwName = satNames.Lotw.GetValueOrDefault(sat.norad_cat_id.Value);
            if (sat.LotwName != null) { sat.name = sat.LotwName; sat.names += ", " + sat.LotwName; }
          }

          sat.BuildAllNames();
          sat.SetFlags();
        }

        SaveToFile();

        loaded = SatelliteList.Count > 0;
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
      SatnogsDbTransmitterList transmitters = JsonConvert.DeserializeObject<SatnogsDbTransmitterList>(json)!;

      // only transmitters with downlink frequency are imported.
      // currently 3 transmitters out of 2K+ have downlink_low=null: KOSEN-2, CHUBUSAT-2 and CHUBUSAT-3
      foreach (SatnogsDbTransmitter t in transmitters.Where(t => t.downlink_low.HasValue))
      {
        var sat = SatelliteList.GetValueOrDefault(t.sat_id);
        if (sat != null)
        {
          sat.Transmitters.Add(t);
          t.Satellite = sat;
        }
      }
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

    internal void Customize(Dictionary<string, SatelliteCustomization> satelliteCustomizations)
    {
      foreach (var cust in satelliteCustomizations.Values)
      {
        if (cust.sat_id == null) continue;

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