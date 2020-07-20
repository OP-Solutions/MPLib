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
        /// Key for commutative encryption of cards, this keys are private and each player generates one for each round
        /// <see cref="PlayerKeys"/>
        /// </summary>
        internal PlayerKeys CardEncryptionKeys { get; set; }

    }
}
