using System;
using System.Collections.Generic;
using System.Text;

namespace MPLib.Core.Game.Poker
{
    class PokerGame : Game
    {

        public static PokerGame Instance { get; } = new PokerGame();

        public override string FullName { get; internal set; } = "Texas Holdem Poker";
        public override Guid Guid { get; } = System.Guid.ParseExact("A2B2722F-A610-4AC2-9612-50AF9F62FCA2", "D");
        public override Version Version { get; internal set; } = new Version(1, 0, 0);

        public override bool Equals(object obj)
        {
            if (obj is Game g)
            {
                return this.Guid == g.Guid;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
