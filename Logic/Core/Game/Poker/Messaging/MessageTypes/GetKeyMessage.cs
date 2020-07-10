namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class GetKeyMessage : PokerMessage
    {
        /// <summary>
        /// 0-based index of card
        /// </summary>
        public int CardIndex { get; set; }
    }
}
