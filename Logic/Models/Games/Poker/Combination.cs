using System;
using EtherBetClientLib.Models.Games.CardGameModels;

namespace EtherBetClientLib.Models.Games.Poker
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
        /// The cards in combination
        /// </summary>
        public Card[] Cards;

        #region CONSTRUCTOR

        /// <summary>
        /// Initializes a new instance of the <see cref="Combination"/> class.
        /// </summary>
        /// <param name="type">
        /// The rank.
        /// </param>
        /// <param name="cards">
        /// The cards.
        /// </param>
        public Combination(CombinationType type, Card[] cards)
        {
            Type = type;
            Cards = cards;
        }
 
        #endregion

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

            return a.Compare(b) == -1 ? true : false;
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
        /// Checks if input array contains specified combination
        /// overridden for all poker combination types
        /// </summary>
        /// <param name="cards">
        /// 7 elements cards array.
        /// </param>
        /// <returns>
        /// The <see>
        ///     <cref>Card[]</cref>
        /// </see>
        /// .
        /// </returns>
        public virtual Combination Check(Card[] cards)
        {
            return null;
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
            
            // else if combination types are equal we must compare greatest cards (override in FullHouse)
            if (Cards[Cards.Length - 1] > Cards[other.Cards.Length - 1]) return 1;
            if (Cards[Cards.Length - 1] < Cards[other.Cards.Length - 1]) return -1;

            return 0;
        }

        #endregion
    }
}
