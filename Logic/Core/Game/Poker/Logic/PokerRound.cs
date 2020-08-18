using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using MPLib.Core.Game.General.CardGame;
using MPLib.Core.Game.General.CardGame.Messaging;
using MPLib.Core.Game.Poker.Exceptions;
using MPLib.Core.Game.Poker.Messaging;
using MPLib.Core.Game.Poker.Messaging.MessageTypes;
using MPLib.Models.Games;
using MPLib.Models.Games.CardGames;
using MPLib.Models.Games.CardGames.Messaging;
using MPLib.Models.Games.Poker;
using MPLib.Networking;

namespace MPLib.Core.Game.Poker.Logic
{

    /// <summary>
    /// This class contains logic of poker game round (dealing cards, betting, etcc..)
    /// TODO: Note that, Implementation of this class is halfway!
    /// </summary>
    public class PokerRound : ICardGameRound<PokerPlayer, MyPokerPlayer>
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
        public ReadOnlyCollection<PokerPlayer> Players { get; }

        public MyPokerPlayer MyPlayer { get; }

        public int SmallBlind { get; }

        /// <summary>
        /// Current bet amount for single player (not sum)
        /// This value start with big blind value and can increase several times in round
        /// as some players will do "Raise"
        /// This value should be updated instantly after <see cref="MyPokerPlayer.Raise"/> is called, before cycle completed
        /// After that, if other players will "Call" or "Fold" to that raised amount, it will stay unchanged,
        /// But if someone will re-"Raise" this value will increase instantly.
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



        CardEncryptionKeys ICardGameRound<PokerPlayer, MyPokerPlayer>.MyPlayerCardEncryptionKeys { get; set; }
        ReadOnlyCollection<Card> ICardGameRound<PokerPlayer, MyPokerPlayer>.SourceDeck { get; set; }

        private readonly IPlayerMessageManager<IPokerMessage> _messageManager;
        private readonly CardManager<PokerPlayer, MyPokerPlayer> _cardManager;
        private List<Card> _publicCards = new List<Card>(5);
        private int _nextCardIndex; // first card index which was not dealt yet (top card on yet non used deck)


        public PokerRound(int smallBlind, ReadOnlyCollection<PokerPlayer> players)
        {
            Players = players;
            State = PokerRoundState.NoStarted;
            SmallBlind = smallBlind;
            var messageManager = _messageManagerBuilder.Build<IMessage>(Players);
            _messageManager = messageManager;
            _cardManager = new CardManager<PokerPlayer, MyPokerPlayer>(messageManager, this);
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

            
            await SingleRoundCycle(true);

            await DealPublicCards(3); // deal flop

            await SingleRoundCycle();

            await DealPublicCards(1); // deal turn

            await SingleRoundCycle();

            await DealPublicCards(1); // deal river

            await ExposeCards();
        }


        private Task<Card[][]> ExposeCards() // cards[i][j] - j-th cards of i-th player
        {
            var result = new Card[Players.Count][];
            throw new NotImplementedException();
        }


        private async Task SingleRoundCycle(bool isFirstCycle = false)
        {

            var curPlayerIndex = 0;
            var betOpenerIndex = 0;

            if (isFirstCycle)
            {
                var bigBlind = SmallBlind * 2;
                var smallBlindPlayer = Players[0];
                var bigBlindPlayer = Players[1];
                smallBlindPlayer.RoundInfo.CurrentBetAmount = SmallBlind;
                bigBlindPlayer.RoundInfo.CurrentBetAmount = bigBlind; // big blind 

                CurrentBetAmount = bigBlind;
                SumOfBet = SmallBlind + bigBlind;
                curPlayerIndex = 2; // skip blinds and start betting from 3rd player

                betOpenerIndex = 2; // bet opener in this case is 3nd player (player after big blind),
                                    // because big blind is last one who does decision in first cycle
            }


            for(; curPlayerIndex != betOpenerIndex; curPlayerIndex++)
            {
                if (curPlayerIndex == Players.Count) curPlayerIndex = 0; // cycle: after last player, comes first

                var player = Players[curPlayerIndex];

                if(player.RoundInfo.State != PokerPlayerState.Active) continue;

                var move = await _messageManager.ReadMessageFrom<PokerMoveMessage>(player);

                if (move.BetAmount < 0) // check for fold
                {
                    player.RoundInfo.State = PokerPlayerState.Fold;
                    continue;
                }

                player.RoundInfo.CurrentBetAmount += move.BetAmount;
                player.CurrentChipAmount -= move.BetAmount;
                SumOfBet += move.BetAmount;


                if (player.RoundInfo.CurrentBetAmount > CurrentBetAmount) // check is player raised current bet
                {
                    betOpenerIndex = curPlayerIndex;
                    CurrentBetAmount = player.RoundInfo.CurrentBetAmount;
                }

                if (player.CurrentChipAmount < 0) throw new PokerException(PokerRuleViolationType.NotEnoughChipsToBet);
                else if (player.CurrentChipAmount == 0)
                {
                    player.RoundInfo.State = PokerPlayerState.AllIn;
                    continue;
                }

                
                if (player.RoundInfo.CurrentBetAmount < CurrentBetAmount) // player did not bet enough funds to call and did not "All-In" either
                {
                    throw new PokerException(PokerRuleViolationType.BetOutOfRange);
                }
            }
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
                    tasks.Add(_cardManager.OpenOtherPlayerCardAsync(i));
                    tasks.Add(_cardManager.OpenOtherPlayerCardAsync(Players.Count + i));
                }else
                {
                    myCard1Task = _cardManager.OpenMyCardAsync(i);
                    tasks.Add(myCard1Task);
                    myCard2Task =_cardManager.OpenMyCardAsync(Players.Count + 1);
                    tasks.Add(myCard2Task);
                }
            }

            _nextCardIndex = Players.Count * 2; // twice players count cards dealt already

            await Task.WhenAll(tasks);

            return new Card[]{myCard1Task.Result, myCard2Task.Result};
        }

        /// <summary>
        /// used for dealing public cards (flop, river, turn).
        /// Skips 1 card before dealing, following poker rules
        /// </summary>
        /// <returns></returns>
        private async Task DealPublicCards(int cardCountToDeal)
        {
            _nextCardIndex++; // skip first card

            var tasks = new Task[cardCountToDeal];

            for (int  i = 0; i < cardCountToDeal; i++)
            {
                tasks[i] = _cardManager.OpenPublicCardAsync(_nextCardIndex);
                _nextCardIndex++;
            }

            await Task.WhenAll(tasks);
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
