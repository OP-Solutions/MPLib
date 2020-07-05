namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class RoundStampMessage : Message
    {
        /// <summary>
        /// Unique Identifier Of Table this round belongs to
        /// </summary>
        public string TableUuid { get; set; }

        /// <summary>
        /// Unique Identifier Of Round
        /// </summary>
        public string RoundUuid { get; set; }

        /// <summary>
        /// Array Of Players, whose will play in corresponding round
        /// </summary>
        public string[] Players { get; set; }

        public RoundStampMessage(string tableUuid, string roundUuid, string[] players)
        {
            Type = MessageType.RoundStamp;
            TableUuid = tableUuid;
            RoundUuid = roundUuid;
            Players = players;
        }
    }
}
