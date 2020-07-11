using System;
using System.Collections.Generic;
using System.Text;
using EtherBetClientLib.Models;
using EtherBetClientLib.Models.Games;
using EtherBetClientLib.Models.Games.Poker;
using EtherBetClientLib.Networking;

namespace EtherBetClientLib.Core.Game.Poker.Logic
{
    /// <summary>
    /// Base class for all poker player.
    /// <remarks>
    /// This class has several events (<see cref="OnThisPlayerTurn"/>, <see cref="OnThisPlayerCheckedCompleted"/>
    /// </remarks>
    /// </summary>
    public class PokerPlayer : Player
    {
        /// <summary>
        /// Fires when this player turn comes
        /// </summary>
        public event Action OnThisPlayerTurn;

        /// <summary>
        /// Fires after this player done "Check" move on his turn
        /// </summary>
        public event Action OnThisPlayerCheckedCompleted;

        /// <summary>
        /// Fires after this player done "Call" move on his turn.
        /// int parameter indicates amount of chips that user added to current bet
        /// </summary>
        public event Action OnThisPlayerCalledCompleted;

        /// <summary>
        /// Fires after this player done "Bet" move on his turn.
        /// </summary>
        public event Action<int> OnThisPlayerBetCompleted;

        /// <summary>
        /// Fires after this player done "Raise" move on his turn.
        /// <see cref="PlayerBetEventHandler"/>
        /// </summary>
        public event PlayerBetEventHandler OnThisPlayerRaiseCompleted;

        /// <summary>
        /// Fires after this player done "Fold" move on his turn.
        /// </summary>
        public event Action OnPlayerFoldCompleted;


        /// <summary>
        /// Fires after this player done "Raise" move on his turn.
        /// int parameter indicates amount of chips that user needed to add to current bet to make "all-in" move
        /// </summary>
        public event Action<int> OnPlayerAllInCompleted;


        /// <summary>Event handler type which is used to subscribe <see cref="PokerPlayer.OnThisPlayerRaiseCompleted"/> </summary>
        /// <param name="betAmount">chip amount that user did just bet</param>
        /// <param name="extraBetAmount">
        /// Extra chip amount (ie "Raise Amount") which user was not needed to bet for "call", but he chose to raise current bet with that extra amount
        /// </param>
        /// <remarks>
        ///  Example: if user need 50 chips to "Call" now, but he "Raises" and bets 200 gold instead, <paramref name="betAmount"/> is 200, and <paramref name="extraBetAmount"/> is 150.
        /// </remarks>
        public delegate void PlayerBetEventHandler(int betAmount, int extraBetAmount);

        public int CurrentChipAmount { get; set; }
        public int LeftChipsAfterBet { get; set; }
        public int CurrentBetAmount { get; set; }
        public bool IsFold { get; set; }
        public bool IsTurn { get; set; }
        public bool IsAllIn { get; set; }
        public bool HasRaised { get; set; }

        internal PokerTable CurrentTable { get; set; }
        internal PokerRound CurrentRound { get; set; }

    }
}
