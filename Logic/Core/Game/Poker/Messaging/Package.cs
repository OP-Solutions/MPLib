using System;

namespace EtherBetClientLib.Core.Game.Poker.Messaging
{
    public class Package
    {
        /// <summary>
        /// Sender's public key
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Destination's public key
        /// </summary>
        public string Destination { get; set; }

        public DateTime TimeStampUtc { get; set; }

        public IMessage Message { get; set; }
    }

    public interface IMessage
    {
    }
}
