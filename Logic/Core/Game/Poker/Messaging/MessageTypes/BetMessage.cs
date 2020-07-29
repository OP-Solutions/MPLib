using ProtoBuf;

namespace MPLib.Core.Game.Poker.Messaging.MessageTypes
{
    [ProtoContract]
    class BetMessage : IPokerMessage
    {
        public const PokerMessageType BoundType = PokerMessageType.Bet;

        public PokerMessageType Type { get; set; } = BoundType;

        [ProtoMember(1)]
        public double BetValue { get; }

        public BetMessage()
        {

        }

        public BetMessage(double betValue)
        {
            BetValue = betValue;
        }
    }
}
