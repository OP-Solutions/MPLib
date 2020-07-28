using System;
using System.Collections.Generic;
using MPLib.Core.Game.Poker.Logic;

namespace MPLib.Models.Games
{
    public abstract class GameTableBase<TPlayer, TMyPlayer>
    {
        public string Name { get; set; }

        public IReadOnlyList<TPlayer> Players => _players;

        public TMyPlayer MyPlayer { get; internal set; }

        public GameTableState State { get; internal set; }

        private List<TPlayer> _players = new List<TPlayer>();

        #region Table Events
        /// <summary>
        /// Fires when new player joins to the table
        /// </summary>
        public event Action<TPlayer> NewPlayerJoined;

        /// <summary>
        /// Fires when player leaves table
        /// </summary>
        public event Action<TPlayer> PlayerLeave;

        /// <summary>
        /// Fires when game is started, parameter is player list who will play first round
        /// </summary>
        public event Action<Player[]> GameStarted;

        /// <summary>
        /// Fires when <see cref="PokerRound"/> is finished and new round is going to start (if not end of game)
        /// </summary>
        public event Action<PokerRound> RoundFinished;

        /// <summary>
        /// Fires when new <see cref="PokerRound"/> is started
        /// </summary>
        public event Action<PokerRound> RoundStarted; 
        #endregion

    }

    public enum GameTableState
    {
        GameNotStarted,
        GameStarted,
        GameFinished,
    }
}
