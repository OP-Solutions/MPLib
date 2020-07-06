using System.Collections.Generic;

namespace EtherBetClientLib.Models
{
    public abstract class GameTableBase
    {
        public List<PlayerBase> Players { get; set; }

        
        public GameTableBase(List<PlayerBase> players)
        {
            Players = players;
        }

    }
}
