using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using EtherBetClientLib.Models.Games.CardGameModels;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class EncryptShuffleMessage : IPokerMessage
    {
        public const PokerMessageType BoundType = PokerMessageType.EncryptSingleKey;

        public PokerMessageType Type { get; set; } = BoundType;

        [ProtoMember(1)]
        public BigInteger[] EncryptedCards { get; set; }
    }
}
