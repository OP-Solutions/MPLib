using System.IO;
using System.Net;
using System.Security.Cryptography;
using MPLib.Helper;
using MPLib.Networking;

namespace MPLib.Models.Games
{
    public class Player
    {

        public static Player CreateMyPlayer(string name)
        {
            return new Player()
            {
                Name = name,
                Key = CngKey.Create(CngAlgorithm.ECDsa)
            };
        }


        protected Player()
        {

        }

        /// <summary>
        /// ECDSA singing key of player. this property contains full (private + public) key if this player is "our" (local)  player
        /// Otherwise if this player is remotely connected this property contains only public key
        /// </summary>
        public CngKey Key { get; internal set; }

        public virtual string Name { get; set; }

        public bool IsPlaying { get; internal set; }

        public bool IsMyPlayer => NetworkStream != null;

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
