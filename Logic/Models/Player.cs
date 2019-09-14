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
        public IPAddress CurrentIp { get; }

        public Player(string ethereumAddress, IPAddress currentIp)
        {
            EthereumAddress = ethereumAddress;
            CurrentIp = currentIp;
        }
    }
}
