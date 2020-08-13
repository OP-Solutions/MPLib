using System;
using System.Collections.Generic;
using MPLib.Models.Games.CardGameModels;

namespace MPLib.Models.Games.Poker.PokerCombinations
{
    /// <summary>
    /// Pair combination
    /// </summary>
    public class Pair : ICombinationTypeChecker
    {

        /// <summary>
        /// Checks if 7 elements cards array contains pair combination
        /// it is assumed that there are no better combinations made up by these cards 
        /// </summary>
        /// <param name="cards">
        /// 7 elements cards array
        /// </param>
        /// <param name="combination">
        /// combination instance
        /// </param>
        /// <returns>
        /// Pair combination if exits
        /// </returns>
        public bool Check(Card[] cards, Combination combination)
        {
            if (combination.SatisfiedCombinationTypes.HasFlag(CombinationType.Pair))
            {
                return true;
            }
            if (combination.UnsatisfiedCombinationTypes.HasFlag(CombinationType.Pair))
            {
                return false;
            }

            var pair = new Card[2];
            for (var i = cards.Length-1; i > 0; i--)
            {
                if (cards[i] != cards[i - 1]) continue;
                pair[0] = cards[i];
                pair[1] = cards[i - 1];
                break;
            }

            if (pair[0].Rank == CardRank.Undefined)
            {
                combination.UnsatisfiedCombinationTypes |= CombinationType.Pair;
                return false;
            }

            combination.SatisfiedCombinationTypes |= CombinationType.Pair;
            var usedRanks = new List<CardRank> {pair[0].Rank};
            var kickers = Combination.GetKickers(cards, usedRanks, 3);

            Array.Resize(ref pair, 4); 
            Array.Copy(kickers, 0, pair, 2,
                3);
            combination.Top5 = pair;
            combination.Type = CombinationType.Pair;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public int Compare(Combination first, Combination second)
        {
            return 0;
        }
    }
}