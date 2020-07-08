using System;
using System.Collections.Generic;
using System.Text;

namespace EtherBetClientLib.Models.Games.Poker
{
    public class PokerTable : GameTableBase
    {
        public double CurrentSmallBlind { get; private set; }
        public int CurrentSmallBlindPlayerIndex { get; set; }

        /// <summary>
        /// Time when current small blind will be doubled
        /// </summary>
        public DateTime SmallBlindDoubleTime { get; set; }

        public PokerTable(List<Player> players) : base(players)
        {
        }
    }
}
