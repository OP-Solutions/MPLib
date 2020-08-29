using System;
using MPLib.Models.Games.CardGames;
using MPLib.Models.Games.Poker;

namespace MPLib.Core.Game.Poker.GameLogic.PokerCombinations
{
    /// <summary>
    /// Four of a kind type Combination
    /// </summary>
    public class FourOfAKind : ICombinationTypeChecker
    {
        /// <summary>
        /// checks if 7 element array of cards contains Four Of A Kind combination
        /// </summary>
        /// <param name="cards">
        /// Array of 7 cards
        /// </param>
        /// <param name="combination">
        /// combination instance
        /// </param>
        /// <returns>
        /// Four Of A Kind  combination if it's found, Null if not found
        /// </returns>
        public bool Check(Card[] cards, Combination combination)
        {
            var frequency = 1; // max frequency of rank in array

            for (var i = 0; i < cards.Length - 1; i++)
            {
                if (cards[i].Rank == cards[i + 1].Rank)
                {
                    frequency++; // if current card is equal to next card increase frequency by 1
                }
                else
                {
                    frequency = 1; // reset frequency
                }

                if (frequency != 4) continue;
                var result = new Card[4];

                // copy 4 "combination builder" cards at i-2, i-1, i, i+1 indexes from cards to result array.
                Array.Copy(cards, i - 2, result, 0, 4);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Compares two Four Of A Kind combination
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>
        /// 0 if equal 1 if current is greater and -1 if other is greater
        /// </returns>
        public int Compare(Combination first, Combination second)
        {
           
            return 0;
        }
    }
}