using MPLib.Core.Game.General.CardGame.Messaging.MessageTypes;
using MPLib.Core.Game.Poker.Messaging.MessageTypes;
using MPLib.Models.Attibutes;

namespace MPLib.Core.Game.Poker.Messaging
{
    public enum PokerMessageType
    {
        [EnumMemberModel(typeof(RoundStampMessage))]
        RoundStamp = 100,

        [EnumMemberModel(typeof(BetMessage))]
        Bet,

        [EnumMemberModel(typeof(FoldMessage))]
        Fold,

        [EnumMemberModel(typeof(LeaveTableMessage))]
        LeaveTable,
    }
}
