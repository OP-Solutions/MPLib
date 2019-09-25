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
        public delegate void FoundGameCallback(string[] ethAddresses);

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


            

            _ws.Connect();
            _ws.Login().WaitOne();
            _ws.OnMessage += (sender, e) =>
            {
                var lst = new List<string>();
                dynamic parser = JObject.Parse(e.Data);
                foreach (dynamic user in parser.users)
                {
                    string ethereumAddress = user.ethereum_address;
                    lst.Add(ethereumAddress);
                }

                OnGameFound(lst.ToArray());
            };
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
