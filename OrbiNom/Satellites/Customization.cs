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
    public List<SatelliteGroup> SatelliteGroups = new();
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
}