# PRD: Transverter Offset Support for SkyRoof

## Problem Statement

Users running HF transceivers connected to VHF/UHF transverters (e.g., K3S + Elecraft XV series,
IC-735 + any transverter) want to use SkyRoof's SDR as the receiver via the transverter's IF output
(28–30 MHz range). Currently SkyRoof always tunes the SDR to the actual RF frequency (144/435 MHz),
which requires the SDR to cover VHF/UHF. With a transverter setup, the SDR should be fixed to the
IF frequency band while SkyRoof continues to display and process everything at the correct RF frequency.

A secondary requirement applies to "dumb" HF rigs without built-in transverter configuration
(e.g., IC-735): SkyRoof must send the IF frequency (e.g., 29.950 MHz) to the radio via CAT instead
of the RF frequency (145.950 MHz), because the rig cannot interpret VHF/UHF frequencies.

---

## Goals

1. Fix the SDR center frequency to the IF band when transverter mode is active; track the
   Doppler-corrected signal by moving only the slicer offset, preserving waterfall history.
2. Optionally subtract the LO offset from CAT-commanded frequencies for rigs that have no built-in
   XVRT configuration.
3. Support independently enabling/disabling the offset for the SDR and each CAT channel (RX CAT,
   TX CAT).
4. Allow separate LO offset values for the SDR path and the CAT path per band, since the SDR
   transverter and the TX transverter may use different local oscillators.
5. Handle multiple transverter bands automatically based on frequency range, including the AO-7 UHF
   uplink sub-band exception.
6. Non-ham satellite frequencies (400 MHz telemetry band, etc.) receive zero offset — no behavior
   change.

## Non-Goals

- Changing what frequencies are *displayed* in the VFO widget or waterfall (always show RF).
- Automated switching between transverter profiles mid-pass (user sets configuration, software
  applies it).

---

## Architecture

### How Frequencies Flow Today

```
RadioLink.ComputeFrequencies()
  → CorrectedDownlinkFrequency  (RF, Doppler-adjusted)
  → CorrectedUplinkFrequency    (RF, Doppler-adjusted)

FrequencyWidget.RadioLinkToRadio()
  → ctx.Sdr.Frequency           = RF center  (tunes SDR hardware, moves waterfall)
  → ctx.Slicer.SetOffset()      = RF - SDR center (audio demodulator offset)
  → ctx.CatControl.Rx.SetRxFrequency(CorrectedDownlinkFrequency)   ← RF to radio
  → ctx.CatControl.Tx.SetTxFrequency(CorrectedUplinkFrequency)     ← RF to radio
```

### Proposed Change

Two new helpers look up the configured transverter band list:
- `GetSdrLoOffset(rfFrequency)` — returns the SDR LO for the matching band (or 0)
- `GetCatLoOffset(rfFrequency)` — returns the CAT LO for the matching band (or 0)

**SDR center frequency is set once per band, not per Doppler tick.**
When `SdrOffsetEnabled` is true and the active downlink frequency falls into a different transverter
band than the SDR is currently covering, the SDR is retuned to the IF center of that band (midpoint
of `RfLow - SdrLoOffset` … `RfHigh - SdrLoOffset`). Within the same band — including switching
between satellites — the SDR center never moves. `BringToPassband()` is suppressed entirely. Only
the slicer offset tracks the Doppler-corrected IF position within the fixed passband.

```
On band change (SdrOffsetEnabled, current band ≠ previous band):
  ifCenter           = (band.RfLow + band.RfHigh) / 2 - band.SdrLoOffset
  ctx.Sdr.Frequency  = ifCenter            // one retune, waterfall resets here

Per Doppler tick:
  ifFrequency   = CorrectedDownlinkFrequency - GetSdrLoOffset(CorrectedDownlinkFrequency)
  if ifFrequency within [ctx.Sdr.Frequency ± bandwidth/2]:
    ctx.Slicer.SetOffset(ifFrequency - ctx.Sdr.Frequency)   // normal tracking
  else:
    // signal outside SDR passband — slicer not updated, no retune
    // waterfall history preserved; signal goes off-screen until it drifts back

CAT (per Doppler tick, when offset enabled):
  RX CAT freq = CorrectedDownlinkFrequency - GetCatLoOffset(CorrectedDownlinkFrequency)
  TX CAT freq = CorrectedUplinkFrequency   - GetCatLoOffset(CorrectedUplinkFrequency)
```

