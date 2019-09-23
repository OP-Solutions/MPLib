using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPR.Core.Game.Poker.Messaging.MessageTypes
{
    class RoundStampMessage : Message
    {
        /// <summary>
        /// Unique Identifier Of Table this round belongs to
        /// </summary>
        public string TableGuid { get; set; }

        /// <summary>
        /// Unique Identifier Of Round
        /// </summary>
        public string RoundGuid { get; set; }

        /// <summary>
        /// Array Of Players, whose will play in corresponding round
        /// </summary>
        public string[] Players { get; set; }
    }
}
