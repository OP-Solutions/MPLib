using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SPR.Models;

namespace SPR.Lobby
{
    public class PlayerMeeting
    {
        public delegate void PlayerConnectedCallBack(Player player);

        public delegate void PlayerMeetingCompleted(Player[] otherPlayers);

        public string[] PlayerAddresses { get; }
        public event PlayerConnectedCallBack OnPlayerConnected;
        public event PlayerMeetingCompleted OnMeetingCompleted;

        public PlayerMeeting(string[] playerEthAddresses)
        {
            PlayerAddresses = playerEthAddresses;
        }

        public void StartMeeting()
        {
            throw new NotImplementedException();
        }
    }
}
