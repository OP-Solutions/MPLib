using EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging
{
    [ProtoInclude((int)PokerMessageType.RoundStamp, typeof(RoundStampMessage))]
    [ProtoInclude((int)PokerMessageType.Shuffle, typeof(EncryptDeckMessage))]
    [ProtoInclude((int)PokerMessageType.SingleKeyExpose, typeof(SingleKeyExposeMessage))]
    [ProtoInclude((int)PokerMessageType.Bet, typeof(BetMessage))]
    [ProtoInclude((int)PokerMessageType.LeaveTable, typeof(LeaveTableMessage))]
    public interface IPokerMessage : IMessage
    {
        public PokerMessageType Type { get; set; }
    }
}
