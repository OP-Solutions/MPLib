using System;
using ProtoBuf;

namespace MPLib.Core.Game.Poker.Messaging.MessageTypes
{
    [ProtoContract]
    public class PokerMoveMessage : IPokerMessage
    {
        

        /// <summary>
        /// Amount of chips that player bet in this move. 0 means "Check", any negative value (typically -1) means "Fold"
        /// </summary>
        [ProtoMember(1)]
        public int BetAmount { get; set; }

        public PokerMoveMessage()
        {

        }

        public PokerMoveMessage(int betAmount)
        {
            BetAmount = betAmount;
        }

    }
}