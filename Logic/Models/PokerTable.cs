using System.Collections.Generic;

namespace SPR.Models
{
    public class PokerTable
    {
        public List<Player> Players { get; set; }

        public List<double> Balances { get; set; }

        public int CurSmallBlindIndex { get; set; }
        public double SmallBlind { get; }


        public PokerTable(List<Player> players)
        {
            Players = players;
        }

    }
}
