using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Signer;
using SPR.Networking;

namespace SPR.Models
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
