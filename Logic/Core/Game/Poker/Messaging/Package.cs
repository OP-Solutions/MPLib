namespace EtherBetClientLib.Core.Game.Poker.Messaging
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
