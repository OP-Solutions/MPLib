using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Signer;

namespace SPR.Core.Game.Poker.Messaging
{
    public class Package
    {
        /// <summary>
        /// Ethereum address of sender
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Ethereum address of destination
        /// </summary>
        public string Destination { get; set; }

        public Message Message { get; set; }
    }
}
