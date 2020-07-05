using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Asn1.Cms;

namespace EtherBetClientLib.Models.Games.Poker
{
    public class PokerTableOptions
    {
        public double InitialSmallBlind { get; set; }
        public double MaxPlayerCount { get; set; }

        public TimeSpan MoveTimeout { get; set; }
    }
}
