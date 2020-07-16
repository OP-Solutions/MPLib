using System.ServiceModel;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    [ProtoContract]
    public class BetMessage : IPokerMessage
    {
        public const PokerMessageType BoundType = PokerMessageType.Bet;

        public PokerMessageType Type { get; set; } = BoundType;

        [ProtoMember(1)]
        public double BetValue;
    }
}
