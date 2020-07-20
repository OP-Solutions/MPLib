namespace EtherBetClientLib.Core.Game.Poker.Messaging
{
    public enum PokerMessageType
    {
        RoundStamp = 100,
        Shuffle,
        EncryptSingleKey,
        DecryptCard,
        KeyExpose,
        Bet,
        Fold,
        FullKeysExpose,
        LeaveTable,
        ReShuffle,
    }
}
