using System.Net;
using EtherBetClientLib.Networking;

namespace EtherBetClientLib.Models
{
    public abstract class PlayerBase
    {
        public static PlayerBase Me { get; } = null;

        public string Name { get; set; }
        public PlayerNetworkClient NetworkClient { get; }

        public PlayerBase(string name, string uniqueIdentifier, IPEndPoint endpointToConnect)
        {
            Name = name;
            NetworkClient = new PlayerNetworkClient(uniqueIdentifier, endpointToConnect);
        }
    }
}
