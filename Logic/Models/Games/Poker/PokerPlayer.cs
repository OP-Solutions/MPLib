using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EtherBetClientLib.Models.Games.Poker
{
    public class PokerPlayer : PlayerBase
    {
        public int CurrentChipCount { get; set; }
        public PokerTable CurrentTable { get; set; }
        public PokerRound CurrentRound { get; set; }
        public int LeftChipsAfterBet { get; set; }

        public PokerPlayer(string name, string uniqueIdentifier, IPEndPoint endpointToConnect) : base(name, uniqueIdentifier, endpointToConnect)
        {
        }


    }

    public class MyPokerPlayer : PokerPlayer
    {

        public MyPokerPlayer(string name, string uniqueIdentifier, IPEndPoint endpointToConnect) : base(name, uniqueIdentifier, endpointToConnect)
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
        ///  Does "Fold" poker move, (ie, stops betting in this round and admit lose)
        /// </summary>
        public void Fold()
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
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// if amount is less than <see cref="GetMinRaise"/> or player does not have enough funds
        /// </exception>
        public void Raise(int amount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets minimal amount of chips which can be used to call <see cref="Raise"/>
        /// for example if method return 50 you can call <see cref="Raise"/> with 50 amount and above only
        /// </summary>
        /// <exception cref="InvalidOperationException">if player is already "Fold"</exception>
        /// <returns></returns>
        public int GetMinRaise()
        {
            throw new NotImplementedException();
        }
    }
}
