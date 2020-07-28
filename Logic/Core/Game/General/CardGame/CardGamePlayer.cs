using MPLib.Models.Games;

namespace MPLib.Core.Game.General.CardGame
{

    public class CardGamePlayer : Player
    {
    }

    public interface IMyCardGamePlayer
    {
        /// <summary>
        /// Key for commutative encryption of cards, this keys are private and each player generates one for each round
        /// <see cref="PlayerKeys"/>
        /// </summary>
        internal PlayerKeys CardEncryptionKeys { get; set; }

    }
}
