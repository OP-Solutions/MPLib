namespace EtherBetClientLib.Core.Game.Poker.Messaging
{
    public enum PokerMessageType
    {
        RoundStamp = 100,
        Shuffle,
        EncryptSingleKey,
        DecryptCard,
        SingleKeyExpose,
        Bet,
        Fold,
        ExposeKeys,
        LeaveTable,
        ReShuffle,
    }
}
