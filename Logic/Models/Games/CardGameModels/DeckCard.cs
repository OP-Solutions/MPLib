using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using MPLib.Core.Game.General.CardGame;

namespace MPLib.Models.Games.CardGameModels
{
    public class DeckCard
    {
        public Player Owner { get; internal set; }

        /// <summary>
        /// card index in deck
        /// </summary>
        public int CardIndex { get; internal set; }

        public bool IsKnown { get; internal set; }

        public Card DecryptedValue { get; internal set; }

        public BigInteger Value { get; internal set; }

        public List<PlayerKeys> KnownKeys { get; internal set; }
    }
}
