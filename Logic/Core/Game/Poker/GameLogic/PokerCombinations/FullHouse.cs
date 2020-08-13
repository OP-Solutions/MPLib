using System;
using MPLib.Models.Games.CardGameModels;

namespace MPLib.Models.Games.Poker.PokerCombinations
{
    /// <summary>
    /// FullHouse combination
    /// </summary>
    public class FullHouse : ICombinationTypeChecker
    {
        /// <summary>
        /// Checks if 7 element Card array contains FullHouse
        /// </summary>
        /// <param name="cards">
        /// 7 element Card array
        /// </param>
        /// <param name="combination">
        /// combination instance
        /// </param>
        /// <returns>
        /// Full House Combination if found
        /// </returns>
        public bool Check(Card[] cards, Combination combination)
        {
            var pair = new Card[2];
            var threeOfaKind = new Card[3];

            /*PokerGameData.Combinations[CombinationType.Pair] = null;
            PokerGameData.Combinations[CombinationType.ThreeOfAKind] = null;
            */

            var isThreeOfaKind = threeOfaKind[0].Rank != CardRank.Undefined;
            for (var i = 0; i < cards.Length - 1; i++)
            {
                if (cards[i].Rank == cards[i + 1].Rank)
                {
                    if (cards.Length > i + 2 && cards[i + 1].Rank == cards[i + 2].Rank)
                    {
                        threeOfaKind[0] = cards[i];
                        threeOfaKind[1] = cards[i + 1];
                        threeOfaKind[2] = cards[i + 2];
                    }
                    else if (threeOfaKind[1] != cards[i])
                    {
                        pair[0] = cards[i];
                        pair[1] = cards[1 + 1];
                    }
                }

                var isPair = pair[0].Rank != CardRank.Undefined;

                /*
                if (isPair) PokerGameData.Combinations[CombinationType.Pair] = pair;
                if (isThreeOfaKind) PokerGameData.Combinations[CombinationType.ThreeOfAKind] = threeOfaKind;
                */

                if (!isPair || !isThreeOfaKind) continue;
                var fullHouse = new Card[5];
                Array.Copy(pair, fullHouse, 2); // copy 2 elements to 0;1 positions
                Array.Copy(threeOfaKind, 0, fullHouse, 2, 3); // copy 3 elements to 2;3;4 positions
               // return new Combination(Type, fullHouse);
                return true;
            }

            return false;
        }

        public int Compare(Combination first, Combination second)
        {
            throw new NotImplementedException();
        }

    }
}