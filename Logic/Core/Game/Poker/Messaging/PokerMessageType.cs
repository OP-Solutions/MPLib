namespace EtherBetClientLib.Core.Game.Poker.Messaging
{
    public enum PokerMessageType
    {
        RoundStamp = 100,
        Shuffle,
        EncryptSingleKey,
        ReEncryptMultiKey,
        KeyExpose,
        Bet,
        Fold,
        FullKeysExpose,
        LeaveTable = Fold + 1,
        ReShuffle,
    }
}
