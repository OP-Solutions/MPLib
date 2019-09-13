namespace SPR.Core.Game.Poker.PokerLogic.PokerCombinations
{
    /// <summary>
    /// The royal flush.
    /// </summary>
    public class RoyalFlush : Combination
    {
        #region CONSTRUCTOR

        public RoyalFlush(Card[] cards) : base(CombinationType.RoyalFlush, cards)
        {
        }

        #endregion

        /// <summary>
        /// Checks if 7 element StraightFlush combination is RoyalFlush
        /// </summary>
        /// <param name="cards">
        /// Array of 5 cards
        /// </param>
        /// <returns>
        /// Cards in RoyalFlush combination if exits otherwise null
        /// </returns>
        public override Combination Check(Card[] cards)
        {
            var straightFlush = new StraightFlush(null).Check(cards, false);
            if (straightFlush == null)
            {
                PokerGameData.Combinations[CombinationType.StraightFlush] = null;
                return null;
            }

            PokerGameData.Combinations[CombinationType.StraightFlush] = straightFlush.Cards;

            return cards[1].Rank == CardRank.Teen
                ? new Combination(Type, cards)
                : null;
        }
    }
}