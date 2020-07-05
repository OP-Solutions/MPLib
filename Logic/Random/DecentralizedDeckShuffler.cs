using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using EtherBetClientLib.Crypto.Encryption.SRA;
using EtherBetClientLib.Models;

namespace EtherBetClientLib.Random
{
    public class DecentralizedDeckShuffler
    {
        public delegate Task SendEvent(List<BigInteger> shuffledDeck);

        public delegate Task<List<BigInteger>> ReceiveFromEvent(Player player);

        public List<BigInteger> SourceDeck { get; }
        public PokerRound Round { get; }
        private SendEvent _send;
        private ReceiveFromEvent _receiveFrom;

        public DecentralizedDeckShuffler(SendEvent send, ReceiveFromEvent receiveFrom, List<BigInteger> sourceDeck, PokerRound round)
        {
            _send = send;
            _receiveFrom = receiveFrom;
            SourceDeck = sourceDeck;
            Round = round;
        }

        public async Task<List<BigInteger>> Shuffle()
        {
            var currentDeck = SourceDeck;

            foreach (var player in Round.Players)
            {
                if (player == Player.Me)
                {
                    var shuffledCards = Shuffling.Shuffle(currentDeck);
                    var provider = new SraCryptoProvider(Round.MyKeys);
                    var encryptedCards = shuffledCards.Select(n => provider.Encrypt(n)).ToList();
                    await _send(encryptedCards);
                }

                currentDeck = await _receiveFrom(player);
            }

            foreach (var player in Round.Players)
            {
                if (player == Player.Me)
                {
                    var i = 0;
                    var encryptedCards = currentDeck.Select(n => new SraCryptoProvider(Round.MyKeys2[i++]).Encrypt(n)).ToList();
                    await _send(encryptedCards);
                }

                currentDeck = await _receiveFrom(player);
            }

            return currentDeck;
        }
    }
}
