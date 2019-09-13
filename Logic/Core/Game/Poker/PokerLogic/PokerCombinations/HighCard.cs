namespace SPR.Core.Game.Poker.PokerLogic.PokerCombinations
{
    /// <summary>
    /// Hig Card combination
    /// </summary>
    public class HighCard : Combination
    {
        public HighCard(Card[] cards) : base(CombinationType.HighCard, cards)
        {
        }

        /// <summary>
        /// checks if 7 elements cards array contains HighCard combination
        /// because all cards array contains HighCard combination it always returns
        /// highest card in array and never returns null
        /// </summary>
        /// <param name="cards">
        /// 7 elements cards array
        /// </param>
        /// <returns>
        /// HighCard combination (which always exist and its highest card in array)
        /// </returns>
        public override Combination Check(Card[] cards)
        {
            return new Combination(Type, new Card[1] {cards[PokerGameData.CardCount - 1]});
        }
    }
}