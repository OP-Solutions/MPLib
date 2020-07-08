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
        public IReadOnlyList<Player> Players { get; }
        public SraCryptoProvider SraProvider { get; }

        private readonly SendEvent _send;
        private readonly ReceiveFromEvent _receiveFrom;

        public DecentralizedDeckShuffler(SendEvent send, ReceiveFromEvent receiveFrom, List<BigInteger> sourceDeck, 
            IReadOnlyList<Player> players, SraCryptoProvider encryptionProvider)
        {
            _send = send;
            _receiveFrom = receiveFrom;
            SourceDeck = sourceDeck;
            Players = players;
            SraProvider = encryptionProvider;
        }

        public async Task<List<BigInteger>> Shuffle()
        {
            var currentDeck = SourceDeck;

            foreach (var player in Players)
            {
                if (player == Player.Me)
                {
                    var shuffledCards = Shuffling.Shuffle(currentDeck);
                    var provider = SraProvider;
                    var encryptedCards = shuffledCards.Select(n => provider.Encrypt(n)).ToList();
                    await _send(encryptedCards);
                }

                currentDeck = await _receiveFrom(player);
            }

            return currentDeck;
        }
    }
}
