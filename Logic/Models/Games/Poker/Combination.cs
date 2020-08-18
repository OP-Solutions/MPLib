using System;
using System.Collections.Generic;
using MPLib.Models.Games.CardGames;

namespace MPLib.Models.Games.Poker
{
    /// <summary>
    /// Card array that builds poker combination
    /// </summary>
    public class Combination : IComparable<Combination>
    {
        

        /// <summary>
        /// Combination of current card set
        /// </summary>
        public CombinationType Type;

        /// <summary>
        /// The 7 cards in hand
        /// </summary>
        public Card[] Cards;

        /// <summary>
        /// Top 5 cards from 7 card hand
        /// </summary>
        public Card[] Top5;


        /// <summary>
        /// array of all the combination types that are satisfied by hand
        /// </summary>
        public CombinationType SatisfiedCombinationTypes = CombinationType.HighCard;


        /// <summary>
        ///  array of all the combination types that are unsatisfied by hand
        /// </summary>
        public CombinationType UnsatisfiedCombinationTypes = CombinationType.HighCard;

        #region CONSTRUCTOR

        /// <summary>
        /// Initializes a new instance of the <see cref="Combination"/> class.
        /// </summary>
        /// <param name="type">
        /// The rank.
        /// </param>
        /// <param name="cards">
        /// 7 cards.
        /// </param>
        /// <param name="top5">
        /// best combination of 5 out of 7 cards
        /// </param>
        public Combination(Card[] cards, CombinationType type, Card[] top5)
        {
            Array.Sort(cards);
            Type = type;
            Cards = cards;
            Top5 = top5;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Combination"/> class.
        /// </summary>
        /// <param name="cards">
        /// The cards.
        /// </param>
        public Combination(Card[] cards)
        {
            Array.Sort(cards);
            foreach (var combination in PokerGameData.CombTypes)
            {
                if (!combination.Check(cards, this)) continue;
                break;
            }

            Cards = cards;
        }

        #endregion

        public static Card[] GetKickers(Card[] cards, List<CardRank> used, int expectedKickerCount)
        {
            var kickerCount = 0;
            var kickers = new Card[expectedKickerCount];
            for (var i = cards.Length - 1; i > 0; i--)
            {
                if (used.Contains(cards[i].Rank)) continue;
                
                if (kickerCount < expectedKickerCount)
                {
                    kickers[kickerCount++] = cards[i];
                    continue;
                }
                break;
            }

            return kickers;
        }


        #region OPERATOR OVERRIDING

        /// <summary>
        /// The &gt;.
        /// </summary>
        /// <param name="a">
        /// first combination
        /// </param>
        /// <param name="b">
        /// second combination
        /// </param>
        /// <returns>
        /// true if first Combination is greater
        /// </returns>
        public static bool operator >(Combination a, Combination b)
        {
            if (a.Type > b.Type)
            {
                return true;
            }

            if (b.Type > a.Type)
            {
                return false;
            }

            return a.Compare(b) == 1;
        }


        /// <summary>
        /// The &lt;.
        /// </summary>
        /// <param name="a">
        /// first combination
        /// </param>
        /// <param name="b">
        /// second combination
        /// </param>
        /// <returns>
        /// true if second combination is Combination is greater
        /// </returns>
        public static bool operator <(Combination a, Combination b)
        {
            if (a.Type < b.Type)
            {
                return true;
            }

            if (b.Type < a.Type)
            {
                return false;
            }

            return a.Compare(b) == -1;
        }
 
        #endregion

        #region NON-STATIC METHODS


        /// <summary>
        /// this method defines default comparer
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int CompareTo(Combination other)
        {
            return Compare(other);
        }


        /// <summary>
        /// The compare.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="byte"/>.
        /// </returns>
        protected virtual int Compare(Combination other)
        {
            if (Type > other.Type) return 1;
            if (Type < other.Type) return -1;

            return PokerGameData.CombTypesDictionary[Type].Compare(this, other);
        }

        #endregion
    }
}
