using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using EtherBetClientLib.Core.Game.General;
using EtherBetClientLib.Crypto.Encryption.SRA;
using EtherBetClientLib.Models;
using EtherBetClientLib.Models.Games.Poker;

namespace EtherBetClientLib.Random
{
    public class DecentralizedDeckShuffler
    {
        public delegate Task SendEvent(List<BigInteger> shuffledDeck);

        public delegate Task<List<BigInteger>> ReceiveFromEvent(Player player);

        public List<BigInteger> SourceDeck { get; }
        public IReadOnlyList<Player> Players { get; }
        
        private readonly SendEvent _send;
        private readonly ReceiveFromEvent _receiveFrom;

        public DecentralizedDeckShuffler(SendEvent send, ReceiveFromEvent receiveFrom, List<BigInteger> sourceDeck, 
            IReadOnlyList<Player> players)
        {
            _send = send;
            _receiveFrom = receiveFrom;
            SourceDeck = sourceDeck;
            Players = players;
        }

        public async Task<List<BigInteger>> Shuffle()
        {
            var currentDeck = SourceDeck;
            var me = (Player.Me as MyCardGamePlayer) ?? throw new InvalidOperationException();
            var provider1 = new SraCryptoProvider(me.CurrentSraKey1);

            foreach (var player in Players)
            {
                if (player == me)
                {
                    var shuffledCards = Shuffling.Shuffle(currentDeck);
                    var encryptedCards = shuffledCards.Select(n => provider1.Encrypt(n)).ToList();
                    await _send(encryptedCards);
                }

                currentDeck = await _receiveFrom(player);
            }

            foreach (var player in Players)
            {
                if (player == me)
                {
                    var reEncryptedCards = new List<BigInteger>(currentDeck.Count);
                    for (var i = 0; i < currentDeck.Count; i++)
                    {
                        var card = currentDeck[i];
                        var decryptedCard = provider1.Decrypt(card);
                        var provider2 = new SraCryptoProvider(me.CurrentSraKeys2[i]);
                        var cardReEncrypted = provider2.Encrypt(decryptedCard);
                        reEncryptedCards.Add(cardReEncrypted);
                    }
                    await _send(reEncryptedCards);
                }

                currentDeck = await _receiveFrom(player);
            }

            return currentDeck;
        }
    }
}
