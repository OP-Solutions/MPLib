using MPLib.Core.Game.General.CardGame.Messaging.MessageTypes;
using MPLib.Core.Game.Poker.Messaging.MessageTypes;
using MPLib.Models.Attibutes;

namespace MPLib.Core.Game.General.CardGame.Messaging
{
    public enum CardGameMessageType
    {
        [EnumMemberModel(typeof(EncryptShuffleDeckMessage))]
        EncryptShuffleDeck,

        [EnumMemberModel(typeof(ReEncryptDeckMessage))]
        ReEncryptDeck,

        [EnumMemberModel(typeof(SingleKeyExposeMessage))]
        SingleKeyExpose,

        [EnumMemberModel(typeof(ExposeKeysMessage))]
        ExposeKeys,

        [EnumMemberModel(typeof(ReshuffleMessage))]
        ReShuffle,
    }
}
