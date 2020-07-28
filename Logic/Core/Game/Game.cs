using System;
using MPLib.Core.Game.Poker.Logic;

namespace MPLib.Core.Game
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
