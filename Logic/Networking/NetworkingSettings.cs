using System.Collections.Generic;

namespace MPLib.Networking
{
    internal static class NetworkingSettings
    {
        public static Dictionary<int, IMessage> MessageTypes = new Dictionary<int, IMessage>();

        static NetworkingSettings()
        {
        }
    }
}
