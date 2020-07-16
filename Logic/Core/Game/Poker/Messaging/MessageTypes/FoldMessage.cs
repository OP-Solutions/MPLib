using System;
using System.Collections.Generic;
using System.Text;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class FoldMessage : IPokerMessage
    {
        public const PokerMessageType BoundType = PokerMessageType.Fold;

        public PokerMessageType Type { get; set; } = BoundType;
    }
}
