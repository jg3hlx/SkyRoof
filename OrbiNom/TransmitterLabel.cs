namespace OrbiNom
{
  internal class TransmitterLabel
  {
    public SatellitePass Pass;
    public List<SatnogsDbTransmitter>? Transmitters;
    public long Frequency;

    public Size Size;
    public float x;
    public double? Span;
    public string Tooltip;

    public RectangleF Rect;

    public TransmitterLabel(SatellitePass pass, long freq)
    {
      Pass = pass;
      Frequency = freq;

      Transmitters = pass.Satellite.Transmitters.Where(tx => tx.downlink_low == freq).ToList();
      Tooltip = string
        .Join("\n", Transmitters.Select(tx => $"  - {tx.service}   {tx.type}   {tx.description}")
        .Distinct());

      var transponder = Transmitters.FirstOrDefault(t => t.downlink_high != null);
      Span = transponder?.downlink_high - transponder?.downlink_low;
    }
  }
}
