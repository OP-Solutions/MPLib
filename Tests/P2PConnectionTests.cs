using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class P2PConnectionTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public P2PConnectionTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestHolePunching()
        {
            var client1 = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5646));
            var client2 = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5647));
            var signaler = new ManualResetEvent(false);
            var signaler2 = new ManualResetEvent(false);
            const string testMessage = "Test message";

            Task.Run(() =>
            {
                signaler.WaitOne();
                client1.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5647));
                using var stream = client1.GetStream();
                using var writer = new StreamWriter(stream, Encoding.ASCII);
                writer.WriteLine(testMessage);
                writer.Flush();
                signaler2.WaitOne();
            });
            Task.Run(() =>
            {
                try
                {
                    signaler.WaitOne();
                    client2.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5646));
                    using var stream = client2.GetStream();
                    using var reader = new StreamReader(stream, Encoding.ASCII);
                    var text = reader.ReadLine();
                    _testOutputHelper.WriteLine($"Got Message: {text}");
                    Assert.True(text == testMessage, $"Expected: {testMessage}, Got: {text}");
                }
                finally
                {
                    signaler2.Set();
                }
            });

            Thread.Sleep(1000);
            signaler.Set();
            signaler2.WaitOne();

        }
    }
}
