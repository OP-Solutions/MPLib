using System.Numerics;
using EtherBetClientLib.Crypto.Encryption.SRA;

namespace EtherBetClientLib.Models
{
    public class PokerRound
    {
        public PlayerBase[] Players { get; }
        public int[] CurrentBets { get; }

        /// <summary>
        /// Deck card list after shuffling, cards are represented as encrypted bigIntegers
        /// </summary>
        public BigInteger[] ShuffledDeck { get; set; }

        public SraParameters MyKeys { get;  }
        public SraParameters[] MyKeys2 { get; }

        
        public PokerRound(PlayerBase[] players, SraParameters myKeys, SraParameters[] myKeys2)
        {
            Players = players;
            MyKeys = myKeys;
            MyKeys2 = myKeys2;
            CurrentBets = new double[players.Length];
        }
    }

    public enum PokerRoundState
    {
        NoStarted, 
        DeckShuffled, 
        SmallBlindPlaced, 
        BigBlindPlaced, 
        CardsDealDone, 
        Flop,
        Turn,
        River,
        Completed,
    }
}
