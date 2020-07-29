using System.IO;
using System.Net;
using System.Security.Cryptography;
using MPLib.Crypto.Encryption.SRA;
using MPLib.Helper;
using MPLib.Networking;

namespace MPLib.Models.Games
{
    public class Player
    {

        internal static Player CreateMyPlayer(string name)
        {
            return new Player() { Name = name, Key = CngKey.Create(CngAlgorithm.ECDsa), PlayerType = PlayerType.Local };
        }

        internal static Player CreateOtherPlayer(string name, Stream networkStream)
        {
            return new Player() { Name = name, NetworkStream = networkStream, PlayerType = PlayerType.Remote };
        }

        protected Player()
        {

        }

        /// <summary>
        /// ECDSA singing key of player. this property contains full (private + public) key if this player is "our" (local)  player
        /// Otherwise if this player is remotely connected this property contains only public key
        /// </summary>
        public CngKey Key { get; internal set; }


        public byte[] PublicKeyBytes {
            get
            {
                if (_publicKeyBytes == null) _publicKeyBytes = Key.Export(CngKeyBlobFormat.EccPublicBlob);
                return _publicKeyBytes;
            }
        }

        private byte[] _publicKeyBytes;

        public virtual string Name { get; set; }

        public bool IsPlaying { get; internal set; }

        public PlayerType PlayerType { get; private set; }

        public bool IsMyPlayer => PlayerType == PlayerType.Local;

        internal Stream NetworkStream { get; private set; }
    }

    public enum PlayerType
    {
        /// <summary>
        /// Local player who owns this device (sometimes also referred in code as "My Player")
        /// </summary>
        Local,

        /// <summary>
        /// Remote player who is player from remote device (sometimes also referred in code as "Other Player")
        /// </summary>
        Remote,
    }
}
