using System.Numerics;
using System.Text;

namespace EtherBetClientLib.Crypto.Encryption.HellMan
{

    public class HellManCryptoProvider
    {
        private PohligHellmanKey Key { get; }

        public HellManCryptoProvider(PohligHellmanKey key)
        {
            this.Key = key;
        }


        public string EncryptMessage(string m)
        {
            byte[] bytearray = Encoding.ASCII.GetBytes(m);
            BigInteger message = new BigInteger(bytearray);
            BigInteger encrypted = encr(message, Key.E, Key.P);
            return Encoding.ASCII.GetString(encrypted.ToByteArray());
        }

        public string DecryptMessage(string c)
        {
            byte[] bytearray = Encoding.ASCII.GetBytes(c);
            BigInteger encrypted = new BigInteger(bytearray);
            BigInteger m = decr(encrypted, Key.D, Key.P);
            return Encoding.ASCII.GetString(m.ToByteArray());
        }

        private BigInteger crypt(BigInteger m, BigInteger e, BigInteger p)
        {
            BigInteger val = BigInteger.ModPow(m, e, p);
            return val;

        }

        private BigInteger decr(BigInteger m, BigInteger d, BigInteger p)
        {
            return crypt(m, d, p);
        }

        private BigInteger encr(BigInteger m, BigInteger e, BigInteger p)
        {
            return crypt(m, e, p);
        }
    }
}
