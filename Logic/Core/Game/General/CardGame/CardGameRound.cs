using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MPLib.Models.Games;
using MPLib.Models.Games.CardGames;
using MPLib.Models.Games.CardGames.Messaging;
using MPLib.Networking;

namespace MPLib.Core.Game.General.CardGame
{
    public abstract class CardGameRound<TPlayer> : GameRound<TPlayer> where TPlayer : Player
    {
        

        /// <summary>
        /// Card index in deck which will be dealt next
        /// </summary>
        protected int NextCardIndex = 0;

        protected CardGameRound(IReadOnlyList<TPlayer> players) 
            : base(players)
        {
            
        }


        protected void ShuffleDeck()
        {
            throw new NotImplementedException();
        }

        protected DeckCard DealCardToPlayer(int playerIndex, bool openForTargetPlayer = true)
        {
            throw new NotImplementedException();
        }

        protected DeckCard DealSharedCard(bool openForAll = true)
        {
            throw new NotImplementedException();
        }

        protected DeckCard SkipNextCard(bool openForAll = true)
        {
            throw new NotImplementedException();
        }

        protected IReadOnlyList<DeckCard> GetAllPlayerCards(int playerIndex)
        {
            throw new NotImplementedException();
        }

        protected IReadOnlyList<DeckCard> GetAllSharedCards()
        {
            throw new NotImplementedException();
        }

        protected override void OnMessageConfiguration(IMessageConfigBuilder builder)
        {
            builder.AddMessageTypesFromEnum<CardGameMessageType>();
        }

    }
}
