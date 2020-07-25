using EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes;
using EtherBetClientLib.Models;

namespace EtherBetClientLib.Core.Game.Poker.Messaging
{
    public enum PokerMessageType
    {
        [EnumMemberModel(typeof(RoundStampMessage))]
        RoundStamp = 100,

        [EnumMemberModel(typeof(EncryptShuffleDeckMessage))]
        EncryptShuffleDeck,

        [EnumMemberModel(typeof(ReEncryptDeckMessage))]
        ReEncryptDeck,

        [EnumMemberModel(typeof(DecryptCardMessage))]
        DecryptCard,

        [EnumMemberModel(typeof(SingleKeyExposeMessage))]
        SingleKeyExpose,

        [EnumMemberModel(typeof(BetMessage))]
        Bet,

        [EnumMemberModel(typeof(FoldMessage))]
        Fold,

        [EnumMemberModel(typeof(ExposeKeysMessage))]
        ExposeKeys,

        [EnumMemberModel(typeof(LeaveTableMessage))]
        LeaveTable,

        ReShuffle,
    }
}
