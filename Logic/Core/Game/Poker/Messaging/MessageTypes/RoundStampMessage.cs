using ProtoBuf;

namespace MPLib.Core.Game.Poker.Messaging.MessageTypes
{
    [ProtoContract]
    class RoundStampMessage : IPokerMessage
    {
        /// <summary>
        /// Unique Identifier Of Table this round belongs to
        /// </summary>
        [ProtoMember(1)]
        public string TableUuid { get; set; }

        /// <summary>
        /// Unique Identifier Of Round
        /// </summary>
        [ProtoMember(2)]
        public string RoundUuid { get; set; }

        /// <summary>
        /// Array Of Players, whose will play in corresponding round
        /// String value is public key of player
        /// </summary>
        [ProtoMember(3)]
        public string[] Players { get; set; }

        public RoundStampMessage(string tableUuid, string roundUuid, string[] players)
        {
            TableUuid = tableUuid;
            RoundUuid = roundUuid;
            Players = players;
        }
    }
}
