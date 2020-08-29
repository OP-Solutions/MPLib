using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MPLib.Models.Games;
using MPLib.Models.Games.CardGames.Messaging;
using MPLib.Networking;

namespace MPLib.Core.Game.General.CardGame
{
    public abstract class CardGameRound<TPlayer> : GameRound<TPlayer> where TPlayer : Player
    {
        protected CardGameRound(IReadOnlyList<TPlayer> players) 
            : base(players)
        {
            
        }

        protected override void OnMessageConfiguration(IMessageConfigBuilder builder)
        {
            builder.AddMessageTypesFromEnum<CardGameMessageType>();
        }
    }
}
