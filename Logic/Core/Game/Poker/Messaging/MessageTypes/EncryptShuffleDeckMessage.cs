using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using EtherBetClientLib.Models.Games.CardGameModels;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class EncryptShuffleDeckMessage : IPokerMessage
    {

        public const PokerMessageType BoundType = PokerMessageType.EncryptShuffleDeck;

        public PokerMessageType Type { get; set; } = BoundType;

        [ProtoMember(1)]
        public IReadOnlyList<BigInteger> EncryptedCards { get; set; }

        public EncryptShuffleDeckMessage(IReadOnlyList<BigInteger> encryptedCards)
        {
            EncryptedCards = encryptedCards;
        }
    }
}
