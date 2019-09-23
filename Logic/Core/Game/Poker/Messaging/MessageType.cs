using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPR.Core.Game.Poker.Messaging
{
    public enum MessageType
    {
        RoundStamp,
        ShuffleNextRound,
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
