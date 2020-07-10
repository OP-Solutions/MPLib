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

        public PlayerNetworkClient NetworkClient { get; }

        public bool IsMyPlayer() => this is MyPlayer;
    }

    public class MyPlayer : Player
    {
    }

}
