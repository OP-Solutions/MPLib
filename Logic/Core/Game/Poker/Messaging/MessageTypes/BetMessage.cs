using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    [ProtoContract]
    public class BetMessage : PokerMessage
    {
        [ProtoMember(1)]
        public double BetValue;
    }
}
