namespace VE3NEA
{
  public class Ft4Slot
  {
    public const double TxLengthSeconds = NativeFT4Coder.ENCODE_SAMPLE_COUNT / (double)NativeFT4Coder.SAMPLING_RATE;

    private DateTime utc;

    public DateTime Utc { get => utc; set => SetUtc(value); }
    public int SlotNumber { get; private set; }
    public double SlotStartSeconds { get; private set; }
    public double SecondsIntoSlot { get; private set; }
    public int SamplesIntoSlot { get; private set; }
    public DateTime CurrentSlotStartTime { get; private set; }

    public bool Odd => (SlotNumber & 1) == 1;

    public DateTime GetTxStartTime(bool odd)
    {
      int slotNumber = SlotNumber;                                 // current slot
      if (odd != Odd) slotNumber++;                                // if rx slot, advance to next one
      else if (SecondsIntoSlot > TxLengthSeconds) slotNumber += 2; // transmission in this slot already finished, advance by 2 slots

      return Utc.Date + TimeSpan.FromSeconds(slotNumber * NativeFT4Coder.TIMESLOT_SECONDS);
    }

    private void SetUtc(DateTime value)
    {
      utc = value;
      double secondsSinceMidnight = (Utc - Utc.Date).TotalSeconds;
      SlotNumber = (int)Math.Truncate(secondsSinceMidnight / NativeFT4Coder.TIMESLOT_SECONDS);
      SlotStartSeconds = SlotNumber * NativeFT4Coder.TIMESLOT_SECONDS;
      SecondsIntoSlot = secondsSinceMidnight - SlotStartSeconds;
      SamplesIntoSlot = (int)(SecondsIntoSlot * NativeFT4Coder.SAMPLING_RATE);

      CurrentSlotStartTime = utc.Date + TimeSpan.FromSeconds(SlotStartSeconds); 
    }
  }
}
