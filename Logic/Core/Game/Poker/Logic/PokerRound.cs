using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using MPLib.Core.Game.General.CardGame;
using MPLib.Core.Game.General.CardGame.Messaging;
using MPLib.Core.Game.Poker.Messaging;
using MPLib.Core.Game.Poker.Messaging.MessageTypes;
using MPLib.Models.Games;
using MPLib.Models.Games.CardGameModels;
using MPLib.Models.Games.Poker;
using MPLib.Networking;

namespace MPLib.Core.Game.Poker.Logic
{

    /// <summary>
    /// This class contains logic of poker game round (dealing cards, betting, etcc..)
    /// TODO: Note that, Implementation of this class is halfway!
    /// </summary>
    public class PokerRound
    {

        private static readonly MessageManagerBuilder _messageManagerBuilder = 
            new MessageManagerBuilder(PlayerIdentifyMode.Index)
                .WithMessageTypes<PokerMessageType>()
                .WithMessageTypes<CardGameMessageType>();

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


        /// <summary>
        /// Players that are playing in this round. Players should be ordered as they are on table and
        /// first player in this list should be small blind of this round, next - big blind, etc..
        /// Note that this may not be same player list as <seealso cref="PokerTable.Players"/> (not just reordered),
        /// because some player may be joined on table but not playing in this round.
        /// For example if player just joined table, it will not be in current round but will start playing from next round
        /// </summary>
        public IReadOnlyList<PokerPlayer> Players { get; }

        public MyPokerPlayer MyPlayer { get; }

        public int SmallBlind { get; }

        /// <summary>
        /// Current bet amount for single player (not sum)
        /// This value start with big blind value and can increase several time in round
        /// as some players will do "Raise"
        /// This value should be updated instantly after <see cref="MyPokerPlayer.Raise"/> is called, before cycle completed
        /// After that, if other players will "Call" or "Fold" to that raised amount, it will stay unchanged,
        /// But if someone will re-"Raise" this value will increase more instantly.
        /// </summary>
        public int CurrentBetAmount { get; private set; }


        /// <summary>
        /// Current sum of all player bets done in this round until this time
        /// </summary>
        public int SumOfBet { get; private set; }


        /// <summary>
        /// Index of player whose turn to move is currently
        /// </summary>
        public int CurrentPlayerIndex { get; internal set; }

        /// <summary>
        /// Player whose turn to move is currently
        /// </summary>
        public Player CurrentPlayer => Players[CurrentPlayerIndex];

        public IReadOnlyList<Card> PublicCards => _publicCards;

        
        public PokerRoundState State { get; set; }


        private readonly IPlayerMessageManager<IPokerMessage> _messageManager;
        private readonly CardManager<PokerPlayer, MyPokerPlayer> _cardManager;
        private List<Card> _publicCards;


        public PokerRound(int smallBlind, IReadOnlyList<PokerPlayer> players)
        {
            Players = players;
            State = PokerRoundState.NoStarted;
            SmallBlind = smallBlind;
            var messageManager = _messageManagerBuilder.Build<IMessage>(Players);
            _messageManager = messageManager;
            _cardManager = new CardManager<PokerPlayer, MyPokerPlayer>(messageManager, PokerGameData.CardDeck, Players, MyPlayer);
        }

        /// <summary>
        /// Core method in poker logic. This method represents lifetime of poker round:
        /// Round is started by calling that method and ended when this method exits
        /// </summary>
        /// <returns></returns>
        public async Task ProcessRound()
        {
            State = PokerRoundState.BeforeDeckShuffle;
            await _cardManager.ShuffleAsync();
            State = PokerRoundState.DeckShuffled;
            await DealPlayerCards();

            
            await SingleRoundCycle();

            var curCardIndex = await DealFlop(); // next card index in the remaining deck
        }

        private async Task SingleRoundCycle(bool isFirstCycle = false)
        {

            IEnumerable<PokerPlayer> players = Players;

            if (isFirstCycle)
            {
                var bigBlind = SmallBlind * 2;
                var smallBlindPlayer = Players[0];
                var bigBlindPlayer = Players[1];
                smallBlindPlayer.CurrentBetAmount = SmallBlind;
                bigBlindPlayer.CurrentBetAmount = bigBlind; // big blind 

                CurrentBetAmount = bigBlind;
                SumOfBet = SmallBlind + bigBlind;
                players = Players.Skip(2); // skip small and big blind player since they already betted
            }

            foreach (var player in players)
            {
                var move = await _messageManager.ReadMessageFrom<PokerMoveMessage>(player);
                switch (move.Type)
                {
                    case PokerPlayerMoveType.Fold:
                        player.State = PokerPlayerState.Fold;
                        continue;
                    case PokerPlayerMoveType.Call:
                        continue;
                    case PokerPlayerMoveType.Raise:
                        var extraBetAmount = move.FullBetAmount - (CurrentBetAmount - player.CurrentBetAmount);
                        CurrentBetAmount += extraBetAmount;
                        break;
                }

                player.CurrentBetAmount += move.FullBetAmount;
                SumOfBet += move.FullBetAmount;
            }

            throw new NotImplementedException();
        }

        private async Task<int> DealFlop()
        {

            var curCardIndex = Players.Count * 2;

            // ReSharper disable once RedundantAssignment
            curCardIndex++; //skip 1 card before opening


            await _cardManager.OpenPublicCardAsync(curCardIndex++);
            await _cardManager.OpenPublicCardAsync(curCardIndex++);
            await _cardManager.OpenPublicCardAsync(curCardIndex);
            return curCardIndex;
        }


        private async Task<Card[]> DealPlayerCards()
        {
            var tasks = new List<Task>();
            Task<Card> myCard1Task = null;
            Task<Card> myCard2Task = null;
            for (var i = 0; i < Players.Count; i++)
            {
                var player = Players[i];
                if (!player.IsMyPlayer)
                {
                    tasks.Add(_cardManager.OpenOtherPlayerCardAsync(player, i));
                    tasks.Add(_cardManager.OpenOtherPlayerCardAsync(player, Players.Count + i));
                }else
                {
                    myCard1Task = _cardManager.OpenMyCardAsync(i);
                    tasks.Add(myCard1Task);
                    myCard2Task =_cardManager.OpenMyCardAsync(Players.Count + 1);
                    tasks.Add(myCard2Task);
                }
            }

            await Task.WhenAll(tasks);

            return new Card[]{myCard1Task.Result, myCard2Task.Result};
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
