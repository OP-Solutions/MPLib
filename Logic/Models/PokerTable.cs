using System.Collections.Generic;

namespace SPR.Models
{
    class PokerTable
    {
        public List<Player> Players { get; set; }
        public int CurSmallBlind { get; set; }
        public double MinBet { get; }


        public PokerTable(List<Player> players)
        {
            Players = players;
        }

    }
}
