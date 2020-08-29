using System;
using MPLib.Models.Games.CardGames;
using MPLib.Models.Games.Poker;

namespace MPLib.Core.Game.Poker.GameLogic.PokerCombinations
{
    /// <summary>
    /// The straight flush combination
    /// </summary>
    public class StraightFlush : ICombinationTypeChecker
    {
        /// <summary>
        /// checks if 7 cards array contains StraightFlush
        /// </summary>
        /// <param name="cards">
        /// 7 cards array
        /// </param>
        /// <param name="combination">
        /// combination instance
        /// </param>
        /// <returns>
        /// Cards in StraightFlush combination if exits otherwise null
        /// </returns>
        public bool Check(Card[] cards, Combination combination)
        {
            

            return false;
        }

        public int Compare(Combination first, Combination second)
        {
            throw new NotImplementedException();
        }
    }
}