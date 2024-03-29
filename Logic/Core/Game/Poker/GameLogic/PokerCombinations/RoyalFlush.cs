﻿using MPLib.Models.Games.CardGames;
using MPLib.Models.Games.Poker;

namespace MPLib.Core.Game.Poker.GameLogic.PokerCombinations
{
    /// <summary>
    /// The royal flush.
    /// </summary>
    public class RoyalFlush : ICombinationTypeChecker
    {
        /// <summary>
        /// Checks if 7 element StraightFlush combination is RoyalFlush
        /// </summary>
        /// <param name="cards">
        /// Array of 5 cards
        /// </param>
        /// <param name="combination">
        /// combination instance
        /// </param>
        /// <returns>
        /// Cards in RoyalFlush combination if exits otherwise null
        /// </returns>
        public bool Check(Card[] cards, Combination combination)
        {
            return false;
        }

        public int Compare(Combination first, Combination second)
        {
            throw new System.NotImplementedException();
        }
    }
}