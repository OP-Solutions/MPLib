using System;
using EtherBetClientLib.Models;

namespace EtherBetClientLib.Lobby
{
    public class PlayerMeeting
    {
        public delegate void PlayerConnectedCallBack(PlayerBase player);

        public delegate void PlayerMeetingCompleted(PlayerBase[] otherPlayers);

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
