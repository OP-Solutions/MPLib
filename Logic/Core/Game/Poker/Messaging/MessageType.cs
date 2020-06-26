using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPR.Core.Game.Poker.Messaging.MessageTypes;

namespace SPR.Core.Game.Poker.Messaging
{
    public enum MessageType
    {
        RoundStamp,
        Shuffle,
        ReEncryptMultiKey,
        GetKey,
        Bet,
        LeaveTable,
        ReShuffle,
        ReEncryptSingleKey,
        KeysExpose,
    }
}
