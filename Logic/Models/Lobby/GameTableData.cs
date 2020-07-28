using System.Collections.Generic;
using System.ComponentModel;

namespace MPLib.Models.Lobby
{
    class GameTableData
    {
        public string TableName { get; }

        public string TableUuid { get; }
        public BindingList<MinimalPlayerData> PlayersJoinedIn { get; }

        public GameTableData(string tableName, string tableUuid, List<MinimalPlayerData> playersJoinedIn)
        {
            TableName = tableName;
            TableUuid = tableUuid;
            PlayersJoinedIn = new BindingList<MinimalPlayerData>(playersJoinedIn);
        }
    }
}
