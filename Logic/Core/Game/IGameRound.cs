using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using MPLib.Models.Games;

namespace MPLib.Core.Game
{
    public interface IGameRound<TPlayer, out TMyPlayer> where TPlayer : Player where TMyPlayer : Player
    {
        IReadOnlyList<TPlayer> Players { get; }
        TMyPlayer MyPlayer { get; }

    }
}