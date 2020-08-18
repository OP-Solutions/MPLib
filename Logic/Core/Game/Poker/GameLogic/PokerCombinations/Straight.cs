using System.Collections.Generic;
using MPLib.Models.Games.CardGames;

namespace MPLib.Models.Games.Poker.PokerCombinations
{
    /// <summary>
    /// Straight combination
    /// </summary>
    public class Straight : ICombinationTypeChecker
    {
        /// <summary>
        /// Checks if 7 element cards array contains Straight and writes resulting combination to <paramref name="combination"/>
        /// </summary>
        /// <param name="cards">
        /// 7 element array of cards
        /// </param>
        /// <param name="combination">
        /// combination instance
        /// </param>
        /// <returns>
        /// if the cards make up a straight
        /// </returns>
        public bool Check(Card[] cards, Combination combination)
        {
            if (combination.SatisfiedCombinationTypes.HasFlag(CombinationType.Straight))
            {
                return true;
            }

            if (combination.UnsatisfiedCombinationTypes.HasFlag(CombinationType.Straight))
            {
                return false;
            }


            var straight = new List<Card>(5) {cards[6]};

            for (var i = PokerGameData.CardCount - 1; i >= 1; i--)
            {
                if ((int) cards[i].Rank - (int) cards[i - 1].Rank == 1 ||
                    (int) cards[i].Rank - (int) cards[i - 1].Rank == 12) straight.Add(cards[i - 1]);

                if ((int) cards[i].Rank - (int) cards[i - 1].Rank > 1)
                {
                    straight.Clear();
                    straight.Add(cards[i - 1]);
                }

                if (straight.Count != 5) continue;
                combination.SatisfiedCombinationTypes |= CombinationType.Straight;
                combination.Top5 = straight.ToArray();
                return true;
            }

            combination.UnsatisfiedCombinationTypes |= CombinationType.Straight;
            return false;
        }

        public int Compare(Combination first, Combination second)
        {
            throw new System.NotImplementedException();
        }
    }
}