using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace OrbiNom
{
  public class Customization
  {
    public Dictionary<string, SatelliteCustomization> SatelliteCustomizations = new();
    public SatelliteGroups SatelliteGroups = new();
  }

  public class SatelliteCustomization
  {
    public string sat_id;
    public string? Name;
  }

  public class SatelliteGroup
  {
    public string? Name;
    public List<string> SatelliteIds = new();
  }

  public class SatelliteGroups : List<SatelliteGroup>
  {
    public void SetDefaultIfEmpty()
    {
      if (Count > 0) return;
      var group = new SatelliteGroup { Name = "Orbital Stations" };
      group.SatelliteIds.Add("XSKZ-5603-1870-9019-3066"); // ISS
      Add(group);
    }
  }
}