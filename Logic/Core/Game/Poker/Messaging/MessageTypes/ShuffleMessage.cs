using System.Numerics;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    [ProtoContract]
    class ShuffleMessage : PokerMessage
    {
        [ProtoMember(1)]
        public BigInteger[] Cards { get; set; }

        public ShuffleMessage(BigInteger[] cards)
        {
            Type = PokerMessageType.Shuffle;
            Cards = cards;
        }
    }
}
