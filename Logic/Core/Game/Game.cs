using System;
using System.Collections.Generic;
using System.Text;
using EtherBetClientLib.Core.Game.Poker.Logic;

namespace EtherBetClientLib.Core.Game
{
    public static class Game
    {
        public static PokerTable CreatePokerTable(string tableName, bool showInLAN, bool showInInternet)
        {
            if(showInInternet) throw new NotSupportedException(); //not yet supported

            throw new NotImplementedException();
        }
    }
}
