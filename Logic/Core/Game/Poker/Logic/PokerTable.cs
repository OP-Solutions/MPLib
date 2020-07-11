using System;
using System.Collections.Generic;
using EtherBetClientLib.Models.Games;

namespace EtherBetClientLib.Core.Game.Poker.Logic
{
    public class PokerTable : GameTableBase
    {
        public int CurrentSmallBlind { get; private set; }
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
