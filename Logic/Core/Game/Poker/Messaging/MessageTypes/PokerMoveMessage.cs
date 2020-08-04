using System;
using ProtoBuf;

namespace MPLib.Core.Game.Poker.Messaging.MessageTypes
{
    [ProtoContract]
    public class PokerMoveMessage : IPokerMessage
    {
        [ProtoMember(0)]
        public PokerPlayerMoveType Type { get; }


        /// <summary>
        /// Full bet amount of player move. this is 0 in case of "Check" and throws <see cref="InvalidOperationException"/> in case of "Fold"
        /// </summary>
        public int FullBetAmount
        {
            get
            {
                if (Type == PokerPlayerMoveType.Fold) throw new InvalidOperationException("Player can't get bet amount of move when move is \"Fold\" ");
                return _fullBetAmount;
            }
        }

        [ProtoMember(1)]
        private int _fullBetAmount { get; set; }


        public PokerMoveMessage()
        {

        }

        public PokerMoveMessage(PokerPlayerMoveType type, int fullBetAmount)
        {
            Type = type;
            _fullBetAmount = fullBetAmount;
        }

    }

    public enum PokerPlayerMoveType { Check, Call, Fold, Bet, Raise }
}