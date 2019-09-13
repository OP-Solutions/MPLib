using System;

namespace SPR.Core.Game.Poker.PokerLogic.PokerCombinations
{
    /// <summary>
    /// The TwoPairs poker combination
    /// </summary>
    public class TwoPairs : Combination
    {
        #region CONSTRUCTOR

        public TwoPairs(Card[] cards) : base(CombinationType.TwoPairs, cards)
        {
        }

        #endregion

        /// <summary>
        /// Checks if array contains TwoPairs combination
        /// </summary>
        /// <param name="cards">
        /// 7 elements cards array
        /// </param>
        /// <returns>
        /// Cards if contains and Null if not contains
        /// </returns>
        public override Combination Check(Card[] cards)
        {
            var firstPair = new Card[2];
            var secondPair = new Card[2];

            for (var i = cards.Length - 1; i >= 1; i--)
            {
                // already assumed that combination doesn't contains Three Of A Kind combination
                if (cards[i].Rank != cards[i - 1].Rank) continue;
                if (firstPair[0].Rank == CardRank.Default)
                {
                    firstPair[0] = cards[i];
                    firstPair[1] = cards[i - 1];
                }
                else
                {
                    secondPair[0] = cards[i];
                    secondPair[1] = cards[i - 1];
                    Array.Resize(ref firstPair, 4); // turn firstPair array into resulting array
                    Array.Copy(secondPair, 0, firstPair, 2,
                        2); // copy elements of second pair array into resulting array
                    return new Combination(Type, firstPair);
                }
            }

            return null;
        }
    }
}