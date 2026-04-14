using System.Globalization;
using System.Text.Json;

namespace SkyRoof
{
  public class RecordingEvents
  {
    public class RecordingEvent
    {
      public DateTime Utc { get; set; }
      public string EventType { get; set; } = string.Empty;
      public string NewValue { get; set; } = string.Empty;
      public string Satellite { get; set; } = string.Empty;
      public string Transmitter { get; set; } = string.Empty;
      public string Mode { get; set; } = string.Empty;
      public string Callsign { get; set; } = string.Empty;
      public string Offset { get; set; } = string.Empty;

      public string GetTooltipText()
      {
        string time = Utc.ToLocalTime().ToString("HH:mm:ss");
        List<string> lines = EventType switch
        {
          "satellite" => [$"{time} satellite changed"],
          "transmitter" => [$"{time} transmitter changed"],
          "mode" => [$"{time} mode changed"],
          "qso" => [$"{time} QSO saved"],
          "offset" => [$"{time} offset changed"],
          _ => [$"{time} {EventType}: {NewValue}"],
        };

        if (EventType == "satellite")
        {
          AddTooltipLine(lines, "SAT", Satellite);
          AddTooltipLine(lines, "TX", Transmitter);
          AddTooltipLine(lines, "MODE", Mode);
        }
        else if (EventType == "transmitter")
        {
          AddTooltipLine(lines, "TX", Transmitter, NewValue);
          AddTooltipLine(lines, "MODE", Mode);
        }
        else if (EventType == "mode") AddTooltipLine(lines, "MODE", Mode, NewValue);
        else if (EventType == "qso") AddTooltipLine(lines, "CALL", Callsign, NewValue);
        else if (EventType == "offset") AddTooltipLine(lines, "OFFSET", Offset, NewValue);

        return string.Join("\n", lines);
      }

