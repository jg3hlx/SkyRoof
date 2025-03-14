using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

/*
    "SatelliteGroups": [
      {
        "Id": "4cf18f87-1eee-4d72-a2f3-4dd9883c5d52",
        "SatelliteIds": ["REOO-2119-3738-4964-1579", "OTOF-8025-8953-4756-4779", "HHSS-6325-1344-4603-7774", "PMAW-9203-2442-8666-3249", "XTKP-9404-9174-4088-2406", "XSKZ-5603-1870-9019-3066", "KLYF-4722-0218-1441-3015", "ANJN-0857-0652-4935-2307", "HIEK-3729-5596-2727-4744", "UFYD-5782-6372-2920-6054", "IRES-5964-9687-1982-0089", "PBTZ-3525-0189-2410-5216", "JLZT-0277-4784-7719-1707"],
        "SelectedSatId": "REOO-2119-3738-4964-1579",
        "Name": "Active Ham Sats"
      },
      {
        "Id": "44e2fa3a-daad-4563-98a5-f2b0fed98114",
        "SatelliteIds": ["XTDR-0995-4168-5549-5936", "WMGA-3000-1529-8035-2634", "NGHO-5109-0535-2414-7008", "WMPC-3806-8308-1122-5138", "EEIT-0315-8565-1765-1994", "AHVS-7983-8710-8819-8034", "VMQF-4924-7475-1891-3199", "TDVY-5283-4801-7631-6209", "LEDR-0123-1783-1341-9342"],
        "SelectedSatId": "XTDR-0995-4168-5549-5936",
        "Name": "Inactive Ham Sats"
      },
      {
        "Id": "710f3a71-7396-4b2a-951e-9c21bd2bf586",
        "SatelliteIds": ["XSKZ-5603-1870-9019-3066", "AYDR-6527-9294-8555-4924"],
        "SelectedSatId": "XSKZ-5603-1870-9019-3066",
        "Name": "Orbital Stations"
      }
    ]
*/

namespace OrbiNom
{
  public class SatelliteSettings
  {
    public DateTime LastDownloadTime = DateTime.MinValue;
    public DateTime LastTleTime = DateTime.MinValue;

    public Dictionary<string, SatelliteCustomization> SatelliteCustomizations = new();
    public List<SatelliteGroup> SatelliteGroups = new();
    public string SelectedGroupId;
    public string SelectedSatelliteId;

    public void DeleteInvalidData(SatnogsDb db)
    {
      // remove deleted sats
      foreach (var group in SatelliteGroups)
        group.SatelliteIds.RemoveAll(id => db.GetSatellite(id) == null);

      Sanitize();
    }

    public void Sanitize()
    {
      // ensure groups are non-empty
      SatelliteGroups.RemoveAll(g => g.SatelliteIds.Count == 0);

      // ensure that at least 1 group is present
      if (SatelliteGroups.Count == 0)
      {
        var group = new SatelliteGroup { Name = "Orbital Stations" };
        group.SatelliteIds.Add("XSKZ-5603-1870-9019-3066"); // ISS
        group.SatelliteIds.Add("AYDR - 6527 - 9294 - 8555 - 4924"); // CSS
        SatelliteGroups.Add(group);
      }

      // ensure there is a selected group
      var selectedGroup = SatelliteGroups.FirstOrDefault(g => g.Id == SelectedGroupId);
      if (string.IsNullOrEmpty(SelectedGroupId) || selectedGroup == null)
      {
        selectedGroup = SatelliteGroups[0];
        SelectedGroupId = selectedGroup.Id;
      }

      // ensure there is a selected sat in each group
      foreach (var group in SatelliteGroups)
        if (string.IsNullOrEmpty(group.SelectedSatId) || !group.SatelliteIds.Contains(group.SelectedSatId))
          group.SelectedSatId = group.SatelliteIds[0];

      // selected transmitter may be null
    }
  }

  public class SatelliteCustomization
  {
    public string sat_id;
    public string? Name;
    public string? SelectedTransmitterId;
    public int DownlinkDopplerCorrection;
    public int DownlinkManualCorrection;
    public bool DownlinkDopplerCorrectionEnabled = true;
    public bool DownlinkManualCorrectionEnabled = true;
  }

  public class SatelliteGroup
  {
    public string Id = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public List<string> SatelliteIds = new();
    public string? SelectedSatId;
  }
}