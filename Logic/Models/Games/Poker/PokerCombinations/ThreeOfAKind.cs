using EtherBetClientLib.Models.Games.CardGameModels;

namespace EtherBetClientLib.Models.Games.Poker.PokerCombinations
{
    /// <summary>
    /// Three Of A Kind combination
    /// </summary>
    public class ThreeOfAKind : Combination
    {
        #region CONSTRUCTOR

        public ThreeOfAKind(Card[] cards) : base(CombinationType.ThreeOfAKind, cards)
        {
        }

        #endregion

        /// <summary>
        /// Checks if given array of cards contains ThreeOfaKind combination
        /// before invoking this method array is already checked for this combination
        /// and result is saved in data class
        /// </summary>
        /// <param name="cards">
        /// 7 elements cards array
        /// </param>
        /// <returns>
        /// Cards if contains, null if not contains
        /// </returns>
        public override Combination Check(Card[] cards)
        {
            return PokerGameData.Combinations[CombinationType.ThreeOfAKind] == null
                ? null
                : new Combination(Type, PokerGameData.Combinations[CombinationType.ThreeOfAKind]);
        }
    }
}