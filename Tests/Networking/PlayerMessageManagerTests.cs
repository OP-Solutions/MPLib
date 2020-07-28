using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using MPLib.Models.Games;
using Xunit;

namespace Tests.Networking
{
    public class PlayerMessageManagerTests
    {



        [Fact]
        public void PlayerMessageManagerTest()
        {
            
        }


        private const int TestPort = 6069;

        private async Task RunPlayer1()
        {
            var listener = new TcpListener(IPAddress.Any, TestPort);
            var client = await listener.AcceptTcpClientAsync();
            var stream = client.GetStream();
        }

        private async Task RunPlayer2()
        {
            var client = new TcpClient("127.0.0.1", TestPort);
            await client.ConnectAsync("127.0.0.1", TestPort);
            var stream = client.GetStream();
        }
    }
}