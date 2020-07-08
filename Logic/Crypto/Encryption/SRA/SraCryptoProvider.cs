using BigInteger = System.Numerics.BigInteger;

namespace EtherBetClientLib.Crypto.Encryption.SRA
{
    public class SraCryptoProvider
    {
        private SraParameters keyPair { get; }

        public SraCryptoProvider(SraParameters keyPair)
        {
            this.keyPair = keyPair;
        }

        public BigInteger Decrypt(BigInteger m)
        {
            var decrypted = BigInteger.ModPow(m, keyPair.DecryptKey,  keyPair.Modulus);
            return decrypted;
        }

        public BigInteger Encrypt(BigInteger m)
        {
            var encrypted = BigInteger.ModPow(m, keyPair.EncryptKey, keyPair.DecryptKey);
            return encrypted;
        }
    }
}
