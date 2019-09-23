using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Signer;

namespace AppContext
{
    public class Context
    {
        public static EthECKey WalletKey { get; set; } = EthECKey.GenerateKey();
    }
}
