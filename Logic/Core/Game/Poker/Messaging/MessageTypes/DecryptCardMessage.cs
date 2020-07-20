using System.Numerics;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class DecryptCardMessage : IPokerMessage
    {
        public const PokerMessageType BoundType = PokerMessageType.DecryptCard;

        public PokerMessageType Type { get; set; } = BoundType;

        public BigInteger Card { get; set; }
    }
}
