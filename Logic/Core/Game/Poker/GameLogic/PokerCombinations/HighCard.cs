﻿using System;
using System.Linq;
using MPLib.Models.Games.CardGames;
using MPLib.Models.Games.Poker;

namespace MPLib.Core.Game.Poker.GameLogic.PokerCombinations
{
    /// <summary>
    /// Hig Card combination
    /// </summary>
    public class HighCard : ICombinationTypeChecker
    {
        /// <summary>
        /// checks if 7 elements cards array contains HighCard combination
        /// because all cards array contains HighCard combination it always returns
        /// highest card in array and never returns null
        /// </summary>
        /// <param name="cards">
        /// 7 elements cards array
        /// </param>
        /// <param name="combination"></param>
        /// <returns>
        /// HighCard combination (which always exist and its highest card in array)
        /// </returns>
        public bool Check(Card[] cards, Combination combination)
        {
            if (cards.Length != 7)
            {
                throw new ArgumentException($"Invalid card array. Should contain 7 cards, but contains {cards.Length} cards", nameof(cards));
            }
            combination.Top5 = cards.Skip(2).ToArray();
            combination.Type = CombinationType.HighCard;
            return true;
        }

        public int Compare(Combination first, Combination second)
        {
            throw new NotImplementedException();
        }
    }
}