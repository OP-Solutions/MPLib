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
            var stream = remoteClient.Stream;
            var toWrite = Encoding.ASCII.GetBytes(messageToSend);
            stream.Write(toWrite,  0, toWrite.Length);
            var toReceive = new byte[messageToReceive.Length];
            var receivedCount = 0;
            while (receivedCount < toReceive.Length)
            {
                receivedCount += stream.Read(toReceive, receivedCount, toReceive.Length - receivedCount);
            }
            var receivedStr = Encoding.ASCII.GetString(toReceive);
            Console.WriteLine($"Received: {receivedStr}");
            Assert.AreEqual(messageToReceive, receivedStr);
        }
    }
}
