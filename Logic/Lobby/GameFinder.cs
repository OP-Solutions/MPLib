using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SPR.Models;

namespace SPR.Lobby
{
    public class GameFinder
    {
        public event FoundGameCallback OnGameFound;

        public GameFinder()
        {

        }

        public GameFinder(FoundGameCallback callback)
        {
            OnGameFound += callback;
        }


        public void FindGame()
        {

        }
    }

    public delegate void FoundGameCallback(Player[] otherPlayers);
}
