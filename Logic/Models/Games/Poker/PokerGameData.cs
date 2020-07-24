using System.Collections.Generic;
using EtherBetClientLib.Models.Games.CardGameModels;
using EtherBetClientLib.Models.Games.Poker.PokerCombinations;

namespace EtherBetClientLib.Models.Games.Poker
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
        /// List of all poker combinations types
        /// </summary>
        public static ICombinationTypeChecker[] CombTypes =
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
        public static Dictionary<CombinationType, ICombinationTypeChecker> CombTypesDictionary = new Dictionary<CombinationType, ICombinationTypeChecker>()
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
        public static Card[] CardDeck = new Card[52];

        #endregion



        /// <summary>
        /// Main database where frequency of getting each combination
        /// as highest combination is stored. frequency of combination
        /// is number how many times we got that combination as highest 
        /// combination after checking all possible card sets in concrete situation. 
        /// </summary>
        public static Dictionary<CombinationType, int> FrequencyTable = new Dictionary<CombinationType, int>(10);

        /// <summary>
        /// Count of all checked cases
        /// </summary>
        public static int CaseCount = 0;


    }
}