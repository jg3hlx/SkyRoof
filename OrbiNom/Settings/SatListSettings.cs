using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbiNom
{
  public class SatListSettings
  {
    public DateTime LastDownloadTime {get; set; } = DateTime.MinValue;
    public DateTime LastTleTime { get; set; } = DateTime.MinValue;
  }
}
