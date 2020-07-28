using System;
using MPLib.Models.Games.CardGameModels;

namespace MPLib.Models.Games.Poker.PokerCombinations
{
    /// <summary>
    /// The Flush combination.
    /// </summary>
    public class FLush : ICombinationTypeChecker
    {


        /// <summary>
        /// Checks if 7 element cards array contains Flush
        /// </summary>
        /// <param name="cards">
        /// 7 element array of cards
        /// </param>
        /// <param name="combination">
        /// combination instance
        /// </param>
        /// <returns>
        /// Flush combination if found
        /// </returns>
        public bool Check(Card[] cards, Combination combination)
        {

            if (combination.SatisfiedCombinationTypes.HasFlag(CombinationType.Flush))
            {
                return true;
            }

            var frequency = new int[4]; // frequency of each suit

            var flushSuit = -1;

            for (var i = 0; i < cards.Length; i++)
            {
                frequency[(int) cards[i].Suit - 1] += 1;

                if (frequency[(int) cards[i].Suit - 1] == 5) flushSuit = (int) cards[i].Suit;
            }

            if (flushSuit == -1)
                return false;

            combination.Type = CombinationType.Flush;
            return true;

        }

        public int Compare(Combination first, Combination second)
        {
            throw new NotImplementedException();
        }
    }
}