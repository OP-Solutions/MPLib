using System.Collections.Generic;
using MPLib.Models.Games;
using MPLib.Models.Games.CardGameModels;

namespace MPLib.Core.Game.General.CardGame
{
    public interface IMyCardGamePlayer
    {
        /// <summary>
        /// Key for commutative encryption of cards, this keys are private and each player generates one for each round
        /// <see cref="PlayerKeys"/>
        /// </summary>
        internal PlayerKeys CardEncryptionKeys { get; set; }

        /// <summary>
        /// Cards visible only for local player
        /// </summary>
        public IReadOnlyList<Card> Cards { get; }

    }
}
