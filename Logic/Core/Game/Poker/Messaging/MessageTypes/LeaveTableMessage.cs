using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    [ProtoContract]
    class LeaveTableMessage : IPokerMessage
    {
        public const PokerMessageType BoundType = PokerMessageType.LeaveTable;

        public PokerMessageType Type { get; set; } = BoundType;

    }
}
