using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    [ProtoContract]
    class LeaveTableMessage : PokerMessage
    {
        public LeaveTableMessage()
        {
            Type = PokerMessageType.LeaveTable;
        }
    }
}
