using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using MPLib.Core.Game.Poker.Messaging;
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
            
        }


        private const int Player1ListenPort = 6169;
        private const int Player2ListenPort = 6170;
        private const int Player3ListenPort = 6171;
        private const string Player1Name = "TestPlyer1";
        private const string Player2Name = "TestPlayer2";
        private const string Player3Name = "TestPlayer3";

        private async Task RunPlayer1()
        {
            var player1 = Player.CreateMyPlayer(Player1Name);

            var player2Client = new TcpClient();
            await player2Client.ConnectAsync(Localhost, Player2ListenPort);
            var player2 = Player.CreateOtherPlayer(Player2Name, player2Client.GetStream());

            var player3Listener = new TcpListener(IPAddress.Any, Player1ListenPort);
            var player3Client = await player3Listener.AcceptTcpClientAsync();
            var player3 = Player.CreateOtherPlayer(Player3Name, player3Client.GetStream());

            var players = new List<Player>(1);
        }

        private async Task RunPlayer2()
        {
            var player2 = Player.CreateMyPlayer(Player2Name);

            var player1Listener = new TcpListener(IPAddress.Any, Player2ListenPort);
            var player1Client = await player1Listener.AcceptTcpClientAsync();
            var player1 = Player.CreateOtherPlayer(Player1Name, player1Client.GetStream());

            var player3Client = new TcpClient();
            await player3Client.ConnectAsync(Localhost, Player3ListenPort); // connect to player 1
            var player3 = Player.CreateOtherPlayer(Player3Name, player3Client.GetStream());

        }


        private async Task RunPlayer3()
        {
            var player3 = Player.CreateMyPlayer(Player3Name);

            var player2Listener = new TcpListener(IPAddress.Any, Player2ListenPort);
            var player2Client = await player2Listener.AcceptTcpClientAsync();
            var player2 = Player.CreateOtherPlayer(Player2Name, player2Client.GetStream());

            var player1Client = new TcpClient();
            await player1Client.ConnectAsync(Localhost, Player1ListenPort); // connect to player 1
            var player1 = Player.CreateOtherPlayer(Player1Name, player1Client.GetStream());
        }
    }
}