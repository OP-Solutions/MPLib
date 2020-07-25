using System.Collections.Generic;
using System.Numerics;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    public class ReEncryptDeckMessage : IPokerMessage
    {
        [ProtoMember(1)]
        public IReadOnlyList<BigInteger> EncryptedCards { get; set; }

        public ReEncryptDeckMessage(IReadOnlyList<BigInteger> encryptedCards)
        {
            EncryptedCards = encryptedCards;
        }
    }
}
