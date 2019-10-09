using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPR.Core.Game.Poker.Messaging.MessageTypes
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
