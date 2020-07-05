namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class LeaveTableMessage : Message
    {
        public LeaveTableMessage()
        {
            Type = MessageType.LeaveTable;
        }
    }
}
