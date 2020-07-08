using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EtherBetClientLib.Core.Game.General;

namespace EtherBetClientLib.Models.Games.Poker
{
    public class PokerPlayer : Player
    {
        public PokerTable CurrentTable { get; set; }
        public PokerRound CurrentRound { get; set; }
        public int CurrentChipAmount { get; set; }
        public int LeftChipsAfterBet { get; set; }
        public int CurrentBetAmount { get; set; }
    }

    public class MyPokerPlayer : MyCardGamePlayer
    {
        public MyPokerPlayer(string name, CngKey key, IPEndPoint endpointToConnect)
        {
        }

        /// <summary>
        ///  Does "Call" poker move (ie, bets exactly enough funds to match current bet)
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If this method is called when it is not this player turn,
        /// Or if method is called when there is no more chips needed to bet in order to match current bet amount (so player can just check)
        /// Or if player does not have enough amount of chips to match current bet
        /// Or player is already "Fold"
        /// </exception>
        public void Call()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Indicates if player can <see cref="Call"/> currently.
        /// </summary>
        /// <returns></returns>
        public bool CanCall()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Does "Check" poker move. (ie, does not bet any more chips, just skip his turn)
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If this method is called when it is not this player turn,
        /// Or if player can't just "Check", because need to bet more chips to match current bet.
        /// Or player is already "Fold"
        /// </exception>
        public void Check()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Indicates if player can <see cref="Check"/> currently.
        /// </summary>
        /// <returns></returns>
        public bool CanCheck()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Does "Fold" poker move, (ie, stops betting in this round and admit lose)
        /// </summary>
        /// <exception cref="InvalidOperationException">If player is already fold or not his move</exception>
        public void Fold()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indicates if player can <see cref="Fold"/> currently.
        /// </summary>
        /// <returns></returns>
        public bool CanFold()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Does "Raise" poker move (raises current bet to specified total amount)
        /// </summary>
        /// <param name="amount">
        /// Amount to raise to. This value is total, that means if user currently have to bet 50 but bets 100,
        /// this value will be 100 and not: 100 - 50 = 50
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// If this method is called when it is not this player turn
        /// Or player is already "Fold"
        /// Or player not has <see cref="amount"/> of chips
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// if amount is less than <see cref="GetMinRaise"/> or player does not have enough (<see cref="amount"/> funds
        /// </exception>
        public void Raise(int amount)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Gets minimum and maximum amount of chips that user can <see cref="Raise"/>
        /// Or <code>default(Range)</code> if raise is not available at all
        /// </summary>
        /// <returns>
        /// Range (inclusive) - For example (5, 10) you can call <see cref="Raise"/> which any of these parameters: 5, 6, 7, 8, 9, 10
        /// </returns>
        public Range CanRaise()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Does "All-in" poker move. this move is always available,
        /// only can not be used when player is already fold or not his turn
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If this method is called when it is not this player turn
        /// Or player is already "Fold"
        /// Or player have 0 chips left currently, so can't bet anymore
        /// </exception>
        public void AllIn()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Indicates if player can <see cref="AllIn"/> currently.
        /// </summary>
        /// <returns></returns>
        public bool CanAllIn()
        {
            throw new NotImplementedException();
        }
    }
}
