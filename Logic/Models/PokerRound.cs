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
        public PokerTable Table { get; }
        public List<double> CurrentBets { get; }
        public List<BigInteger> ShuffledDeck { get; }

        public SraParameters MyKeys { get; }
    }
}
