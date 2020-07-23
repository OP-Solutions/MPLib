using System;
using System.Collections.Generic;
using EtherBetClientLib.Models.Games;

namespace EtherBetClientLib.Core.Game.Poker.Logic
{
    public class PokerTable : GameTableBase<PokerPlayer, MyPokerPlayer>
    {
        public int CurrentSmallBlind { get; internal set; }
        public int CurrentSmallBlindPlayerIndex { get; set; }
        public PokerPlayer CurrentSmallBlindPlayer => Players[CurrentSmallBlindPlayerIndex];


        /// <summary>
        /// Time when current small blind will be changed
        /// Can be <see cref="DateTime.MaxValue"/> meaning constant small blind all game
        /// </summary>
        public DateTime NextSmallBlindChangeTime { get; internal set; }

        /// <summary>
        /// Value of small blind after <see cref="NextSmallBlindChangeTime"/>
        /// </summary>
        public int NextSmallBlind { get; internal set; }

        /// <summary>
        /// Notifies remote players about being ready to start
        /// If others player also call this method within some interval game will start
        /// </summary>
        public void RequireStart()
        {
            throw new NotImplementedException();
        }
    }
}
