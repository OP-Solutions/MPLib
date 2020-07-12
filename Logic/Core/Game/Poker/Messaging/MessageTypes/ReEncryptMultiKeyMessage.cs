using System.Numerics;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    /// <summary>
    /// This message has exactly same format as <see cref="ShuffleMessage"/>
    /// </summary>
    [ProtoContract]
    class ReEncryptMultiKeyMessage : ShuffleMessage
    {
        public ReEncryptMultiKeyMessage(BigInteger[] cards) : base(cards)
        {
            Type = PokerMessageType.ReEncryptMultiKey;
        }
    }
}
