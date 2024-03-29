﻿using System.Collections.Generic;
using System.Numerics;
using MPLib.Core.Game.Poker.Messaging;
using MPLib.Models.Games.CardGames.Messaging;
using ProtoBuf;

namespace MPLib.Core.Game.General.CardGame.Messaging.MessageTypes
{
    public class ReEncryptDeckMessage : ICardGameMessage
    {
        [ProtoMember(1)]
        public IReadOnlyList<BigInteger> EncryptedCards { get; set; }

        public ReEncryptDeckMessage(IReadOnlyList<BigInteger> encryptedCards)
        {
            EncryptedCards = encryptedCards;
        }

        public ReEncryptDeckMessage()
        {

        }
    }
}
