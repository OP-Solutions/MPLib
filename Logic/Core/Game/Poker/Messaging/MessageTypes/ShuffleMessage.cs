using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SPR.Core.Game.Poker.Messaging.MessageTypes
{
    class ShuffleMessage : Message
    {
        public BigInteger[] Cards { get; set; }

        public ShuffleMessage(BigInteger[] cards)
        {
            Type = MessageType.Shuffle;
            Cards = cards;
        }
    }
}
