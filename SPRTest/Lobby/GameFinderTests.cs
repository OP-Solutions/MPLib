using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPR.Lobby;

namespace SPRTest.Lobby
{
    [TestClass()]
    public class GameFinderTests
    {

        private void RequestNewGame()
        {
            var gameFinder = new GameFinder();
            gameFinder.OnGameFound += players =>
            {
                Console.WriteLine(players.Length);
            };
            gameFinder.FindGame(4, 4.4);
        }


        [TestMethod()]
        public void FindGameTest()
        {
            for (int i = 1; i < 10; i++)
            {
                RequestNewGame();
            }
            Thread.Sleep(15000);

        }
    }
}