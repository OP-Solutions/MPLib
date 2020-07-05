using System.Numerics;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    /// <summary>
    /// This message has exactly same format as <see cref="ShuffleMessage"/>
    /// </summary>
    class ReEncryptMultiKeyMessage : ShuffleMessage
    {
        public ReEncryptMultiKeyMessage(BigInteger[] cards) : base(cards)
        {
            Type = MessageType.ReEncryptMultiKey;
        }
    }
}
