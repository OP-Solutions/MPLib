using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MPLib.Models.Games.CardGames.Messaging;
using MPLib.Networking;

namespace MPLib.Core.Game.General.CardGame
{
    public abstract class CardGameRound<TPlayer> : GameRound<TPlayer>
    {
        protected IPlayerMessageManager MessageManager;

        protected CardGameRound(ReadOnlyCollection<TPlayer> players, IMessageConfigBuilder messageConfigBuilder) 
            : base(players, messageConfigBuilder)
        {
            messageConfigBuilder.AddMessageTypesFromEnum<CardGameMessageType>();
        }
    }
}
