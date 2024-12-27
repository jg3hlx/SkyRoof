using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace OrbiNom
{
  public class SatelliteSettings
  {
    public Dictionary<string, SatelliteCustomization> SatelliteCustomizations = new();
    public List<SatelliteGroup> SatelliteGroups = new();
    public string? SelectedGroup;

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
        SatelliteGroups.Add(group);
      }

      // ensure there is a selected group
      var selectedGroup = SatelliteGroups.FirstOrDefault(g => g.Id == SelectedGroup);
      if (string.IsNullOrEmpty(SelectedGroup) || selectedGroup == null)
      {
        selectedGroup = SatelliteGroups[0];
        SelectedGroup = selectedGroup.Id;
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
    public string? SelectedTransmitter;
  }

  public class SatelliteGroup
  {
    public string Id = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public List<string> SatelliteIds = new();
    public string? SelectedSatId;
  }
}