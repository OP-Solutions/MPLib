using System;
using System.Net;
using System.Security.Cryptography;
using EtherBetClientLib.Core.Game.General;
using EtherBetClientLib.Models.Games.Poker.Interfaces;

namespace EtherBetClientLib.Core.Game.Poker.Logic
{
    public class MyPokerPlayer : MyCardGamePlayer, IPokerPlayer
    {

        public PokerTable CurrentTable { get; set; }
        public PokerRound CurrentRound { get; set; }
        public int CurrentChipAmount { get; set; }
        public int LeftChipsAfterBet { get; set; }
        public int CurrentBetAmount { get; set; }
        public bool IsFold { get; set; }
        public bool IsTurn { get; set; }
        public bool IsAllIn { get; set; }
        public bool CheckFoldState { get; set; }
        public bool CallAnyState { get; set; }

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
            if (!CanCall())
                throw new InvalidOperationException();
            

            if (CurrentRound.CurrentBetAmount - CurrentBetAmount >= CurrentChipAmount)
            {
                AllIn();
                return;
            }

            
            CurrentChipAmount -= (CurrentRound.CurrentBetAmount - CurrentBetAmount);

        }


        /// <summary>
        /// Indicates if player can <see cref="Call"/> currently.
        /// </summary>
        /// <returns></returns>
        public bool CanCall()
        {
            if (IsTurn && CurrentRound.CurrentBetAmount > CurrentBetAmount && !IsAllIn && !IsFold)
                return true;

            return false;
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
            if(!CanCheck())
                throw new InvalidOperationException();


        }


        /// <summary>
        /// Indicates if player can <see cref="Check"/> currently.
        /// </summary>
        /// <returns></returns>
        public bool CanCheck()
        {
            if (IsTurn && CurrentRound.CurrentBetAmount - CurrentBetAmount == 0 && !IsFold)
                return true;

            return false;
        }

        /// <summary>
        ///  Does "Fold" poker move, (ie, stops betting in this round and admit lose)
        /// </summary>
        /// <exception cref="InvalidOperationException">If player is already fold or not his move</exception>
        public void Fold()
        {
            if(!CanFold())
                throw new InvalidOperationException();

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
        /// Does "Raise" poker move (raises current bet to specified total <paramref name="amount"/>)
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
        /// if amount is less than <see cref="GetMinRaise"/> or player does not have enough <paramref name="amount"/> funds
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


        public void ChangeCheckFoldState()
        {
            CheckFoldState = !CheckFoldState;
        }

        public bool IsCheckFoldMarked()
        {
            return CheckFoldState;
        }

        public void ChangeCallAnyState()
        {
            CallAnyState = !CallAnyState;
        }

        public bool IsCallAnyMarked()
        {
            return CallAnyState;
        }


    }
}
