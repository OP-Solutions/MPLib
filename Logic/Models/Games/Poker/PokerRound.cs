using System;
using System.Numerics;
using System.Threading.Tasks;
using EtherBetClientLib.Crypto.Encryption.SRA;
using EtherBetClientLib.Models.Games.Poker;
using EtherBetClientLib.Random;

namespace EtherBetClientLib.Models
{
    public class PokerRound
    {
        public PokerPlayer[] Players { get; }


        public int CurrentBetAmount { get; }

        public int[] CurrentBets { get; }

        /// <summary>
        /// Deck card list after shuffling, cards are represented as encrypted bigIntegers
        /// </summary>
        public BigInteger[] ShuffledDeck { get; set; }

        public SraParameters MyKey { get;  }

        public PokerRoundState State { get; set; }

        public PokerRound(PokerPlayer[] players, SraParameters myKey)
        {
            Players = players;
            MyKey = myKey;
            CurrentBets = new int[players.Length];
            State = PokerRoundState.NoStarted;
        }

        /// <summary>
        /// Core method in poker logic. This method represents lifetime of poker round:
        /// Round is started by calling that method and ended when this method exits
        /// </summary>
        /// <returns></returns>
        public async Task ProcessRound()
        {
            State = PokerRoundState.BeforeDeckShuffle;
            throw new NotImplementedException();
        }
    }

    public enum PokerRoundState
    {
        NoStarted,
        BeforeDeckShuffle,
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
