using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using MPLib.Core.Game.General.CardGame.Messaging.MessageTypes;
using MPLib.Crypto.Encryption.SRA;
using MPLib.Models.Games.CardGames.Messaging;
using MPLib.Networking;
using MPLib.Random;

namespace MPLib.Models.Games.CardGames
{
    class CardManager<TPlayer, TMyPlayer>
        where TPlayer : Player, ICardGamePlayer
        where TMyPlayer : Player , ICardGamePlayer
    {

       


        private DeckCard[] _deck;

        private readonly Dictionary<TPlayer, IReadOnlyList<BigInteger>> _firstCycleDeck;
        private readonly Dictionary<TPlayer, IReadOnlyList<BigInteger>> _secondCycleDeck;

        private readonly IPlayerMessageManager<ICardGameMessage> _messageManager;
        private readonly ICardGameRound<TPlayer, TMyPlayer> _round;

        public CardManager(IPlayerMessageManager<ICardGameMessage> messageManager, ICardGameRound<TPlayer, TMyPlayer> round)
        {

            _messageManager = messageManager;
            _round = round;
            _firstCycleDeck = new Dictionary<TPlayer, IReadOnlyList<BigInteger>>();
            _secondCycleDeck = new Dictionary<TPlayer, IReadOnlyList<BigInteger>>();
        }



        /// <summary>
        /// shuffle and encrypt the deck of cards. after this process the deck becomes an array of 52 integers
        /// each encrypted by every player with a unique key
        /// </summary>
        /// <returns>shuffled and encrypted deck</returns>

        public async Task ShuffleAsync()
        {
            var provider1 = new SraCryptoProvider(_round.MyPlayerCardEncryptionKeys.SraKey1);
            IReadOnlyList<BigInteger> currentDeck = _deck.Select(d => d.Value).ToArray();

            foreach (var player in _round.Players)
            {
                if (player == _round.MyPlayer)
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

            foreach (var player in _round.Players)
            {
                if (player == _round.MyPlayer)
                {
                    var reEncryptedCards = new List<BigInteger>(currentDeck.Count);
                    for (var i = 0; i < currentDeck.Count; i++)
                    {
                        var card = currentDeck[i];
                        var decryptedCard = provider1.Decrypt(card);
                        var provider2 = new SraCryptoProvider(_round.MyPlayerCardEncryptionKeys.SraKeys2[i]);
                        var cardReEncrypted = provider2.Encrypt(decryptedCard);
                        reEncryptedCards.Add(cardReEncrypted);
                    }
                    await _messageManager.BroadcastMessage(new ReEncryptDeckMessage(reEncryptedCards));


                    currentDeck = reEncryptedCards;
                }
                else
                {
                    currentDeck = (await _messageManager.ReadMessageFrom<ReEncryptDeckMessage>(player)).EncryptedCards;
                }
                _secondCycleDeck.Add(player, currentDeck);
            }

            _deck = currentDeck.Select( (val, index) => new DeckCard()
            {
                IsKnown = false,
                Owner = null,
                Value = val,
                CardIndex = index 
            }).ToArray();
        }


        /// <summary>
        /// Open card from deck only for local player. (only local player will see what card it is).
        /// When this is called local player gets sra keys from other players and decrypts card with these keys
        /// </summary>
        /// <returns>Decrypted card</returns>
        /// <remarks>
        /// when this is called, <see cref="OpenOtherPlayerCardAsync"/> should be called on all remote player devices.
        /// </remarks>
        public async Task<Card> OpenMyCardAsync(int cardIndex)
        {
            var card = _deck[cardIndex];
            var value = card.Value;
            foreach (var player in _round.Players)
            {
                var key = player == _round.MyPlayer ? _round.MyPlayerCardEncryptionKeys.SraKeys2[cardIndex] : (await _messageManager.ReadMessageFrom<SingleKeyExposeMessage>(player)).Key;
                var provider = new SraCryptoProvider(key);
                value = provider.Decrypt(card.Value);
            }
            
            return Card.FromNumber((int)value);
        }


        /// <summary>
        /// Open card from deck for target player (only specified player will see what card it is)
        /// When this is called local sends his keys to all others (only player who should see card, not exposes his key to others).
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This method is should be called on all player devices, except of target player who should decrypt card,
        /// instead <see cref="OpenMyCardAsync"/> should be called on target player device.
        /// </remarks>
        public async Task OpenOtherPlayerCardAsync(int cardIndex)
        {
            await _messageManager.BroadcastMessage(new SingleKeyExposeMessage()
            {
                CardIndex = cardIndex,
                Key = _round.MyPlayerCardEncryptionKeys.SraKeys2[cardIndex]
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
        public async Task<Card> OpenPublicCardAsync(int cardIndex)
        {
            await _messageManager.BroadcastMessage(new SingleKeyExposeMessage()
            {
                CardIndex = cardIndex,
                Key = _round.MyPlayerCardEncryptionKeys.SraKeys2[cardIndex]
            });

            var card = _deck[cardIndex];
            var value = card.Value;
            foreach (var player in _round.Players)
            {
                var key = player == _round.MyPlayer ? _round.MyPlayerCardEncryptionKeys.SraKeys2[cardIndex] : (await _messageManager.ReadMessageFrom<SingleKeyExposeMessage>(player)).Key;
                var provider = new SraCryptoProvider(key);
                value = provider.Decrypt(value);
            }

            return Card.FromNumber((int)value);
        }


        /// <summary>
        /// publicly shows card whose this player already knows, to other players (makes it public).
        /// Difference with this method and <see cref="OpenPublicCardAsync"/> is that, in current method local player not expects key from remote players,
        /// therefore card is not decrypted for local player and no card value returned, because its assumed that local player already had decrypted card
        /// or knows its value in some other way, so he do not need keys from other players
        /// </summary>
        /// <param name="cardIndexInDeck">index of card to show, in deck. Please note that index is not relative to players cards</param>
        /// <returns></returns>
        public async Task ShowMyCardAsync(int cardIndexInDeck)
        {
            await _messageManager.BroadcastMessage(new SingleKeyExposeMessage()
            {
                CardIndex = cardIndexInDeck,
                Key = _round.MyPlayerCardEncryptionKeys.SraKeys2[cardIndexInDeck]
            });
        }


        /// <summary>
        /// Sends all his keys to other players, receives keys from them also and decrypts whole deck
        /// This is called same time from all players, after this all cards will be public
        /// </summary>
        /// <returns></returns>
        public async Task ExposeAllCards()
        {
            await _messageManager.BroadcastMessage(new ExposeKeysMessage() {Keys = _round.MyPlayerCardEncryptionKeys});

            var keys = new CardEncryptionKeys[_round.Players.Count];

            for (var i = 0; i < _round.Players.Count; i++)
            {
                var player = _round.Players[i];
                if (player.IsMyPlayer) keys[i] = _round.MyPlayerCardEncryptionKeys;
                else keys[i] = (await _messageManager.ReadMessageFrom<ExposeKeysMessage>(player)).Keys;
            }

            for (var cardIndex = 0; cardIndex < _deck.Length; cardIndex++)
            {
                var curCardVal = _deck[cardIndex].Value;

                for (var playerIndex = 0; playerIndex < _round.Players.Count; playerIndex++)
                {
                    var key = keys[playerIndex].SraKeys2[cardIndex];
                    var provider = new SraCryptoProvider(key);
                    curCardVal = provider.Decrypt(curCardVal);
                }

                var card = _deck[cardIndex];
                card.IsKnown = true;
                card.Value = curCardVal;
                card.DecryptedValue = Card.FromNumber((int)curCardVal);
            }
        }


        /// <summary>
        /// start a process that will find every invalid action during shuffling process
        /// and return an array of these actions along with players that executed them
        /// </summary>
        /// <returns> a list of cheat instances</returns>
        public async Task<List<CheatInstance>> FindCheaterAsync(TPlayer requestedBy)
        {
            var playerKeyDict = new Dictionary<TPlayer, CardEncryptionKeys>();

            foreach (var player in _round.Players)
            {
                if (!player.IsMyPlayer)
                {
                    var playerKeys = (await _messageManager.ReadMessageFrom<ExposeKeysMessage>(player)).Keys;
                    playerKeyDict.Add(player, playerKeys);
                }
                else
                {
                    playerKeyDict.Add(player, _round.MyPlayerCardEncryptionKeys);
                }
            }

            IReadOnlyList<BigInteger> encryptedDeck = _deck.Select(c => c.Value).ToArray();
            var cheaters = new List<CheatInstance>();

            foreach (var player in _round.Players)
            {
                if (!CheckShuffleValidity(encryptedDeck, _firstCycleDeck[player], playerKeyDict[player].SraKey1))
                {
                    cheaters.Add(new CheatInstance(player, CheatType.InvalidShuffle));
                }

                encryptedDeck = _firstCycleDeck[player];
            }

            foreach (var player in _round.Players)
            {
                if (!CheckPostShuffleEncryptionValidity(encryptedDeck, _secondCycleDeck[player], playerKeyDict[player]))
                {
                    cheaters.Add(new CheatInstance(player, CheatType.InvalidPostShuffleEncryption));
                }

                encryptedDeck = _secondCycleDeck[player];
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
        private bool CheckPostShuffleEncryptionValidity(IReadOnlyList<BigInteger> sourceDeck, IReadOnlyList<BigInteger> resultDeck, CardEncryptionKeys keys)
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
