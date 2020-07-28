using MPLib.Crypto.Encryption.SRA;

namespace MPLib.Core.Game.General.CardGame
{
    public class PlayerKeys
    {
        /// <summary>
        /// Key for commutative encryption of cards, this key is private and each player generates one for each round
        /// Each card is encrypted with this key sequentially with each players key (order does not matter, its "commutative") 
        /// After rounds end this key is publicly exposed to other players, in order to verify fairness of game
        /// Except verification, this key is not used is actual gaming process, it is used to shuffle cards
        /// After shuffling card encrypted with that key and re-encrypted (<see cref="SraKeys2"/>)
        /// </summary>
        public SraParameters SraKey1 { get; set; }


        /// <summary>
        /// <seealso cref="SraKey1"/>
        /// Key for commutative encryption of cards, this key is private and each player generates (52 for each card) in each round
        /// After rounds end these keys is publicly exposed to other players, in order to verify fairness of game
        /// This is done for final card encryption:
        /// After shuffling player decrypts all card encrypted with <see cref="SraKey1"/> and
        /// and re-encrypts them each one with separate keys (<see cref="SraKey1"/> key was used on all before this step)
        /// </summary>
        public SraParameters[] SraKeys2 { get; set; }
    }
}
