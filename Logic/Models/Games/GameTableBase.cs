using System.Collections.Generic;
using System.Threading.Tasks;

namespace EtherBetClientLib.Models.Games
{
    public abstract class GameTableBase<TPlayer, TMyPlayer>
    {
        public string Name { get; set; }

        public IReadOnlyList<TPlayer> Players => _players;

        public TMyPlayer MyPlayer { get; internal set; }

        public GameTableState State { get; internal set; }

        private List<TPlayer> _players = new List<TPlayer>();

    }

    public enum GameTableState
    {
        GameNotStarted,
        GameStarted,
        GameFinished,
    }
}
