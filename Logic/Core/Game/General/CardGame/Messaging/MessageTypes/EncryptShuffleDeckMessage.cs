using System.Collections.Generic;
using System.Numerics;
using MPLib.Core.Game.Poker.Messaging;
using ProtoBuf;

namespace MPLib.Core.Game.General.CardGame.Messaging.MessageTypes
{
    class EncryptShuffleDeckMessage : ICardGameMessage
    {
        [ProtoMember(1)]
        public IReadOnlyList<BigInteger> EncryptedCards { get; set; }

        public EncryptShuffleDeckMessage(IReadOnlyList<BigInteger> encryptedCards)
        {
            EncryptedCards = encryptedCards;
        }

        public EncryptShuffleDeckMessage()
        {

        }
    }
}