      private static void AddTooltipLine(List<string> lines, string label, params string[] values)
      {
        string value = values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v)) ?? string.Empty;
        if (value != string.Empty) lines.Add($"{label}: {value}");
      }
    }

    public List<RecordingEvent> Events { get; set; } = [];

    private string? lastSatelliteValue;
    private string? lastTransmitterValue;
    private string? lastModeValue;
    private double? lastRememberedOffset;

    public void Start(Context ctx)
    {
      Events.Clear();

      lastSatelliteValue = GetSelectedSatelliteValue(ctx);
      lastTransmitterValue = GetSelectedTransmitterValue(ctx);
      lastModeValue = GetDownlinkModeValue(ctx);
      lastRememberedOffset = GetCurrentTransponderOffset(ctx);

      var utc = DateTime.UtcNow;
      if (!string.IsNullOrWhiteSpace(lastSatelliteValue))
        Add(utc, "satellite", lastSatelliteValue, lastSatelliteValue, lastTransmitterValue ?? string.Empty, lastModeValue ?? string.Empty, string.Empty, string.Empty);
    }

    public void Add(DateTime utc, string eventType, string newValue)
    {
      Add(utc, eventType, newValue, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
    }

    public void Add(DateTime utc, string eventType, string newValue, string satellite, string transmitter, string mode, string callsign, string offset)
    {
      Events.Add(new RecordingEvent
      {
        Utc = DateTime.SpecifyKind(utc, DateTimeKind.Utc),
        EventType = eventType,
        NewValue = newValue,
        Satellite = satellite,
        Transmitter = transmitter,
        Mode = mode,
        Callsign = callsign,
        Offset = offset,
      });
    }

    public bool RememberChanges(Context ctx)
    {
      DateTime utc = DateTime.UtcNow;
      string? satelliteValue = GetSelectedSatelliteValue(ctx);
      string? transmitterValue = GetSelectedTransmitterValue(ctx);
      string? modeValue = GetDownlinkModeValue(ctx);
      double? offset = GetCurrentTransponderOffset(ctx);
      bool satelliteChanged = !string.IsNullOrWhiteSpace(satelliteValue) && satelliteValue != lastSatelliteValue;
      bool transmitterChanged = !string.IsNullOrWhiteSpace(transmitterValue) && transmitterValue != lastTransmitterValue;
      bool modeChanged = !string.IsNullOrWhiteSpace(modeValue) && modeValue != lastModeValue;

      if (satelliteChanged)
      {
        lastSatelliteValue = satelliteValue;
        lastTransmitterValue = transmitterValue;
        lastModeValue = modeValue;
        lastRememberedOffset = offset;
        Add(utc, "satellite", satelliteValue!, satelliteValue!, transmitterValue ?? string.Empty, modeValue ?? string.Empty, string.Empty, string.Empty);
        return true;
      }

      if (transmitterChanged)
      {
        lastTransmitterValue = transmitterValue;
        if (modeChanged) lastModeValue = modeValue;
        lastRememberedOffset = offset;
        Add(utc, "transmitter", transmitterValue!, string.Empty, transmitterValue!, modeValue ?? string.Empty, string.Empty, string.Empty);
        return true;
      }

      if (modeChanged)
      {
        lastModeValue = modeValue;
        Add(utc, "mode", modeValue!, string.Empty, string.Empty, modeValue!, string.Empty, string.Empty);
        return true;
      }

      if (offset == null)
      {
        lastRememberedOffset = null;
        return false;
      }

      if (lastRememberedOffset == null)
      {
        lastRememberedOffset = offset;
        return false;
      }

      if (Math.Abs(offset.Value - lastRememberedOffset.Value) <= 1000) return false;

      lastRememberedOffset = offset;
      Add(utc, "offset", FormatOffset(offset.Value), string.Empty, string.Empty, string.Empty, string.Empty, FormatOffset(offset.Value));
      return true;
    }

    public bool RememberQsoSaved(string callsign)
    {
      if (string.IsNullOrWhiteSpace(callsign)) return false;

      string call = callsign.Trim().ToUpperInvariant();
      Add(DateTime.UtcNow, "qso", call, string.Empty, string.Empty, string.Empty, call, string.Empty);
      return true;
    }

    public double GetRelativeSeconds(RecordingEvent recordingEvent)
    {
      if (Events.Count == 0) return 0;
      return Math.Max(0, (recordingEvent.Utc - Events[0].Utc).TotalSeconds);
    }

    public void Save(string fileName)
    {
      string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(fileName, json);
    }

    public static RecordingEvents Load(string fileName)
    {
      try
      {
        if (!File.Exists(fileName)) return new();

        string json = File.ReadAllText(fileName);
        RecordingEvents? recordingEvents = JsonSerializer.Deserialize<RecordingEvents>(json);
        if (recordingEvents == null) return new();

        foreach (var recordingEvent in recordingEvents.Events)
          if (recordingEvent.Utc.Kind == DateTimeKind.Unspecified)
            recordingEvent.Utc = DateTime.SpecifyKind(recordingEvent.Utc, DateTimeKind.Utc);

        if (recordingEvents.Events.Count > 0)
        {
          recordingEvents.lastSatelliteValue = recordingEvents.Events.LastOrDefault(e => !string.IsNullOrWhiteSpace(e.Satellite))?.Satellite;
          recordingEvents.lastTransmitterValue = recordingEvents.Events.LastOrDefault(e => !string.IsNullOrWhiteSpace(e.Transmitter))?.Transmitter;
          recordingEvents.lastModeValue = recordingEvents.Events.LastOrDefault(e => !string.IsNullOrWhiteSpace(e.Mode))?.Mode;

          var lastOffset = recordingEvents.Events.LastOrDefault(e => e.EventType == "offset")?.Offset;
          if (string.IsNullOrWhiteSpace(lastOffset)) lastOffset = recordingEvents.Events.LastOrDefault(e => e.EventType == "offset")?.NewValue;
          if (!string.IsNullOrWhiteSpace(lastOffset) && lastOffset.EndsWith(" kHz") &&
            double.TryParse(lastOffset[..^4], NumberStyles.Float, CultureInfo.InvariantCulture, out double offsetKhz))
            recordingEvents.lastRememberedOffset = offsetKhz * 1000;
        }

        return recordingEvents;
      }
      catch
      {
        return new();
      }
    }

    public static string FormatOffset(double offset)
    {
      return string.Create(CultureInfo.InvariantCulture, $"{offset / 1000d:F1} kHz");
    }

    private static string? GetSelectedSatelliteValue(Context ctx)
    {
      return ctx.SatelliteSelector.SelectedSatellite?.name;
    }

    private static string? GetSelectedTransmitterValue(Context ctx)
    {
      return ctx.SatelliteSelector.SelectedTransmitter?.description;
    }

    private static string? GetDownlinkModeValue(Context ctx)
    {
      if (ctx.FrequencyControl.RadioLink.TxCust == null) return null;
      return ctx.FrequencyControl.RadioLink.DownlinkMode.ToString();
    }

    private static double? GetCurrentTransponderOffset(Context ctx)
    {
      var radioLink = ctx.FrequencyControl.RadioLink;
      if (radioLink.TxCust == null || !radioLink.IsTransponder) return null;
      return radioLink.TransponderOffset;
    }
  }
}
