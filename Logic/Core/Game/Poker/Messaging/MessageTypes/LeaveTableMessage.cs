using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPR.Core.Game.Poker.Messaging.MessageTypes
{
    class LeaveTableMessage : Message
    {
        public LeaveTableMessage()
        {
            Type = MessageType.LeaveTable;
        }
    }
}
