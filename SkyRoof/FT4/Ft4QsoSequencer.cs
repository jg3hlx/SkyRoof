
using System.Diagnostics.Eventing.Reader;
using static VE3NEA.NativeFT4Coder;

namespace SkyRoof
{
  // meaningful names for the WsjtxQsoState enum
  public enum Ft4MessageType {Unknown, DE, dB, R_dB, RR73, _73, CQ }

  internal class Ft4QsoSequencer
  {
    private string mySquare;
    private string myCall;
    private string?[] messages = new string[7];

    public string MyCall { get => myCall; set => SetMyCall(value); }
    public string MySquare{ get => mySquare; set => SetMySquare(value); }

    private string? LastHisCall;

    public string? HisCall { get; private set; }
    public string? HisSquare { get; private set; }
    public int? MySnr { get; private set; }
    public int? HisSnr { get; private set; }

    public Ft4MessageType MessageType { get; private set; }
    public string? Message => messages[(int)MessageType];


    public Ft4QsoSequencer(string call, string square)
    {
      myCall = call;
      mySquare = square[..4];
      Reset();
    }

    public void Reset()
    {
      LastHisCall = HisCall;

      HisCall = null;
      HisSquare = null;
      HisSnr = null;
      MySnr = null;
      GenerateMessages();
      MessageType = Ft4MessageType.CQ;
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                           update state
    //--------------------------------------------------------------------------------------------------------------

    // messages received
    // called only if DXCallsign == MyCall, and Sender is in TX mode
    public bool ProcessReceivedMessage(DecodedItem item)
    {
      // extract info from his message
      var hisMessage = (Ft4MessageType)item.Parse.QsoState;
      HisCall = item.Parse.DECallsign;
      HisSnr = item.Decode.Snr;
      if (item.Parse.DXCallsign == MyCall &&
        !string.IsNullOrEmpty(item.Parse.Report) &&
        item.Parse.Report != "RR73")
        MySnr = ReportToInt(item.Parse.Report);
      if (!string.IsNullOrEmpty(item.Parse.GridSquare)) HisSquare = item.Parse.GridSquare;

      GenerateMessages();

      // I am calling him
      if (item.Parse.DXCallsign != MyCall)
        MessageType = Ft4MessageType.DE;

      // I am replying to his message
      else

        switch (hisMessage)
        {
          case Ft4MessageType.CQ:
            MessageType = Ft4MessageType.DE;
            break;

          case Ft4MessageType.Unknown:
          case Ft4MessageType.DE:
          case Ft4MessageType._73:
            MessageType = Ft4MessageType.dB;
            break;

          default:
            MessageType = hisMessage + 1;
            break;
        }
      return true;
    }

    // user clicked on a message type button
    public bool ForceMessage(Ft4MessageType messageType)
    {
      if (!IsMessageAvailable(messageType)) return false;
      MessageType = messageType;
      return true;
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                           helper funcs
    //--------------------------------------------------------------------------------------------------------------
    private void SetMyCall(string value)
    {
      myCall = value;
      GenerateMessages();
    }

    private void SetMySquare(string value)
    {
      mySquare = value[..4];
      GenerateMessages();
    }

    private int? ReportToInt(string report)
    {
      if (report.StartsWith("R")) report = report[1..];
      return int.Parse(report);
    }

    private string IntToReport(int snr)
    {
      snr = Math.Min(49, Math.Max(-24, snr));
      string result = $"{snr,2}";
      if (snr > 0) result = "+" + result;
      return result;
    }

    private void GenerateMessages()
    {
      messages[(int)Ft4MessageType.CQ] = $"CQ {myCall} {mySquare}";

      if (HisCall != null)
      {
        messages[(int)Ft4MessageType.DE] = $"{HisCall} {myCall} {mySquare}";
        messages[(int)Ft4MessageType.dB] = $"{HisCall} {myCall} {IntToReport((int)HisSnr!)}";
        messages[(int)Ft4MessageType.R_dB] = $"{HisCall} {myCall} R{IntToReport((int)HisSnr!)}";
        messages[(int)Ft4MessageType.RR73] = $"{HisCall} {myCall} RR73";
        messages[(int)Ft4MessageType._73] = $"{HisCall} {myCall} 73";
      }
      else
      {
        for (Ft4MessageType i = Ft4MessageType.Unknown; i <= Ft4MessageType._73; i++)
          messages[(int)i] = null;

        if (LastHisCall != null) messages[(int)Ft4MessageType._73] = $"{LastHisCall} {myCall} 73";
      }
    }

    public bool IsMessageAvailable(Ft4MessageType type)
    {
      return messages[(int)type] != null;
    }
  }
}
