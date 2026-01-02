
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
    public string? HisCall { get; private set; }
    public string? HisSquare { get; private set; }
    public int? MyReport { get; private set; }
    public int? HisReport { get; private set; }

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
      HisCall = null;
      HisSquare = null;
      HisReport = null;
      GenerateMessages();
      MessageType = Ft4MessageType.CQ;
    }

    public bool IsMessageAvailable(Ft4MessageType type)
    {
      return messages[(int)type] != null;
    }

    // messages received
    public bool ProcessMessages(IEnumerable<DecodedItem> items)
    {

      return false;
    }

    // user clicked on received message
    public void ReplyToMessage(DecodedItem item)
    {

    }

    // user clicked on a message type button
    public void ForceMessage(Ft4MessageType messageType)
    {

    }

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

    private void GenerateMessages()
    {
      messages[(int)Ft4MessageType.CQ] = $"CQ {myCall} {mySquare}";

      if (HisCall != null)
      {
        messages[(int)Ft4MessageType.DE] = $"{HisCall} {myCall} {mySquare}";
        messages[(int)Ft4MessageType.dB] = $"{HisCall} {myCall} {MyReport}";
        messages[(int)Ft4MessageType.R_dB] = $"{HisCall} {myCall} R{MyReport}";
        messages[(int)Ft4MessageType.RR73] = $"{HisCall} {myCall} RR73";
        messages[(int)Ft4MessageType._73] = $"{HisCall} {myCall} 73";
      }
      else
        for (Ft4MessageType i = Ft4MessageType.Unknown; i <= Ft4MessageType._73; i++) 
          messages[(int)i] = null;
    }
  }
}
