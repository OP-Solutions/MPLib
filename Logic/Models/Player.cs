using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Signer;

namespace SPR.Models
{
    public class Player
    {
        public string Name { get; set; }
        public double CurTableBalance { get; set; }

        public RemoteClient Client { get; set; }
    }
}
