using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using EtherBetClientLib.Core.Game.General;
using EtherBetClientLib.Crypto.Encryption.SRA;

namespace EtherBetClientLib.Core.Game.Poker.Logic
{
    public class MyPokerPlayer : PokerPlayer, IMyCardGamePlayer
    {
        public bool AutoCheckFoldEnabled { get; set; }
        public bool AutoCallEnabled { get; set; }


        #region Internal Properties
        SraParameters IMyCardGamePlayer.CurrentSraKey1 { get; set; }
        SraParameters[] IMyCardGamePlayer.CurrentSraKeys2 { get; set; } 
        #endregion

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
            if (IsTurn && !IsAllIn && !IsFold )
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
            if (!CanCheck())
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
            if (!CanFold())
                throw new InvalidOperationException();

            State = PokerPlayerState.Fold;
        }

        /// <summary>
        /// Indicates if player can <see cref="Fold"/> currently.
        /// </summary>
        /// <returns></returns>
        public bool CanFold()
        {
            if (IsTurn && !IsAllIn && !IsFold)
                return true;

            return false;
        }


        /// <summary>
        /// Does "Bet" poker move (bets total <paramref name="amount"/>)
        /// </summary>
        /// <param name="amount">
        /// Amount to bet to.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// If this method is called when it is not this player turn
        /// Or player is already "Fold"
        /// Or player doesn't <see cref="amount"/> of chips
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// if amount is less than <see cref="GetMinRaise"/> or player does not have enough <paramref name="amount"/> funds
        /// </exception>
        public void Bet(int amount)
        {
            Range range;
            CanBet(out range);

            if (!CanBet(out range) && range.Start.Value < amount && range.End.Value > amount)
                throw new InvalidOperationException();


            CurrentBetAmount += amount;
            CurrentChipAmount -= amount;
        }

        /// <summary>
        /// Indicates if user can bet (<see cref="Bet"/> is available only when there is nothing to call so user can check or bet)
        /// Gets minimum and maximum amount of chips that user can <see cref="Bet"/>
        /// Or <code>default(Range)</code> if bet is not available at all
        /// </summary>
        /// <param name="betRange">/// Range (inclusive) - For example (5, 10) you can call <see cref="Bet"/> which any of these parameters: 5, 6, 7, 8, 9, 10</param>
        public bool CanBet(out Range betRange)
        {

            if (CanCheck() && !HasRaised && !IsAllIn)
            {
                Index startIndex = new Index(Math.Min(CurrentTable.CurrentSmallBlind * 2, CurrentChipAmount));
                Index endIndex = new Index(CurrentChipAmount);
                betRange = new Range(startIndex, endIndex);
                return true;
            }

            betRange = new Range();
            return false;
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
        /// Or player doesn't <see cref="amount"/> of chips
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// if amount is less than <see cref="GetMinRaise"/> or player does not have enough <paramref name="amount"/> funds
        /// </exception>
        public void Raise(int amount)
        {
            Range range;

            if(!CanRaise(out range))
                throw new InvalidOperationException();


            CurrentBetAmount += amount;
            CurrentChipAmount -= amount;
        }


        /// <summary>
        /// Indicates if user can raise (<see cref="Raise"/> is available only if there is some bet to call and player hasn't raised in this round yet)
        /// Gets minimum and maximum amount of chips that user can <see cref="Raise"/>
        /// Or <code>default(Range)</code> if raise is not available at all
        /// </summary>
        /// <param name="raiseRange">/// Range (inclusive) - For example (5, 10) you can call <see cref="Raise"/> which any of these parameters: 5, 6, 7, 8, 9, 10</param>
        public bool CanRaise(out Range raiseRange)
        {


            if (CanCall() && !HasRaised)
            {
                Index startIndex = new Index(Math.Min(CurrentTable.CurrentSmallBlind * 2, CurrentChipAmount));
                Index endIndex = new Index(CurrentChipAmount);
                raiseRange = new Range(startIndex, endIndex);
                return true;
            }

            raiseRange = new Range();
            return false;
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
            if(!CanAllIn())
                throw new InvalidOleVariantTypeException();

            State = PokerPlayerState.AllIn;
        }


        /// <summary>
        /// Indicates if player can <see cref="AllIn"/> currently.
        /// </summary>
        /// <returns></returns>
        public bool CanAllIn()
        {
            if (IsTurn && !IsFold && !IsAllIn)
                return true;

            return false;
        }
    }
}
