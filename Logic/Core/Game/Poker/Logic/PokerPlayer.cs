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
    /// </summary>
    public class PokerPlayer : Player
    {
        /// <summary>
        /// Fires when this player turn comes
        /// </summary>
        public event PokerPlayerMoveEventHandler OnThisPlayerTurn;

        /// <summary>
        /// Are called after player move is "Completed", "Completed" means player have done move, other players on network are notified,
        /// All cryptographic work is done and now its next player turn already
        /// </summary>
        public event PokerPlayerMoveEventHandler ThisPlayerMoveCompleted;


        /// <summary>
        /// Are called as soon as information about player move is received before any cryptographic, verification
        /// </summary>
        public event PokerPlayerMoveEventHandler ThisPlayerMoveReceived; 

        public int CurrentChipAmount { get; internal set; }
        public int LeftChipsAfterBet { get; internal set; }
        public int CurrentBetAmount { get; internal set; }
        public bool IsFold => State == PokerPlayerState.Fold;


        /// <summary>
        /// Indicates if it's this player turn now
        /// </summary>
        public bool IsTurn => CurrentRound.CurrentPlayer == this;

        public bool IsAllIn => State == PokerPlayerState.AllIn;
        public PokerPlayerState State { get; internal set; }

        internal PokerTable CurrentTable { get; set; }
        internal PokerRound CurrentRound { get; set; }
        internal bool HasRaised { get; set; }

    }

    public enum PokerPlayerState
    {
        Active,
        Fold,
        AllIn,
        NotPlaying
    }

    /// <summary>
    /// used in several <see cref="PokerPlayer"/> and <see cref="PokerRound"/> events
    /// </summary>
    /// <param name="moveAuthor">Author of move</param>
    /// <param name="moveInfo">move information</param>
    /// <param name="nextPlayer">Player whose turn is next after this move</param>
    public delegate void PokerPlayerMoveEventHandler(Player moveAuthor, PokerPlayerMoveInfo moveInfo, Player nextPlayer);

    public enum PokerPlayerMoveType { Check, Call, Fold, Bet, Raise }
}
