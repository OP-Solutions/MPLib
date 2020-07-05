using System.Net;
using EtherBetClientLib.Networking;

namespace EtherBetClientLib.Models
{
    public class Player
    {
        public static Player Me { get; } = null;

        public string Name { get; set; }
        public string EthereumAddress { get; }
        public PlayerNetworkClient NetworkClient { get; }

        public Player(string name, string ethereumAddress, IPEndPoint endpointToConnect)
        {
            Name = name;
            EthereumAddress = ethereumAddress;
            NetworkClient = new PlayerNetworkClient(ethereumAddress, endpointToConnect);
        }
    }
}
