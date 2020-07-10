using System.Security.Cryptography;
using EtherBetClientLib.Networking;

namespace EtherBetClientLib.Models.Games
{
    public class Player
    {
        public static MyPlayer Me { get; } = new MyPlayer();

        /// <summary>
        /// ECDSA singing key of player. this property contains full (private + public) key if this player is "our" (local)  player
        /// Otherwise if this player is remotely connected this property contains only public key
        /// </summary>
        public CngKey Key { get; set; }

        public string Name { get; set; }
    }

    public class OtherPlayer : Player
    {
        public PlayerNetworkClient NetworkClient { get; }

        public CngKey Key { get; set; }
        public string Name { get; set; }

        public OtherPlayer(PlayerNetworkClient client)
        {
            NetworkClient = client;
        }
    }


    public class MyPlayer : Player
    {
        public string Name { get; set; }

        /// <summary>
        /// ECDSA singing key of player. this property contains full (private + public) key if this player is "our" (local)  player
        /// Otherwise if this player is remotely connected this property contains only public key
        /// </summary>
        public CngKey Key { get; set; }
    }

}
