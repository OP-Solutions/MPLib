using System;
using System.Numerics;
using System.Threading.Tasks;
using EtherBetClientLib.Models.Games;

namespace EtherBetClientLib.Core.Game.Poker.Logic
{
    public class PokerRound
    {

        #region Events

        public event Action<PokerRoundState> StateChanges;


        /// <summary>
        /// <see cref="PokerPlayer.ThisPlayerMoveCompleted"/>
        /// </summary>
        public event PokerPlayerMoveEventHandler AnyPlayerMoveCompleted;

        /// <summary>
        /// same as <see cref="AnyPlayerMoveCompleted"/>, but not fires on my player move, fires only when other player does any poker move
        /// </summary>
        public event PokerPlayerMoveEventHandler OtherPlayerMoveCompleted;

        /// <summary>
        /// same as <see cref="AnyPlayerMoveCompleted"/>, but not fires on other player move, fires only when my player does any poker move
        /// </summary>
        public event PokerPlayerMoveEventHandler MyPlayerMoveCompleted;


        /// <summary>
        /// <see cref="PokerPlayer.ThisPlayerMoveReceived"/>
        /// </summary>
        public event PokerPlayerMoveEventHandler AnyPlayerMoveReceived;

        /// <summary>
        /// same as <see cref="AnyPlayerMoveReceived"/>, but not fires on my player move, fires only when other player does any poker move
        /// </summary>
        public event PokerPlayerMoveEventHandler OtherPlayerMoveReceived;


        /// <summary>
        /// same as <see cref="AnyPlayerMoveReceived"/>, but not fires on other player move, fires only when my player does any poker move
        /// </summary>
        public event PokerPlayerMoveEventHandler MyPlayerMoveReceived;

        #endregion

        public PokerTable Table { get; internal set; }

        /// <summary>
        /// Current bet amount for single player (not sum)
        /// This value start with big blind value and can increase several time in round
        /// as some players will do "Raise"
        /// This value should be updated instantly after <see cref="MyPokerPlayer.Raise"/> is called, before cycle completed
        /// After that, if other players will "Call" or "Fold" to that raised amount, it will stay unchanged,
        /// But if someone will re-"Raise" this value will increase more instantly.
        /// </summary>
        public int CurrentBetAmount { get; }


        /// <summary>
        /// Current sum of all player bets done in this round until thi time
        /// </summary>
        public int SumOfBet { get; }


        /// <summary>
        /// Index of player whose turn to move is currently
        /// </summary>
#pragma warning disable 618
        public int CurrentPlayerIndex
        {
            get => _currentPlayerIndex;
            internal set
            {
                _currentPlayerIndex = value;
                CurrentPlayer = Table.Players[value];
            }
        }
#pragma warning restore 618

        public Player CurrentPlayer { get; private set; }

        public PokerRoundState State { get; set; }

        /// <summary>
        /// Deck card list after shuffling, cards are represented as encrypted bigIntegers
        /// </summary>
        private BigInteger[] ShuffledDeck { get; set; }

        [Obsolete("Not use this variable, instead use this: " + nameof(CurrentPlayerIndex))]
        private int _currentPlayerIndex;


        public PokerRound(PokerTable table)
        {
            Table = table;
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

        private void PlaceBlinds()
        {

        }

        private void DealCards()
        {

        }

        private void RoundFlop()
        {

        }

        private void RoundTurn()
        {

        }

        private void RoundRiver()
        {

        }

    }


    public class PokerPlayerMoveInfo
    {
        public PokerPlayerMoveType Type { get; }


        private int _fullBetAmount { get; set; }
        private int _extraBetAmount { get; set; }


        public PokerPlayerMoveInfo(PokerPlayerMoveType type, int fullBetAmount, int extraBetAmount)
        {
            Type = type;
            _fullBetAmount = fullBetAmount;
            _extraBetAmount = extraBetAmount;
        }

        /// <summary>
        /// Full bet amount of player move. this is 0 in case of "Check" and throws <see cref="InvalidOperationException"/> in case of "Fold"
        /// </summary>
        public int FullBetAmount
        {
            get
            {
                if (Type == PokerPlayerMoveType.Fold) throw new InvalidOperationException("Player can't get bet amount of move when move is \"Fold\" ");
                return _fullBetAmount;
            }
        }


        /// <summary>
        /// Extra bet amount of player move. this is 0 in case of "Check", "Call" and throws <see cref="InvalidOperationException"/> in case of "Fold"
        /// </summary>
        public int ExtraBetAmount
        {
            get
            {
                if (Type == PokerPlayerMoveType.Fold) throw new InvalidOperationException("Player can't get bet amount of move when move is \"Fold\" ");
                return _fullBetAmount;
            }
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
