using System.Collections.Generic;

namespace MPLib.Models.Games.CardGames
{
    public interface ICardGamePlayer
    {
        /// <summary>
        /// This player's cards
        /// </summary>
        IReadOnlyCollection<Card> Cards { get; }
    }
}