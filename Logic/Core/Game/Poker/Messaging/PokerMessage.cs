using EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging
{
    [ProtoInclude(100, typeof(RoundStampMessage))]
    [ProtoInclude(101, typeof(ShuffleMessage))]
    [ProtoInclude(102, typeof(ReEncryptMultiKeyMessage))]
    [ProtoInclude(102, typeof(GetKeyMessage))]
    [ProtoInclude(102, typeof(BetMessage))]
    [ProtoInclude(102, typeof(LeaveTableMessage))]
    public class PokerMessage : IMessage
    {
        public PokerMessageType Type { get; set; }
    }
}
