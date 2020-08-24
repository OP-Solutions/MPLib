using System.Collections.Generic;
using MPLib.Models.Games.CardGames;
using MPLib.Models.Games.Poker;

namespace MPLib.Core.Game.Poker.GameLogic.PokerCombinations
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


            var noDuplicates = new List<Card>(5){cards[cards.Length - 1]};
            for (var i = cards.Length - 2; i >= 0; i--)
            {
                if (cards[i] != cards[i-1])
                {
                    noDuplicates.Add(cards[i]);
                }
            }

            if (cards[cards.Length - 1].Rank == CardRank.Ace)
            {
                noDuplicates.Add(cards[cards.Length - 1]);
            }

            for (var i = 0; i <= noDuplicates.Count - 5; i++)
            {
                var isStraight = true; 
                for (var j = 0; j < 4; j++)
                {
                    if (noDuplicates[i+j].Rank != noDuplicates[i+j+1].Rank + 1 && (noDuplicates[i+j+1].Rank != CardRank.Ace || noDuplicates[i+j].Rank != CardRank.Two))
                    {
                        isStraight = false;
                        break;
                    }
                }

                if (isStraight)
                {
                    var straight = new Card[5];
                    for (int j = 0; j < 4; j++)
                    {
                        straight[j] = noDuplicates[i + j];
                    }

                    combination.SatisfiedCombinationTypes |= CombinationType.Straight;
                    combination.Top5 = straight;
                    combination.Type = CombinationType.Straight;
                    return true;
                }
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