using System.Numerics;
using EtherBetClientLib.Crypto.Encryption.SRA;

namespace EtherBetClientLib.Models
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
