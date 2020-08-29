using System;
using System.Collections.Generic;
using MPLib.Models.Games.CardGames;
using MPLib.Models.Games.Poker;

namespace MPLib.Core.Game.Poker.GameLogic.PokerCombinations
{
    /// <summary>
    /// Three Of A Kind combination
    /// </summary>
    public class ThreeOfAKind : ICombinationTypeChecker
    {

        /// <summary>
        /// Checks if given array of cards contains ThreeOfaKind combination
        /// before invoking this method array is already checked for this combination
        /// and result is saved in data class
        /// </summary>
        /// <param name="cards">
        /// 7 elements cards array
        /// </param>
        /// <param name="combination">
        /// combination instance
        /// </param>
        /// <returns>
        /// Cards if contains, null if not contains
        /// </returns>
        public bool Check(Card[] cards, Combination combination)
        {
            if (combination.SatisfiedCombinationTypes.HasFlag(CombinationType.ThreeOfAKind))
            {
                return true;
            }
            if (combination.UnsatisfiedCombinationTypes.HasFlag(CombinationType.ThreeOfAKind))
            {
                return false;
            }
            var triple = new Card[2];
            for (var i = cards.Length - 2; i > 0; i--)
            {
                if (cards[i] != cards[i - 1] || cards[i-1] != cards[i-2]) continue;
                triple[0] = cards[i];
                triple[1] = cards[i - 1];
                triple[2] = cards[i - 2];
                break;
            }

            if (triple[0].Rank == CardRank.Undefined)
            {
                combination.UnsatisfiedCombinationTypes |= CombinationType.ThreeOfAKind;
                return false;
            }

            combination.SatisfiedCombinationTypes |= CombinationType.ThreeOfAKind;
            var usedRanks = new List<CardRank> { triple[0].Rank };
            var kickers = Combination.GetKickers(cards, usedRanks, 2);

            Array.Resize(ref triple, 5);
            Array.Copy(kickers, 0, triple, 3,
                2);
            combination.Top5 = triple;
            combination.Type = CombinationType.ThreeOfAKind;
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