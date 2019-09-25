using Nethereum.Signer;
using Nethereum.Util;
using SPR.Helper;

namespace SPR.Crypto.Signing
{
    public static class Signer
    {
        public static string Sign(byte[]message)
        {
            var signer = new MessageSigner();
            return signer.HashAndSign(message, AppContext.Context.WalletKey).Substring(2);
        }
    }
}
