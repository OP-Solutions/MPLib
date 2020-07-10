using System.Collections.Generic;

namespace EtherBetClientLib.Models.Games
{
    public abstract class GameTableBase
    {
        public List<Player> Players { get; set; }

        
        public GameTableBase(List<Player> players)
        {
            Players = players;
        }

    }
}
