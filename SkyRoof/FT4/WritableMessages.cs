using WsjtxUtils.WsjtxMessages;
using WsjtxUtils.WsjtxMessages.Messages;
using System;

namespace SkyRoof
{
  internal class WritableStatus : Status, IWsjtxDirectionIn
  {
    public override void WriteMessage(WsjtxMessageWriter messageWriter)
    {
      base.WriteMessage(messageWriter);
      messageWriter.WriteUInt64(DialFrequencyInHz);
      messageWriter.WriteString(Mode);
      messageWriter.WriteString(DXCall);
      messageWriter.WriteString(Report);
      messageWriter.WriteString(TXMode);
      messageWriter.WriteBool(TXEnabled);
      messageWriter.WriteBool(Transmitting);
      messageWriter.WriteBool(Decoding);
      messageWriter.WriteUInt32(RXOffsetFrequencyHz);
      messageWriter.WriteUInt32(TXOffsetFrequencyHz);
      messageWriter.WriteString(DECall);
      messageWriter.WriteString(DEGrid);
      messageWriter.WriteString(DXGrid);
      messageWriter.WriteBool(TXWatchdog);
      messageWriter.WriteString(SubMode);
      messageWriter.WriteBool(FastMode);
      messageWriter.WriteEnum(SpecialOperationMode);
      messageWriter.WriteUInt32(FrequencyTolerance);
      messageWriter.WriteUInt32(TRPeriod);
      messageWriter.WriteString(ConfigurationName);
      messageWriter.WriteString(TXMessage);
    }
  }

  internal class WritableDecode : Decode, IWsjtxDirectionIn
  {
    public override void WriteMessage(WsjtxMessageWriter messageWriter)
    {
      base.WriteMessage(messageWriter);
      messageWriter.WriteBool(New);
      messageWriter.WriteUInt32(Time);
      messageWriter.WriteInt32(Snr);
      messageWriter.WriteDouble(OffsetTimeSeconds);
      messageWriter.WriteUInt32(OffsetFrequencyHz);
      messageWriter.WriteString(Mode);
      messageWriter.WriteString(Message);
      messageWriter.WriteBool(LowConfidence);
      messageWriter.WriteBool(OffAir);
    }
  }

  internal class WritableQsoLogged : QsoLogged, IWsjtxDirectionIn
  {
    public override void WriteMessage(WsjtxMessageWriter messageWriter)
    {
      base.WriteMessage(messageWriter);
      messageWriter.WriteQDateTime(DateTimeOff);
      messageWriter.WriteString(DXCall);
      messageWriter.WriteString(DXGrid);
      messageWriter.WriteUInt64(TXFrequencyInHz);
      messageWriter.WriteString(Mode);
      messageWriter.WriteString(ReportSent);
      messageWriter.WriteString(ReportReceived);
      messageWriter.WriteString(TXPower);
      messageWriter.WriteString(Comments);
      messageWriter.WriteString(Name);
      messageWriter.WriteQDateTime(DateTimeOn);
      messageWriter.WriteString(OperatorCall);
      messageWriter.WriteString(MyCall);
      messageWriter.WriteString(MyGrid);
      messageWriter.WriteString(ExchangeSent);
      messageWriter.WriteString(ExchangeReceived);
      messageWriter.WriteString(AdifPropagationMode);
    }
  }

  internal static class WsjtxMessageWriterExtensions
  {
    public static void WriteQDateTime(this WsjtxMessageWriter writer, DateTime dateTime)
    {
      DateTime utc = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

      long julianDay = (long)Math.Floor(utc.Date.ToOADate()) + 2415019;
      writer.WriteInt64(julianDay);

      uint milliseconds = (uint)utc.TimeOfDay.TotalMilliseconds;
      writer.WriteUInt32(milliseconds);

      writer.WriteEnum(Timespec.UTC);
    }
  }
}