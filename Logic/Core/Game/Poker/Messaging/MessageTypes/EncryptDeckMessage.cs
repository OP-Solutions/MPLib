using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using EtherBetClientLib.Models.Games.CardGameModels;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class EncryptDeckMessage : IPokerMessage
    {

        public const PokerMessageType BoundType = PokerMessageType.EncryptSingleKey;

        public PokerMessageType Type { get; set; } = BoundType;

        [ProtoMember(1)]
        public IReadOnlyList<BigInteger> EncryptedCards { get; set; }

        public EncryptDeckMessage(IReadOnlyList<BigInteger> encryptedCards)
        {
            EncryptedCards = encryptedCards;
        }
    }
}
