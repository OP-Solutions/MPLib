using System.Net;

namespace MPLib.Models.Lobby
{
    class MinimalPlayerData
    {

        /// <summary>
        /// User defined named for player
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Generated unique id for player
        /// </summary>
        public string UUid { get; set; }

        /// <summary>
        /// Ip address where user responded from
        /// </summary>
        public IPAddress Address { get; }
    }
}
