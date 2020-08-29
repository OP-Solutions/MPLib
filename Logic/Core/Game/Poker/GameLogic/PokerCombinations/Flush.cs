using System;
using System.Collections.Generic;
using System.Linq;
using MPLib.Models.Games.CardGames;
using MPLib.Models.Games.Poker;

namespace MPLib.Core.Game.Poker.GameLogic.PokerCombinations
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
            if (combination.UnsatisfiedCombinationTypes.HasFlag(CombinationType.Flush))
            {
                return false;
            }

            var cardsOfSuit = new List<List<Card>>(); // frequency of each suit
            for (int i = 0; i < 4; i++)
            {
                cardsOfSuit.Add(new List<Card>());
            }

            var flushSuit = -1;

            for (var i = cards.Length - 1; i >= 0; i--)
            {
                var suit = (int) cards[i].Suit - 1;
                cardsOfSuit[suit].Add(cards[i]);

                if (cardsOfSuit[suit].Count != 5) continue;
                flushSuit = (int) cards[i].Suit;
                break;
            }

            if (flushSuit == -1)
            {
                combination.UnsatisfiedCombinationTypes |= CombinationType.Flush;
                return false;
            }

            combination.SatisfiedCombinationTypes |= CombinationType.Flush;
            combination.Top5 = cardsOfSuit[flushSuit].Take(5).ToArray();
            combination.Type = CombinationType.Flush;
            return true;

        }

        public int Compare(Combination first, Combination second)
        {
            throw new NotImplementedException();
        }
    }
}