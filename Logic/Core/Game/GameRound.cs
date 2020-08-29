using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;
using MPLib.Models.Games;
using MPLib.Networking;

namespace MPLib.Core.Game
{
    public abstract class GameRound<TPlayer> where TPlayer : Player
    { 
        public IReadOnlyList<TPlayer> Players { get; set; }


        protected IPlayerMessageManager MessageManager;

        public GameRound(IReadOnlyList<TPlayer> players)
        {
            Players = players;
            var messageManagerBuilder = new MessageManagerBuilder(PlayerIdentifyMode.Index);
            OnMessageConfiguration(messageManagerBuilder);
            MessageManager = messageManagerBuilder.Build(players);
        }

        protected abstract void OnMessageConfiguration(IMessageConfigBuilder builder);
    }
}
