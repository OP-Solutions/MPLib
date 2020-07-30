using System.Collections.Generic;
using System.Linq;
using MPLib.Models.Games.CardGameModels;
using MPLib.Models.Games.Poker.PokerCombinations;

namespace MPLib.Models.Games.Poker
{
    /// <summary>
    /// Class for storing essential variables and constraints
    /// </summary>
    public static class PokerGameData
    {
        #region CONSTANTS

        #region GuiConstants

        public static string[] CombinationNames = new string[]
        {
            "High Card",
            "Pair",
            "Two Pairs",
            "Three of a Kind",
            "Straight",
            "Flush",
            "Full House",
            "Four of a Kind",
            "Straight Flush",
            "Royal Flush"
        };

        #endregion

        /// <summary>
        /// Count of card ranks
        /// </summary>
        public const int RankCount = 13;

        /// <summary>
        /// Count of card suits
        /// </summary>
        public const int SuitCount = 4;

        /// <summary>
        /// 5 cards which are on table.
        /// </summary>
        public const int CardsOnTableCount = 5;

        /// <summary>
        /// 2 unique cards which each player has and only he can see them.
        /// </summary>
        public const int PlayerUniqueCardCount = 2;

        /// <summary>
        /// Full number of each player cards which can be included in combination
        /// </summary>
        public const int CardCount = CardsOnTableCount + PlayerUniqueCardCount;

        /// <summary>
        /// Number of poker cards in deck
        /// </summary>
        public const int AllCardCount = 52;

        /// <summary>
        /// List of all poker combinations types, sorted from higher combinations to lower
        /// </summary>
        public static readonly ICombinationTypeChecker[] CombTypes =
        {
            new RoyalFlush(),
            new StraightFlush(),
            new FourOfAKind(),
            new FullHouse(),
            new FLush(),
            new Straight(),
            new ThreeOfAKind(),
            new TwoPairs(),
            new Pair(),
            new HighCard()
        };


        /// <summary>
        /// Dictionary of all poker combination types
        /// </summary>
        public static readonly Dictionary<CombinationType, ICombinationTypeChecker> CombTypesDictionary = new Dictionary<CombinationType, ICombinationTypeChecker>()
        {
            [CombinationType.RoyalFlush] = new RoyalFlush(),
            [CombinationType.StraightFlush] = new StraightFlush(),
            [CombinationType.FourOfAKind] = new FourOfAKind(),
            [CombinationType.FullHouse] = new FullHouse(),
            [CombinationType.Flush] = new FLush(),
            [CombinationType.StraightFlush] = new Straight(),
            [CombinationType.ThreeOfAKind] = new ThreeOfAKind(),
            [CombinationType.TwoPairs] = new TwoPairs(),
            [CombinationType.Pair] = new Pair(),
            [CombinationType.HighCard] = new HighCard()
        };

        /// <summary>
        /// List of all cards
        /// will be initialized in main method
        /// </summary>
        public static readonly IReadOnlyList<Card> CardDeck = Enumerable.Range(0, 52).Select(Card.FromNumber).ToArray();

        #endregion

    }
}