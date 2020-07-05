using System.Collections.Generic;

namespace EtherBetClientLib.Core.Game.Poker.PokerLogic.PokerCombinations
{
    /// <summary>
    /// Straight combination
    /// </summary>
    public class Straight : Combination
    {
        #region CONSTRUCTOR

        public Straight(Card[] cards) : base(CombinationType.Straight, cards)
        {
        }

        #endregion


        /// <summary>
        /// Checks if 7 element cards array contains Straight
        /// </summary>
        /// <param name="cards">
        /// 7 element array of cards
        /// </param>
        /// <returns>
        /// Cards in Straight combination
        /// </returns>
        public override Combination Check(Card[] cards)
        {
            var containsRank = new bool[13]; // index == rank

            var straight = new List<Card>(5) {cards[6]};

            for (var i = PokerGameData.CardCount - 1; i >= 1; i--)
            {
                containsRank[(int) cards[i].Rank - 1] = true;
                if ((int) cards[i].Rank - (int) cards[i - 1].Rank == 1 ||
                    (int) cards[i].Rank - (int) cards[i - 1].Rank == 12) straight.Add(cards[i - 1]);

                if ((int) cards[i].Rank - (int) cards[i - 1].Rank > 1)
                {
                    straight.Clear();
                    straight.Add(cards[i - 1]);
                }

                if (straight.Count == 5) return new Combination(Type, straight.ToArray());
            }

            return null;
        }
    }
}