using System;
using System.Collections.Generic;
using System.Text;

namespace EtherBetClientLib.Models.Games.Poker
{
    public class PokerTableOptions
    {
        public double InitialSmallBlind { get; set; }
        public double MaxPlayerCount { get; set; }

        public TimeSpan MoveTimeout { get; set; }
    }
}
