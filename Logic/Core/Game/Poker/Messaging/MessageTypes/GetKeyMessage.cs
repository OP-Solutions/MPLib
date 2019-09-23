using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPR.Core.Game.Poker.Messaging.MessageTypes
{
    class GetKeyMessage : Message
    {
        /// <summary>
        /// 0-based index of card
        /// </summary>
        public int CardIndex { get; set; }
    }
}
