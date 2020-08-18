using MPLib.Core.Game.General.CardGame.Messaging.MessageTypes;
using MPLib.Models.Attibutes;

namespace MPLib.Models.Games.CardGames.Messaging
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
