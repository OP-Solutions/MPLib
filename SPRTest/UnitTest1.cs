using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPRTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var client1 = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5646));
            var client2 = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5647));
            var signaler = new ManualResetEvent(false);
            var signaler2 = new ManualResetEvent(false);

            Task.Run(() =>
            {
                signaler.WaitOne();
                client1.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5647));
                using (var stream = client1.GetStream())
                {
                    using (var writer = new StreamWriter(stream, Encoding.ASCII))
                    {
                        writer.Write("chudo xom saocrebaa \n");
                    }
                    stream.Flush();
                }
                signaler2.WaitOne();
            });
            Task.Run(() =>
            {
                signaler.WaitOne();
                client2.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5646));
                using (var stream = client2.GetStream())
                {
                    using (var reader = new StreamReader(stream, Encoding.ASCII))
                    {
                        var text = reader.ReadLine();
                        Console.WriteLine($"Got Message: {text}");
                        signaler2.Set();
                    }
                }
            });

            Thread.Sleep(1000);
            signaler.Set();
            signaler2.WaitOne();

        }
    }
}
