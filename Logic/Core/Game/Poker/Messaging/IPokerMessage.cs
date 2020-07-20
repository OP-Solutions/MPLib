using EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging
{
    [ProtoInclude((int)PokerMessageType.RoundStamp, typeof(RoundStampMessage))]
    [ProtoInclude((int)PokerMessageType.Shuffle, typeof(EncryptShuffleMessage))]
    [ProtoInclude((int)PokerMessageType.KeyExpose, typeof(KeyExposeMessage))]
    [ProtoInclude((int)PokerMessageType.Bet, typeof(BetMessage))]
    [ProtoInclude((int)PokerMessageType.LeaveTable, typeof(LeaveTableMessage))]
    public interface IPokerMessage : IMessage
    {
        public PokerMessageType Type { get; set; }
    }
}
