using System.Numerics;
using EtherBetClientLib.Core.Game.General;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    public class ExposeKeysMessage : IPokerMessage
    {
        public const PokerMessageType BoundType = PokerMessageType.ExposeKeys;

        public PokerMessageType Type { get; set; } = BoundType;

       
        [ProtoMember(2)]
        public PlayerKeys Keys { get; set; }
    }
}