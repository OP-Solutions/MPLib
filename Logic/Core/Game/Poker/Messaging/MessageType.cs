namespace EtherBetClientLib.Core.Game.Poker.Messaging
{
    public enum MessageType
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
