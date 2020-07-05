using System.Numerics;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class ShuffleMessage : Message
    {
        public BigInteger[] Cards { get; set; }

        public ShuffleMessage(BigInteger[] cards)
        {
            Type = MessageType.Shuffle;
            Cards = cards;
        }
    }
}
