using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using EtherBetClientLib.Crypto.Encryption.SRA;
using EtherBetClientLib.Models;
using EtherBetClientLib.Models.Games;

namespace EtherBetClientLib.Core.Game.General
{

    public class CardGamePlayer : Player
    {

    }

    public interface IMyCardGamePlayer
    {
        /// <summary>
        /// Key for commutative encryption of cards, this key is private and each player generates one for each round
        /// Each card is encrypted with this key sequentially with each players key (order does not matter, its "commutative") 
        /// After rounds end this key is publicly exposed to other players, in order to verify fairness of game
        /// </summary>
        internal SraParameters CurrentSraKey1 { get; set; }

    }
}
