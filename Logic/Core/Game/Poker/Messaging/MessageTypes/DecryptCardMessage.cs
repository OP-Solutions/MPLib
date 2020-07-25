using System.Numerics;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class DecryptCardMessage : IPokerMessage
    {
        public BigInteger Card { get; set; }
    }
}