Because Doppler is already baked into `CorrectedDownlinkFrequency` before the LO is subtracted,
the IF frequency sent to the radio inherits the same Doppler correction automatically. The LO
itself is fixed, so Doppler rate is unaffected.

**Passband coverage:** with the satellite frequency ranges used in the default band table, all IF
spans fit within the SDR's 3 MHz maximum bandwidth. The only marginal case is UHF 70cm (3 MHz IF
span = 3 MHz max SDR bandwidth); if the SDR is configured at less than its maximum bandwidth,
signals at the 435 or 438 MHz edges may go off-screen. All other bands are well within the
passband at any realistic SDR bandwidth setting.

---

## Data Model

### New `TransverterBand`

```csharp
public class TransverterBand
{
  public string Name { get; set; }      // "VHF 2m", "UHF 70cm", "UHF AO-7 uplink"
  public long RfLow { get; set; }       // Hz, inclusive
  public long RfHigh { get; set; }      // Hz, inclusive
  public long SdrLoOffset { get; set; } // Hz, subtracted from RF for SDR tuning
  public long CatLoOffset { get; set; } // Hz, subtracted from RF for CAT commands
}
```

### New `TransverterSettings`

```csharp
public class TransverterSettings
{
  public bool SdrOffsetEnabled   { get; set; } = false;
  public bool RxCatOffsetEnabled { get; set; } = false;
  public bool TxCatOffsetEnabled { get; set; } = false;
  public List<TransverterBand> Bands { get; set; } = DefaultBands();
}
```

### Default Band Table

| Name                  | RF Low (MHz) | RF High (MHz) | SDR LO (MHz) | CAT LO (MHz) | IF Range         |
|-----------------------|--------------|---------------|--------------|--------------|------------------|
| VHF 2m                | 145.800      | 146.000       | 116.000      | 116.000      | 29.8–30.0 MHz    |
| UHF 70cm AO-7 uplink  | 432.000      | 434.000       | 404.000      | 404.000      | 28.0–30.0 MHz    |
| UHF 70cm              | 435.000      | 438.000       | 407.000      | 407.000      | 28.0–31.0 MHz    |

RF ranges reflect actual satellite allocations: 145.8–146.0 MHz for VHF and 435–438 MHz for UHF.
The AO-7 mode U-V uplink (432–434 MHz) is the only known exception and uses a different LO
(404 vs 407 MHz); it is split into its own entry so frequency-range lookup resolves it automatically.

With these ranges all three IF spans fit within the SDR's 3 MHz maximum bandwidth:
- VHF: 200 kHz IF span — always fully in passband
- UHF AO-7 uplink: 2 MHz IF span — always in passband
- UHF 70cm: 3 MHz IF span — exactly matches max SDR bandwidth; signals at band edges may go
  off-screen if the SDR runs below its maximum bandwidth setting

Having separate SDR LO and CAT LO columns allows users whose SDR and TX transverters use different
local oscillators to configure them independently. In the typical case they are identical.

Non-ham frequencies (e.g., 400 MHz band) fall outside all bands → both `GetSdrLoOffset()` and
`GetCatLoOffset()` return 0 → SDR and CAT behave exactly as today.

---

## UI: Settings Dialog — New "Transverter" Section

Add a new page to the existing Settings dialog (alongside CAT, Audio, etc.):

```
┌─ Transverter ────────────────────────────────────────────────────┐
│  [✓] Apply offset to SDR tuning                                  │
│  [ ] Apply offset to RX CAT frequency                            │
│  [ ] Apply offset to TX CAT frequency                            │
│                                                                   │
│  Band Offsets:                                                    │
│  ┌──────────────────┬──────────┬──────────┬─────────┬─────────┐  │
│  │ Name             │ RF Low   │ RF High  │ SDR LO  │ CAT LO  │  │
│  ├──────────────────┼──────────┼──────────┼─────────┼─────────┤  │
│  │ VHF 2m           │ 144.000  │ 148.000  │ 116.000 │ 116.000 │  │
│  │ UHF 70cm AO-7 up │ 432.000  │ 434.000  │ 404.000 │ 404.000 │  │
│  │ UHF 70cm         │ 434.000  │ 440.000  │ 407.000 │ 407.000 │  │
│  └──────────────────┴──────────┴──────────┴─────────┴─────────┘  │
│  [Add Band]  [Remove]                                             │
└───────────────────────────────────────────────────────────────────┘
```

