using System;
using System.Threading.Tasks;
using MPLib.Models.Lobby;

namespace MPLib.Networking
{
    /// <summary>
    /// This class is responsible to create table or find all created tables of specific game within local network.
    /// When table is created by user, user's device responds on udp "table search request" with table uuid and name
    /// Players can join to other player's created table,
    /// after join they also respond to "table search request" with joined table uuid and name
    /// Table's uuid should be only business layer identifier of table, name is just for showing to user.
    /// "table search request" referred earlier ^ is udp broadcast packet in case of LAN
    /// </summary>
    class TableManager
    {
        
        /// <summary>
        /// Create a table with specified name
        /// After table created device should respond discovery request with that table name and uuid
        /// After creating table it should be tracked in real time,
        /// for example when new player joins or leaves, <see cref="GameTableData.PlayersJoinedIn"/> should be updated
        /// </summary>
        /// <param name="gameName">game name/identifier</param>
        /// <param name="tableName">table name. table will be discovered with that name in lan</param>
        /// <returns></returns>
        public GameTableData CreateTable(string gameName, string tableName)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Discovers all table on lan network, with udp broadcast
        /// You can receive several response to this discovery request with same table uuid,
        /// that just means several player joined to that same table.
        /// After getting table list, their player list should be dynamically tracked (<seealso cref="CreateTable"/>)
        /// </summary>
        /// <param name="gameName"> game name/identifier </param>
        /// <returns></returns>
        public Task<GameTableData[]> DiscoverAllLanTables(string gameName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Joins specified table
        /// Table can/should be discovered (for example with <see cref="DiscoverAllTables"/>) before calling this method
        /// After joining device should respond discovery request with that table name and uuid
        /// Also after joining table should be tracked as in case of creating. <seealso cref="CreateTable"/>
        /// </summary>
        /// <param name="tableUuid">uuid identifier of table to join <see cref="GameTableData.TableUuid"/></param>
        /// <returns></returns>
        public Task JoinTable(string tableUuid)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Leaves table. (with same principle as <see cref="JoinTable"/>)
        /// </summary>
        /// <param name="tableUud"></param>
        /// <returns></returns>
        public Task LeaveTable(string tableUud)
        {
            throw new NotImplementedException();
        }
    }

    
}
