using System;
using MPLib.Core.Game.Poker.Logic;

namespace MPLib.Core.Game
{
    public class GameManager
    {
       public static GameFactory Factory { get; internal set; } = new GameFactory();
    }


    public class GameFactory
    {

    }
}
