using System;
using System.Numerics;
using System.Threading.Tasks;
using EtherBetClientLib.Core.Game.Poker.Logic;
using EtherBetClientLib.Crypto.Encryption.SRA;
using EtherBetClientLib.Models.Games.Poker;
using EtherBetClientLib.Models.Games.Poker.Interfaces;
using EtherBetClientLib.Random;

namespace EtherBetClientLib.Models
{
    public class PokerRound
    {
        public IPokerPlayer[] Players { get; }


        /// <summary>
        /// Current bet amount for single player (not sum)
        /// This value start with big blind value and can increase several time in round
        /// as some players will do "Raise"
        /// This value should be updated instantly after <see cref="MyPokerPlayer.Raise"/> is called, before cycle completed
        /// After that, if other players will "Call" or "Fold" to that raised amount, it will stay unchanged,
        /// But if someone will re-"Raise" this value will increase more instantly.
        /// </summary>
        public int CurrentBetAmount { get; }


        public int SumOfBet { get;  }

        /// <summary>
        /// Deck card list after shuffling, cards are represented as encrypted bigIntegers
        /// </summary>
        public BigInteger[] ShuffledDeck { get; set; }

        public PokerRoundState State { get; set; }

        public PokerRound(OtherPokerPlayer[] players)
        {
            Players = players;
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
