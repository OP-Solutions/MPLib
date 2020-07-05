namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class GetKeyMessage : Message
    {
        /// <summary>
        /// 0-based index of card
        /// </summary>
        public int CardIndex { get; set; }
    }
}
