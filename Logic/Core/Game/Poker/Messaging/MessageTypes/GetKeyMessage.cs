using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    [ProtoContract]
    class GetKeyMessage : PokerMessage
    {
        /// <summary>
        /// 0-based index of card
        /// </summary>
        [ProtoMember(1)]
        public int CardIndex { get; set; }
    }
}
