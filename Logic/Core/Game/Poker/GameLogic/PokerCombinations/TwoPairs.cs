using System;
using System.Collections.Generic;
using MPLib.Models.Games.CardGames;

namespace MPLib.Models.Games.Poker.PokerCombinations
{
    /// <summary>
    /// The TwoPairs poker combination
    /// </summary>
    public class TwoPairs : ICombinationTypeChecker
    {

        /// <summary>
        /// Checks if array contains TwoPairs combination
        /// </summary>
        /// <param name="cards">
        /// 7 elements cards array
        /// </param>
        /// <param name="combination">
        /// combination instance
        /// </param>
        /// <returns>
        /// Cards if contains and Null if not contains
        /// </returns>
        public bool Check(Card[] cards, Combination combination)
        {
            if (combination.SatisfiedCombinationTypes.HasFlag(CombinationType.TwoPairs))
            {
                return true;
            }

            if (combination.UnsatisfiedCombinationTypes.HasFlag(CombinationType.TwoPairs))
            {
                return false;
            }

            var firstPair = new Card[2];
            var secondPair = new Card[2];

            for (var i = cards.Length - 1; i > 0; i--)
            {
                // already assumed that combination doesn't contains Three Of A Kind combination
                if (cards[i].Rank != cards[i - 1].Rank) continue;

                if (firstPair[0].Rank == CardRank.Undefined)
                {
                    firstPair[0] = cards[i];
                    firstPair[1] = cards[i - 1];
                }
                else
                {
                    secondPair[0] = cards[i];
                    secondPair[1] = cards[i - 1];
                }
            }

            if (firstPair[0].Rank == CardRank.Undefined)
            {
                combination.UnsatisfiedCombinationTypes |= CombinationType.TwoPairs;
                combination.UnsatisfiedCombinationTypes |= CombinationType.Pair;
                return false;
            }

            combination.SatisfiedCombinationTypes |= CombinationType.Pair;
            var usedRanks = new List<CardRank> {firstPair[0].Rank};
            Card[] kickers;
            Array.Resize(ref firstPair, 5);

            if (secondPair[0].Rank != CardRank.Undefined)
            {
                combination.SatisfiedCombinationTypes |= CombinationType.TwoPairs;
                usedRanks.Add(secondPair[0].Rank);
                kickers = Combination.GetKickers(cards, usedRanks, 1);
                Array.Copy(secondPair, 0, firstPair, 2,
                    2);
                Array.Copy(kickers, 0, firstPair, 4, 1);
                combination.Top5 = firstPair;
                combination.Type = CombinationType.TwoPairs;
                return true;
            }

            combination.UnsatisfiedCombinationTypes |= CombinationType.TwoPairs;
            kickers = Combination.GetKickers(cards, usedRanks, 3);
            Array.Copy(kickers, 0, firstPair, 2, 3);
            combination.Top5 = firstPair;
            combination.Type = CombinationType.Pair;
            return false;
        }

        public int Compare(Combination first, Combination second)
        {
            throw new NotImplementedException();
        }
    }
}