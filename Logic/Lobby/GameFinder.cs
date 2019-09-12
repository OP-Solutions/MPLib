using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPR.Models;
using SPR.Networking;
using WebSocketSharp;
// ReSharper disable PossibleNullReferenceException


namespace SPR.Lobby
{
    public class GameFinder
    {
        public delegate void FoundGameCallback(Player[] otherPlayers);

        public event FoundGameCallback OnGameFound;

        public GameFinder()
        {
            _ws = new WebSocket("ws://127.0.0.1:8080/ws");
        }

        public GameFinder(FoundGameCallback callback)
        {
            OnGameFound += callback;
        }

        private readonly WebSocket _ws;

        public void FindGame(int playerCount, double smallBlind)
        {


            _ws.OnMessage += (sender, e) =>
            {
                List<Player> lst = new List<Player>();
                dynamic parser = JObject.Parse(e.Data);
                foreach (dynamic user in parser.users)
                {
                    string ipAddr = user.ip_address;
                    string ethereumAddress = user.ethereum_address;
                    var player = new Player(ethereumAddress, IPAddress.Parse(ipAddr));
                    lst.Add(player);
                }

                OnGameFound(lst.ToArray());
            };

            _ws.Connect();
            var a = new JObject
            {
                ["type"] = "find_table",
                ["msg"] = new JObject()
                {
                    ["player_count"] = playerCount,
                    ["small_blind"] = smallBlind,
                }
            };
            _ws.Send(a.ToString());

        }

        ~GameFinder()
        {
            (_ws as IDisposable).Dispose();
        }
    }
}
