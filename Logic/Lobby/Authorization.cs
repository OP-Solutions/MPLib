using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Nethereum.Hex.HexConvertors.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPR.Crypto.Signing;
using WebSocketSharp;

namespace SPR.Lobby
{
    static class Authorization
    {
        public static EventWaitHandle Login(this WebSocket ws)
        {
            var ev = new ManualResetEvent(false);
            ws.OnMessage += (sender, e) =>
            {
                dynamic data = JObject.Parse(e.Data);
                if (data.type != "authorization_random_data_message") return;
                
                var signature = Signer.Sign(Convert.FromBase64String((string)data.msg));
                var a = new JObject
                {
                    ["type"] = "user_credentials",
                    ["msg"] = new JObject()
                    {
                        ["signature"] = signature,
                        ["public_key"] = AppContext.Context.WalletKey.GetPubKey().ToHex()
                    }
                };
                ws.Send(a.ToString());
                ev.Set();
            };

            return ev;
        }
    }
}
