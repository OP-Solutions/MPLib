using MPLib.Core.Game.Poker.Messaging;
using MPLib.Crypto.Encryption.SRA;
using MPLib.Models.Games.CardGames.Messaging;
using ProtoBuf;

namespace MPLib.Core.Game.General.CardGame.Messaging.MessageTypes
{
    [ProtoContract]
    class SingleKeyExposeMessage : ICardGameMessage
    {
        [ProtoMember(1)]
        public int CardIndex { get; set; }

        /// <summary>
        /// 0-based index of card
        /// </summary>
        [ProtoMember(2)]
        public SraParameters Key { get; set; }
    }
}
