using System.Numerics;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    [ProtoContract]
    class KeyExposeMessage : IPokerMessage
    {
        public const PokerMessageType BoundType = PokerMessageType.KeyExpose;

        public PokerMessageType Type { get; set; } = BoundType;


        [ProtoMember(1)]
        public int CardIndex { get; set; }

        /// <summary>
        /// 0-based index of card
        /// </summary>
        [ProtoMember(2)]
        public BigInteger Key { get; set; }
    }
}
