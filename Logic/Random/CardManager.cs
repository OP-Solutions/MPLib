using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using EtherBetClientLib.Core.Game.General;
using EtherBetClientLib.Crypto.Encryption.SRA;
using EtherBetClientLib.Models;
using EtherBetClientLib.Models.Games;
using EtherBetClientLib.Models.Games.CardGameModels;
using EtherBetClientLib.Models.Games.Poker;

namespace EtherBetClientLib.Random
{
    public class CardManager<TPlayer, TMyPlayer>
        where TPlayer : CardGamePlayer
        where TMyPlayer : TPlayer, IMyCardGamePlayer
    {
        public delegate Task SendEvent(List<BigInteger> shuffledDeck);

        public delegate Task<List<BigInteger>> ReceiveDeckFromEvent(TPlayer player);

        public delegate Task<PlayerKeys> ReceiveKeysFromEvent(TPlayer player);

        public List<BigInteger> SourceDeck { get; }
        public IReadOnlyList<TPlayer> Players { get; }
        public TMyPlayer MyPlayer { get; }


        private readonly Dictionary<TPlayer, List<BigInteger>> _firstCycleDeck;
        private readonly Dictionary<TPlayer, List<BigInteger>> _secondCycleDeck;
        private readonly SendEvent _send;
        private readonly ReceiveDeckFromEvent _receiveDeckFrom;
        private readonly ReceiveKeysFromEvent _receiveKeysFrom;

        public CardManager(SendEvent send, ReceiveDeckFromEvent receiveDeckFrom, ReceiveKeysFromEvent receiveKeysFrom,
            List<BigInteger> sourceDeck, IReadOnlyList<TPlayer> players, TMyPlayer myPlayer)
        {
            _send = send;
            _receiveDeckFrom = receiveDeckFrom;
            _receiveKeysFrom = receiveKeysFrom;
            _firstCycleDeck = new Dictionary<TPlayer, List<BigInteger>>();
            _secondCycleDeck = new Dictionary<TPlayer, List<BigInteger>>();
            SourceDeck = sourceDeck;
            Players = players;
            MyPlayer = myPlayer;
        }

        public async Task<IReadOnlyList<BigInteger>> Shuffle()
        {
            var currentDeck = SourceDeck;
            var provider1 = new SraCryptoProvider(MyPlayer.CardEncryptionKeys.SraKey1);

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
                _firstCycleDeck.Add(player, currentDeck);
            }

            foreach (var player in Players)
            {
                if (player == MyPlayer)
                {
                    var reEncryptedCards = new List<BigInteger>(currentDeck.Count);
                    for (var i = 0; i < currentDeck.Count; i++)
                    {
                        var card = currentDeck[i];
                        var decryptedCard = provider1.Decrypt(card);
                        var provider2 = new SraCryptoProvider(MyPlayer.CardEncryptionKeys.SraKeys2[i]);
                        var cardReEncrypted = provider2.Encrypt(decryptedCard);
                        reEncryptedCards.Add(cardReEncrypted);
                    }
                    await _send(reEncryptedCards);
                    currentDeck = reEncryptedCards;
                }
                else
                {
                    currentDeck = await _receiveDeckFrom(player);
                }
                _secondCycleDeck.Add(player, currentDeck);
            }

            return currentDeck;
        }


        /// <summary>
        /// Open card from deck only for local player. (only local player will see what card it is).
        /// When this is called local player gets sra keys from other players and decrypts card with these keys
        /// </summary>
        /// <returns>Decrypted card</returns>
        /// <remarks>
        /// when this is called, <see cref="OpenOtherPlayerCard"/> should be called on all remote player devices.
        /// </remarks>
        public async Task<Card> OpenMyCard(int cardIndex)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Open card from deck for target player (only specified player will see what card it is)
        /// When this is called local player sends his keys to specified player.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This method is should be called on all player devices, except of target player who should decrypt card,
        /// instead <see cref="OpenMyCard"/> should be called on target player device.
        /// </remarks>
        public async Task OpenOtherPlayerCard(Player targetPlayer, int cardIndex)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Publicly open card from deck. (all players will see what card is).
        /// When this is called local player gets sra keys from other players and also sending own key to them,
        /// after that all players will have all keys for corresponding card and they will decrypt it.
        /// </summary>
        /// <remarks>
        /// This method should be called on all player devices, so everyone knows keys of each other and all can decrypt target card
        /// </remarks>  
        /// <returns>Decrypted card</returns>
        public async Task<Card> OpenPublicCard(int cardIndex)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CheatInstance>> FindCheater(TPlayer requestedBy)
        {
            var playerKeyDict = new Dictionary<TPlayer, PlayerKeys>();

            foreach (var player in Players)
            {
                if (player != MyPlayer)
                {
                    var playerKeys = await _receiveKeysFrom(player);
                    playerKeyDict.Add(player, playerKeys);
                }
                else
                {
                    var playerKeys = new PlayerKeys
                    {
                        SraKey1 = MyPlayer.CardEncryptionKeys.SraKey1,
                        SraKeys2 = MyPlayer.CardEncryptionKeys.SraKeys2
                    };
                    playerKeyDict.Add(player, playerKeys);
                }
            }

            var sourceDeck = SourceDeck;
            var cheaters = new List<CheatInstance>();

            foreach (var player in Players)
            {
                if (!CheckShuffleValidity(sourceDeck, _firstCycleDeck[player], playerKeyDict[player].SraKey1))
                {
                    cheaters.Add(new CheatInstance(player, CheatType.InvalidShuffle));
                }

                sourceDeck = _firstCycleDeck[player];
            }

            foreach (var player in Players)
            {
                if (!CheckPostShuffleEncryptionValidity(sourceDeck, _secondCycleDeck[player], playerKeyDict[player]))
                {
                    cheaters.Add(new CheatInstance(player, CheatType.InvalidPostShuffleEncryption));
                }

                sourceDeck = _secondCycleDeck[player];
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
            return IsPermutation(shuffledDeck, resultDeck);
        }

        private bool CheckPostShuffleEncryptionValidity(List<BigInteger> sourceDeck, List<BigInteger> resultDeck, PlayerKeys keys)
        {
            var provider1 = new SraCryptoProvider(keys.SraKey1);
            var reEncryptedCards = new List<BigInteger>(sourceDeck.Count);
            for (var i = 0; i < sourceDeck.Count; i++)
            {
                var card = sourceDeck[i];
                var decryptedCard = provider1.Decrypt(card);
                var provider2 = new SraCryptoProvider(keys.SraKeys2[i]);
                var cardReEncrypted = provider2.Encrypt(decryptedCard);
                reEncryptedCards.Add(cardReEncrypted);
            }
            return IsPermutation(reEncryptedCards, resultDeck);
        }

        private bool IsPermutation(List<BigInteger> sourceDeck, List<BigInteger> resultDeck)
        {
            if (sourceDeck.Count != resultDeck.Count)
            {
                return false;
            }
            var sourceDeckCopy = new BigInteger[SourceDeck.Count];
            var resultDeckCopy = new BigInteger[resultDeck.Count];
            sourceDeck.CopyTo(sourceDeckCopy);
            resultDeck.CopyTo(resultDeckCopy);
            Array.Sort(sourceDeckCopy);
            Array.Sort(resultDeckCopy);


            for (int i = 0; i < sourceDeckCopy.Length; i++)
            {
                if (!sourceDeckCopy[i].Equals(resultDeckCopy[i]))
                {
                    return false;
                }
            }

            return true;

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
        InvalidPostShuffleEncryption,
        InvalidCheatAccusation
    }
}
