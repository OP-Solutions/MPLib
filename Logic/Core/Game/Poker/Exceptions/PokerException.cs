using System;
using System.Collections.Generic;
using System.Text;
using MPLib.Models.Exceptions;

namespace MPLib.Core.Game.Poker.Exceptions
{
    class PokerException : PlayerRuleViolationException<PokerRuleViolationType>
    {
        public PokerException(PokerRuleViolationType errorCode) : base(PokerGame.Instance, errorCode)
        {
        }
    }
}
