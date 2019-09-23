using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SPR.Crypto.Encryption.HellMan;
using SPR.Crypto.Encryption.SRA;

namespace SPR.Models
{
    public class PokerRound
    {
        public Player[] Players { get; }
        public double[] CurrentBets { get; }
        public BigInteger[] ShuffledDeck { get; set; }

        public SraParameters MyKeys { get;  }
        public SraParameters[] MyKeys2 { get; }
        public PokerRound(Player[] players, SraParameters myKeys, SraParameters[] myKeys2)
        {
            Players = players;
            MyKeys = myKeys;
            MyKeys2 = myKeys2;
            CurrentBets = new double[players.Length];
        }
    }
}
