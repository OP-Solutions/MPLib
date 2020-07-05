using System;
using System.Collections.Generic;
using System.Text;

namespace EtherBetClientLib.Models.Games.Poker
{
    class PokerTable : GameTableBase
    {
        public double CurrentSmallBlind { get; private set; }
        public int CurrentSmallBlindPlayerIndex { get; set; }

        public PokerTable(List<Player> players) : base(players)
        {
        }
    }
}
