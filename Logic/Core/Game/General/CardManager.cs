using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using EtherBetClientLib.Core.Game.Poker.Messaging;
using EtherBetClientLib.Core.Game.Poker.Messaging.MessageTypes;
using EtherBetClientLib.Crypto.Encryption.SRA;
using EtherBetClientLib.Models.Games;
using EtherBetClientLib.Models.Games.CardGameModels;
using EtherBetClientLib.Networking;
using EtherBetClientLib.Random;

namespace EtherBetClientLib.Core.Game.General
{
    public class CardManager<TPlayer, TMyPlayer>
        where TPlayer : CardGamePlayer
        where TMyPlayer : TPlayer, IMyCardGamePlayer
    {

        public IReadOnlyList<BigInteger> SourceDeck { get; }
        public IReadOnlyList<TPlayer> Players { get; }
        public TMyPlayer MyPlayer { get; }

        private IReadOnlyList<BigInteger> _finalDeck;
        private readonly Dictionary<TPlayer, IReadOnlyList<BigInteger>> _firstCycleDeck;
        private readonly Dictionary<TPlayer, IReadOnlyList<BigInteger>> _secondCycleDeck;

        private readonly IPlayerMessageManager<TPlayer, IMessage> _messageManager;

        public CardManager(IPlayerMessageManager<TPlayer, IMessage> messageManager, IReadOnlyList<Card> sourceDeck, IReadOnlyList<TPlayer> players, TMyPlayer myPlayer)
        {

            _messageManager = messageManager;
            _firstCycleDeck = new Dictionary<TPlayer, IReadOnlyList<BigInteger>>();
            _secondCycleDeck = new Dictionary<TPlayer, IReadOnlyList<BigInteger>>();
            SourceDeck = sourceDeck.Select(card => new BigInteger(Card.ToNumber(card))).ToArray();
            Players = players;
            MyPlayer = myPlayer;
        }



        /// <summary>
        /// shuffle and encrypt the deck of cards. after this process the deck becomes an array of 52 integers
        /// each encrypted by every player with a unique key
        /// </summary>
        /// <returns>shuffled and encrypted deck</returns>

        public async Task<IReadOnlyList<BigInteger>> Shuffle()
        {
            var currentDeck = SourceDeck;
            var provider1 = new SraCryptoProvider(MyPlayer.CardEncryptionKeys.SraKey1);

            foreach (var player in Players)
            {
                if (player == MyPlayer)
                {
                    var shuffledCards = Shuffling.Shuffle(currentDeck);
                    var encryptedCards = shuffledCards.Select(n => provider1.Encrypt(n)).ToArray();
                    await _messageManager.BroadcastMessage(new EncryptShuffleDeckMessage(encryptedCards));
                    currentDeck = encryptedCards;
                }
                else
                {
                    currentDeck = (await _messageManager.ReadMessageFrom<EncryptShuffleDeckMessage>(player)).EncryptedCards;
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
                    await _messageManager.BroadcastMessage(new EncryptShuffleDeckMessage(reEncryptedCards));


                    currentDeck = reEncryptedCards;
                }
                else
                {
                    currentDeck = (await _messageManager.ReadMessageFrom<EncryptShuffleDeckMessage>(player)).EncryptedCards;
                }
                _secondCycleDeck.Add(player, currentDeck);
            }


            return (_finalDeck = currentDeck);
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
            var card = _finalDeck[cardIndex];
            foreach (var player in Players)
            {
                var key = player == MyPlayer ? MyPlayer.CardEncryptionKeys.SraKeys2[cardIndex] : (await _messageManager.ReadMessageFrom<SingleKeyExposeMessage>(player)).Key;
                var provider = new SraCryptoProvider(key);
                card = provider.Decrypt(card);
            }
            
            return Card.FromNumber((int)card);
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
        public async Task OpenOtherPlayerCard(TPlayer targetPlayer, int cardIndex)
        {
            await _messageManager.SendMessageTo(targetPlayer, new SingleKeyExposeMessage()
            {
                CardIndex = cardIndex,
                Key = MyPlayer.CardEncryptionKeys.SraKeys2[cardIndex]
            });
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
            await _messageManager.BroadcastMessage(new SingleKeyExposeMessage()
            {
                CardIndex = cardIndex,
                Key = MyPlayer.CardEncryptionKeys.SraKeys2[cardIndex]
            });

            var card = _finalDeck[cardIndex];
            foreach (var player in Players)
            {
                var key = player == MyPlayer ? MyPlayer.CardEncryptionKeys.SraKeys2[cardIndex] : (await _messageManager.ReadMessageFrom<SingleKeyExposeMessage>(player)).Key;
                var provider = new SraCryptoProvider(key);
                card = provider.Decrypt(card);
            }

            return Card.FromNumber((int)card);
        }


        /// <summary>
        /// start a process that will find every invalid action during shuffling process
        /// and return an array of these actions along with players that executed them
        /// </summary>
        /// <returns> a list of cheat instances</returns>
        public async Task<List<CheatInstance>> FindCheater(TPlayer requestedBy)
        {
            var playerKeyDict = new Dictionary<TPlayer, PlayerKeys>();

            foreach (var player in Players)
            {
                if (player != MyPlayer)
                {
                    var playerKeys = (await _messageManager.ReadMessageFrom<ExposeKeysMessage>(player)).Keys;
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

        /// <summary>
        /// check if a step during a shuffling process was executed properly
        /// </summary>
        /// <param name="sourceDeck"> deck state at the start of the step </param>
        /// <param name="resultDeck"> deck state at the end of the step </param>
        /// <param name="key"> key by which the cards were encrypted post shuffle </param>
        /// <returns> true or false depending on the validity of the action taken </returns>
        private bool CheckShuffleValidity(IEnumerable<BigInteger> sourceDeck, IReadOnlyList<BigInteger> resultDeck, SraParameters key)
        {

            var provider = new SraCryptoProvider(key);
            var shuffledDeck = sourceDeck.Select(n => provider.Encrypt(n)).ToList();
            return IsPermutation(shuffledDeck, resultDeck);
        }

        /// <summary>
        /// check if a step during a reencrypting process was executed properly
        /// </summary>
        /// <param name="sourceDeck"> deck state at the start of the step </param>
        /// <param name="resultDeck"> deck state at the end of the step </param>
        /// <param name="keys"> keys by which the cards were encrypted post shuffle </param>
        /// <returns> true or false depending on the validity of the action taken </returns>
        private bool CheckPostShuffleEncryptionValidity(IReadOnlyList<BigInteger> sourceDeck, IReadOnlyList<BigInteger> resultDeck, PlayerKeys keys)
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

        /// <summary>
        /// checks if one array is another array's permutation
        /// </summary>
        private bool IsPermutation(IReadOnlyList<BigInteger> sourceDeck, IReadOnlyList<BigInteger> resultDeck)
        {
            if (sourceDeck.Count != resultDeck.Count)
            {
                return false;
            }
            var sourceDeckCopy = sourceDeck.ToArray();
            var resultDeckCopy = resultDeck.ToArray();
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
            CheatType = cheatType;
        }

        public Player Players { get; }
        public CheatType CheatType { get; }
    }

    public enum CheatType
    {
        InvalidShuffle,
        InvalidPostShuffleEncryption,
        InvalidCheatAccusation
    }
}
