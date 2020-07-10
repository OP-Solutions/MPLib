namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class LeaveTableMessage : PokerMessage
    {
        public LeaveTableMessage()
        {
            Type = MessageType.LeaveTable;
        }
    }
}
