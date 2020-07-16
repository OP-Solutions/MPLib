using System.Numerics;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    [ProtoContract]
    class ShuffleMessage : IPokerMessage
    {
        public const PokerMessageType BoundType = PokerMessageType.Shuffle;

        public PokerMessageType Type { get; set; } = BoundType;

        [ProtoMember(1)]
        public BigInteger[] Cards { get; set; }

        public ShuffleMessage(BigInteger[] cards)
        {
            Cards = cards;
        }
    }
}
