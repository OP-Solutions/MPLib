using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SPR.Core.Game.Poker.PokerLogic;
using SPR.Models;

namespace SPR.Random
{
    class DecentralizedDeckShuffler
    {
        public delegate Task SendToEvent(Player player, List<BigInteger> shuffledDeck);

        public delegate Task ReceiveFromEvent(Player player, List<BigInteger> shuffledDeck);

        public List<Card> SourceDeck { get; }
        private SendToEvent _sendTo;
        private ReceiveFromEvent _receiveFrom;

        public DecentralizedDeckShuffler(SendToEvent sendTo, ReceiveFromEvent receiveFrom, List<Card> sourceDeck)
        {
            _sendTo = sendTo;
            _receiveFrom = receiveFrom;
            SourceDeck = sourceDeck;
        }

        public async Task<List<BigInteger>> Shuffle()
        {
            throw new NotImplementedException();
        }
    }
}
