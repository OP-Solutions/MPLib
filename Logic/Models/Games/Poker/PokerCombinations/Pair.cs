using EtherBetClientLib.Models.Games.CardGameModels;

namespace EtherBetClientLib.Models.Games.Poker.PokerCombinations
{
    /// <summary>
    /// Pair combination
    /// </summary>
    public class Pair : Combination
    {
        #region CONSTRUCTOR

        public Pair(Card[] cards) : base(CombinationType.Pair, cards)
        {
        }

        #endregion

        /// <summary>
        /// Checks if 7 elements cards array contains pair combination
        /// before invoking this method array is already checked for this combination
        /// and result is saved in data class
        /// </summary>
        /// <param name="cards">
        /// 7 elements cards array
        /// </param>
        /// <returns>
        /// Pair combination if exits
        /// </returns>
        public override Combination Check(Card[] cards)
        {
            return PokerGameData.Combinations[CombinationType.Pair] == null
                ? null
                : new Combination(Type, PokerGameData.Combinations[CombinationType.Pair]);
        }
    }
}