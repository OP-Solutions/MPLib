using System.Numerics;
using EtherBetClientLib.Models.Games.CardGameModels;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    /// <summary>
    /// This message has exactly same format as <see cref="ShuffleMessage"/>
    /// </summary>
    [ProtoContract]
    class ReEncryptMultiKeyMessage : IPokerMessage
    {
        public const PokerMessageType BoundType = PokerMessageType.Bet;

        public PokerMessageType Type { get; set; } = BoundType;

        [ProtoMember(1)]
        public BigInteger[] Cards { get; set; }
    }
}
