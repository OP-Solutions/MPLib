using System.Collections.Generic;
using System.Collections.Immutable;

namespace MPLib.Models.Games.CardGames
{
    public interface ICardGamePlayer
    {
        /// <summary>
        /// This player's cards
        /// </summary>
        sealed ImmutableList<Card> Cards => InternalCards;

        internal ImmutableList<Card> InternalCards { get; set; }
    }
}