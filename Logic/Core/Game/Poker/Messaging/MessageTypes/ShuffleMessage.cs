using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Math;

namespace SPR.Core.Game.Poker.Messaging.MessageTypes
{
    public class ShuffleMessage : Message
    {
        public BigInteger[] Cards;
    }
}
