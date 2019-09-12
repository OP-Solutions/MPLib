using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPR.Models;

namespace SPR
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
