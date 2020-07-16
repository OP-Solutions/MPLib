using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using EtherBetClientLib.Models.Games.CardGameModels;
using ProtoBuf;

namespace EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes
{
    class EncryptShuffleMessage : IPokerMessage
    {
        public const PokerMessageType BoundType = PokerMessageType.EncryptSingleKey;

        public PokerMessageType Type { get; set; } = BoundType;

        [ProtoMember(1)]
        public BigInteger[] EncryptedCards { get; set; }

        /// <summary>
        /// Shuffle map is array which maps shuffled array to array before it was shuffled.
        /// this array has same size as <see cref="EncryptedCards"/>, which is also card deck size
        /// Values in this array indicates on which index is moved card which was at this index before shuffle.
        /// So if array start like {3, 5, 2 ...} this means that 1st card is now on index 3 in shuffled array and so on:
        /// ( beforeShuffleArray[0] == AfterShuffleArray[3]) ( beforeShuffleArray[1] == AfterShuffleArray[5]) ( beforeShuffleArray[2] == AfterShuffleArray[2])
        /// </summary>
        public byte[] ShuffleMap { get; set; }
    }
}
