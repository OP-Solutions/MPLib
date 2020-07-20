using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using EtherBetClientLib.Core.Game.General;
using EtherBetClientLib.Crypto.Encryption.SRA;
using EtherBetClientLib.Helper;
using EtherBetClientLib.Models;
using EtherBetClientLib.Models.Games;
using EtherBetClientLib.Models.Games.Poker;

namespace EtherBetClientLib.Random
{
    public class DecentralizedDeckShuffler<TPlayer, TMyPlayer>
        where TPlayer : CardGamePlayer
        where TMyPlayer : TPlayer, IMyCardGamePlayer
    {
        public delegate Task SendEvent(List<BigInteger> shuffledDeck);

        public delegate Task<List<BigInteger>> ReceiveDeckFromEvent(TPlayer player);
        public delegate Task<BigInteger> RequestCardDecryptFromEvent(TPlayer playerToRequestFrom);

        public delegate Task<IReadOnlyDictionary<Player, SraParameters>> KeysExposeEvent();

        public List<BigInteger> SourceDeck { get; }
        public IReadOnlyList<TPlayer> Players { get; }
        public TMyPlayer MyPlayer { get; }
        

        private readonly Dictionary<TPlayer, List<BigInteger>> _shuffledEncryptedDeck;
        private readonly SendEvent _send;
        private readonly ReceiveDeckFromEvent _receiveDeckFrom;
        private readonly KeysExposeEvent _exposeKeys;
        public DecentralizedDeckShuffler(SendEvent send, ReceiveDeckFromEvent receiveDeckFrom, KeysExposeEvent exposeKeys,
            List<BigInteger> sourceDeck, IReadOnlyList<TPlayer> players, TMyPlayer myPlayer)
        {
            _send = send;
            _receiveDeckFrom = receiveDeckFrom;
            _shuffledEncryptedDeck = new Dictionary<TPlayer, List<BigInteger>>();
            SourceDeck = sourceDeck;
            Players = players;
            MyPlayer = myPlayer;
            _exposeKeys = exposeKeys;
        }

        public async Task<IReadOnlyList<BigInteger>> Shuffle()
        {
            var currentDeck = SourceDeck;
            var provider1 = new SraCryptoProvider(MyPlayer.CurrentSraKey1);

            foreach (var player in Players)
            {
                if (player == MyPlayer)
                {
                    var shuffledCards = Shuffling.Shuffle(currentDeck);
                    var encryptedCards = shuffledCards.Select(n => provider1.Encrypt(n)).ToList();
                    await _send(encryptedCards);
                    currentDeck = encryptedCards;
                }
                else
                {
                    currentDeck = await _receiveDeckFrom(player);
                }
                _shuffledEncryptedDeck.Add(player, currentDeck);
            }
            return currentDeck;
        }

        public async Task<List<CheatInstance>> FindCheater(TPlayer requestedBy)
        {
            var sourceDeck = SourceDeck;
            var cheaters = new List<CheatInstance>();

            var playerKeyDict = await _exposeKeys();

            foreach (var player in Players)
            {
                if (!CheckShuffleValidity(sourceDeck, _shuffledEncryptedDeck[player], playerKeyDict[player]))
                {
                    cheaters.Add(new CheatInstance(player, CheatType.InvalidShuffle));
                }

                sourceDeck = _shuffledEncryptedDeck[player];
            }


            if (cheaters.Count > 0)
                return cheaters;


            cheaters.Add(new CheatInstance(requestedBy, CheatType.InvalidCheatAccusation));
            return cheaters;
        }

        private bool CheckShuffleValidity(List<BigInteger> sourceDeck, List<BigInteger> resultDeck, SraParameters key)
        {

            var provider = new SraCryptoProvider(key);
            var shuffledDeck = sourceDeck.Select(n => provider.Encrypt(n)).ToList();
            return shuffledDeck.IsPermutationOf(resultDeck);
        }       

    }

    public class CheatInstance
    {
        public CheatInstance(Player players, CheatType cheatType)
        {
            Players = players;
            CheackType = cheatType;
        }

        public Player Players { get; }
        public CheatType CheackType { get; }
    }

    public enum CheatType
    {
        InvalidShuffle,
        InvalidCheatAccusation
    }
}
