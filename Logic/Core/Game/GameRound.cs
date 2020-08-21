using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;
using MPLib.Networking;

namespace MPLib.Core.Game
{
    public abstract class GameRound<TPlayer>
    { 
        public ReadOnlyCollection<TPlayer> Players { get; set; }

        public GameRound(ReadOnlyCollection<TPlayer> players, IMessageConfigBuilder messageConfigBuilder)
        {
            Players = players;
        }
    }
}
