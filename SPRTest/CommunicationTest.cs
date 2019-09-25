using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPR.Models;
using SPR.Networking;

namespace SPRTest
{
    [TestClass]
    public class CommunicationTest
    {
        [DataTestMethod]
        [DataRow("62.168.171.183", 8789, "Chudo123", "bakurits")] // from me // kukur home ip 62.168.171.183
        //[DataRow("31.146.149.134, 8788", "bakurits", "Chudo123")] // from kukur
        public void TestP2P(string targetIp, int targetPort, string messageToSend, string messageToReceive)
        {
            var remoteClient = new PlayerNetworkClient(null, new IPEndPoint(IPAddress.Parse(targetIp), targetPort));
            var time = DateTime.UtcNow;
            var time2 = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute + 1, 0);
            Thread.Sleep(time2 - time);
            Console.WriteLine($"Sending Connect: {DateTime.UtcNow.Second}.{DateTime.UtcNow.Millisecond}");
            remoteClient.Connect().Wait();
            var stream = remoteClient.GetStream();
            stream.SendMessage(messageToSend);
            var received = stream.ReceiveMessage();
            Console.WriteLine($"Received: {messageToReceive}");
            Assert.AreEqual(messageToReceive, received);
        }
    }
}
