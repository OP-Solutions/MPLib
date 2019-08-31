using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPR.Models;

namespace SPRTest
{
    class CommunicationTest
    {
        [DataTestMethod]
        [DataRow("217.147.224.170", 87, "Chudo123", "bakurits")] // from me
        [DataRow("94.100.237.197", 87, "bakurits", "Chudo123")] // from kukur
        public void TestP2P(string targetIp, int targetPort, string messageToSend, string messageToReceive)
        {
            var time = DateTime.UtcNow;
            var time2 = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute + 1, 0);
            Thread.Sleep(time2 - time);
            var remoteClient = new RemoteClient(null, null, new IPEndPoint(IPAddress.Parse(targetIp), targetPort));
            remoteClient.Connect();
            var stream = remoteClient.GetStream();
            stream.SendMessage(messageToSend);
            var received = stream.ReceiveMessage();
            Console.WriteLine($"Received: {messageToReceive}");
            Assert.AreEqual(messageToReceive, received);
        }
    }
}
