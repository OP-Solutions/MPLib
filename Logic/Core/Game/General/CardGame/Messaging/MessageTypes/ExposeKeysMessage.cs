using MPLib.Core.Game.Poker.Messaging;
using ProtoBuf;

namespace MPLib.Core.Game.General.CardGame.Messaging.MessageTypes
{
    public class ExposeKeysMessage : IPokerMessage
    {
        [ProtoMember(2)]
        public PlayerKeys Keys { get; set; }
    }
}