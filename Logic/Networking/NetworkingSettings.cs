using System;
using System.Collections.Generic;
using System.Text;
using EtherBetClientLib.Core.Game.Poker.Messaging;

namespace EtherBetClientLib.Networking
{
    internal static class NetworkingSettings
    {
        public static Dictionary<int, IMessage> MessageTypes = new Dictionary<int, IMessage>();

        static NetworkingSettings()
        {
        }
    }
}
