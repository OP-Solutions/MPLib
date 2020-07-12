namespace EtherBetClientLib.Core.Game.Poker.Messaging
{
    public enum PokerMessageType
    {
        RoundStamp,
        Shuffle,
        ReEncryptMultiKey,
        GetKey,
        Bet,
        LeaveTable,
        ReShuffle,
        ReEncryptSingleKey,
        KeysExpose,
    }
}
