using MPLib.Core.Game.General.CardGame.Messaging.MessageTypes;
using MPLib.Core.Game.Poker.Messaging.MessageTypes;
using MPLib.Models.Attibutes;

namespace MPLib.Core.Game.Poker.Messaging
{
    public enum PokerMessageType
    {
        [EnumMemberModel(typeof(RoundStampMessage))]
        RoundStamp,

        [EnumMemberModel(typeof(PokerMoveMessage))]
        Move,

        [EnumMemberModel(typeof(LeaveTableMessage))]
        LeaveTable,
    }
}
