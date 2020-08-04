using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MPLib.Core.Game.Poker.Messaging;
using MPLib.Core.Game.Poker.Messaging.MessageTypes;
using MPLib.Models.Games;
using MPLib.Networking;
using Xunit;
using static Tests.StaticData;

namespace Tests.Networking
{
    public class PlayerMessageManagerTests
    {



        [Fact]
        public void PlayerMessageManagerTest()
        {
            var task1 = Task.Run(RunPlayer1);
            var task2 = Task.Run(RunPlayer2);
            var task3 = Task.Run(RunPlayer3);

            Task.WaitAll(task1, task2, task3);
        }


        private const int Player1ListenPort = 6169;
        private const int Player2ListenPort = 6170;
        private const int Player3ListenPort = 6171;

        private const string Player1Name = "TestPlyer1";
        private const string Player2Name = "TestPlayer2";
        private const string Player3Name = "TestPlayer3";

        private static readonly CngKey Player1Key = CngKey.Create(CngAlgorithm.ECDsaP384);
        private static readonly CngKey Player2Key = CngKey.Create(CngAlgorithm.ECDsaP384);
        private static readonly CngKey Player3Key = CngKey.Create(CngAlgorithm.ECDsaP384);

        private async Task RunPlayer1()
        {
            var player1 = Player.CreateMyPlayer(Player1Name, Player1Key);

            var player2Client = new TcpClient();
            await player2Client.ConnectAsync(Localhost, Player2ListenPort);
            var player2 = Player.CreateOtherPlayer(Player2Name, player2Client.GetStream(), Player2Key);

            var player3Listener = new TcpListener(IPAddress.Any, Player1ListenPort);
            player3Listener.Start();
            var player3Client = await player3Listener.AcceptTcpClientAsync();
            var player3 = Player.CreateOtherPlayer(Player3Name, player3Client.GetStream(), Player3Key);

            var players = new List<Player>(){player1, player2, player3};
            var messageManager = new MessageManagerBuilder(PlayerIdentifyMode.Index)
                .WithMessageTypes<PokerMessageType>().Build<IPokerMessage>(players);
            await messageManager.BroadcastMessage(new PokerMoveMessage(PokerPlayerMoveType.Fold, 0));
        }

        private async Task RunPlayer2()
        {
            var player2 = Player.CreateMyPlayer(Player2Name, Player2Key);

            var player1Listener = new TcpListener(IPAddress.Any, Player2ListenPort);
            player1Listener.Start();
            var player1Client = await player1Listener.AcceptTcpClientAsync();
            player1Listener.Start();
            var player1 = Player.CreateOtherPlayer(Player1Name, player1Client.GetStream(), Player1Key);

            var player3Client = new TcpClient();
            await player3Client.ConnectAsync(Localhost, Player3ListenPort); // connect to player 1
            var player3 = Player.CreateOtherPlayer(Player3Name, player3Client.GetStream(), Player3Key);

            var players = new List<Player>() { player1, player2, player3 };
            var messageManager = new MessageManagerBuilder(PlayerIdentifyMode.Index)
                .WithMessageTypes<PokerMessageType>().Build<IPokerMessage>(players);

            var moveMessage = await messageManager.ReadMessageFrom<PokerMoveMessage>(player1);
        }


        private async Task RunPlayer3()
        {
            var player3 = Player.CreateMyPlayer(Player3Name, Player3Key);

            var player2Listener = new TcpListener(IPAddress.Any, Player3ListenPort);
            player2Listener.Start();
            var player2Client = await player2Listener.AcceptTcpClientAsync();
            var player2 = Player.CreateOtherPlayer(Player2Name, player2Client.GetStream(), Player2Key);

            var player1Client = new TcpClient();
            await player1Client.ConnectAsync(Localhost, Player1ListenPort); // connect to player 1
            var player1 = Player.CreateOtherPlayer(Player1Name, player1Client.GetStream(), Player1Key);

            var players = new List<Player>() { player1, player2, player3 };
            var messageManager = new MessageManagerBuilder(PlayerIdentifyMode.Index)
                .WithMessageTypes<PokerMessageType>().Build<IPokerMessage>(players);

            var betMessage = await messageManager.ReadMessageFrom<PokerMoveMessage>(player1);
        }
    }
}