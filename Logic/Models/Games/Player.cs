using System.Net;
using EtherBetClientLib.Networking;

namespace EtherBetClientLib.Models
{
    public class Player
    {
        public static Player Me { get; } = null;

        public string Name { get; set; }
        public PlayerNetworkClient NetworkClient { get; }

        public Player(string name, string uniqueIdentifier, IPEndPoint endpointToConnect)
        {
            Name = name;
            NetworkClient = new PlayerNetworkClient(uniqueIdentifier, endpointToConnect);
        }
    }
}
