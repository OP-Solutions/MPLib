using System;
using EtherBetClientLib.Models.Games.CardGameModels;

namespace EtherBetClientLib.Models.Games.Poker
{
    [Flags]
    public enum CombinationType
    {
        HighCard,
        Pair,
        TwoPairs,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        RoyalFlush
    }

    public interface ICombinationTypeChecker
    {

        /// <summary>
        /// Checks if input array contains specified combination
        /// overridden for all poker combination types
        /// </summary>
        /// <param name="cards">
        /// sorted 7 cards array.
        /// </param>
        /// <param name="combination">
        /// combination instance
        /// </param>
        /// <returns>
        /// The <see>
        ///     <cref>Card[]</cref>
        /// </see>
        /// .
        /// </returns>
        bool Check(Card[] cards, Combination combination);

        /// <summary>
        /// compare combinations of the same type;
        /// </summary>
        /// <param name="first">
        /// first combination
        /// </param>
        /// <param name="second">
        /// second combination
        /// </param>
        /// <returns></returns>
        int Compare(Combination first, Combination second);
    }

}