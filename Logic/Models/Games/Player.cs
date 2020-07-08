using System.Net;
using System.Security.Cryptography;
using EtherBetClientLib.Networking;

namespace EtherBetClientLib.Models
{
    public abstract class Player
    {
        public static Player Me { get; } = null;

        public string Name { get; protected set; }
        public PlayerNetworkClient NetworkClient { get; }

        public Player(string name, CngKey key, IPEndPoint endpointToConnect)
        {
            Name = name;
            NetworkClient = new PlayerNetworkClient(name, key, endpointToConnect);
        }
    }
}
