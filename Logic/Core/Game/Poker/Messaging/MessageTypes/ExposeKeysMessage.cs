using System.Numerics;
using EtherBetClientLib.Core.Game.General;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    public class ExposeKeysMessage : IPokerMessage
    {
        [ProtoMember(2)]
        public PlayerKeys Keys { get; set; }
    }
}