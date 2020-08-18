using MPLib.Core.Game.Poker.Messaging;
using MPLib.Models.Games.CardGames;
using MPLib.Models.Games.CardGames.Messaging;
using ProtoBuf;

namespace MPLib.Core.Game.General.CardGame.Messaging.MessageTypes
{
    public class ExposeKeysMessage : ICardGameMessage
    {
        [ProtoMember(2)]
        public CardEncryptionKeys Keys { get; set; }
    }
}