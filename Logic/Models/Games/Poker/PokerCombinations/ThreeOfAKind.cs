using EtherBetClientLib.Models.Games.CardGameModels;

namespace EtherBetClientLib.Models.Games.Poker.PokerCombinations
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


            return false;

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