Frequencies in the table are displayed in MHz for readability; stored in Hz in JSON settings.

---

## Behavior by Use Case

### Case 1: K3S with XVRT config + SDR on transverter IF
- `SdrOffsetEnabled = true`, `RxCatOffsetEnabled = false`, `TxCatOffsetEnabled = false`
- On first use of the UHF 70cm band: SDR center tuned to the IF band center (≈ 29 MHz). Waterfall
  then shows a fixed IF-domain view for all UHF satellites. Switching to another UHF satellite
  keeps the SDR center in place; switching to a VHF satellite triggers a retune to the VHF IF
  center. Doppler shifts only the slicer offset within the passband.
- K3S receives 435.835 MHz from SkyRoof (unchanged) and applies its own XVRT conversion internally.

### Case 2: IC-735 (no transverter awareness) + SDR on transverter IF
- `SdrOffsetEnabled = true`, `RxCatOffsetEnabled = true`, `TxCatOffsetEnabled = true`
- Example FO-29 (Doppler off for clarity):
  - Downlink 435.835 MHz → SDR center fixed at ~29 MHz; slicer offset = 28.835 - 29 = -165 kHz;
    IC-735 RX commanded to 28.835 MHz
  - Uplink 145.965 MHz → IC-735 TX commanded to 29.965 MHz

### Case 3: SDR directly on VHF/UHF antenna, smart rig
- All offsets disabled. No change in behavior.

### Case 4: AO-7 mode U-V
- Uplink 432.xxx MHz → "UHF 70cm AO-7 uplink" band → CAT LO = 404 MHz → IF ≈ 28.xxx MHz
- Downlink 145.xxx MHz → "VHF 2m" band → SDR LO = 116 MHz → SDR fixed at ~29 MHz
- Each link uses its own correct LO automatically.

### Case 5: Non-ham telemetry satellite (e.g., 400.550 MHz)
- No matching band → offsets = 0 → SDR tunes to 400.550 MHz directly → no change.

---

## Code Changes

| File | Change |
|------|--------|
| `Settings/TransverterSettings.cs` | **New.** `TransverterSettings` and `TransverterBand` with separate `SdrLoOffset` / `CatLoOffset` per band and default band table |
| `Settings/Settings.cs` | Add `public TransverterSettings Transverter { get; set; } = new();` |
| `Settings/SettingsDialog` | Add Transverter page with enable checkboxes and editable band grid |
| `RadioLink.cs` | No change needed — offset is applied downstream |
| `Widgets/FrequencyWidget.cs` — `SettingsToRadioLink()` | When `SdrOffsetEnabled` and the downlink band has changed, retune SDR center to `(band.RfLow + band.RfHigh) / 2 - band.SdrLoOffset`; otherwise leave SDR center unchanged |
| `Widgets/FrequencyWidget.cs` — `RadioLinkToRadio()` | Apply `GetCatLoOffset()` before calling `SetRxFrequency` / `SetTxFrequency` when offsets enabled |
| `Widgets/FrequencyWidget.cs` — `SetSlicerFrequency()` | When `SdrOffsetEnabled`, check passband in IF domain: compute `ifFrequency = CorrectedDownlinkFrequency - GetSdrLoOffset(...)`, update slicer offset only if `ifFrequency` is within the current SDR passband; skip `BringToPassband()` entirely |
| `Widgets/FrequencyWidget.cs` — `BringToPassband()` | No-op when `SdrOffsetEnabled` is true |

---

## Open Questions / Decisions Deferred

1. **Waterfall indicator:** A small transverter-active indicator on the waterfall could be useful
   (similar to the Doppler correction indicator). Low priority; can be added later.
2. **Frequency scale label:** When transverter is active the waterfall frequency axis shows IF
   values (28–30 MHz). It may be useful to relabel it as "IF" or offset-annotate the scale.
   Decision deferred to implementation.
