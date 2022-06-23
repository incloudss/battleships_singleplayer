using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships.Tests
{
    public class GameTests
    {
        [Test]
        public async Task GivenGame_WhenSinkingAllShips_ThenTheGameIsEnded()
        {
            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(3, 4), Bearing.Down))
            };

            var gameBoard = new GameBoard(ships);
            var simulator = new Simulator();
            simulator.SimulatedUserInputs = new List<string>
            {
                "E4",
                "",
                "F4",
                "",
                "A1",
                "",
                "incorrectNotation",
                "",
                "F4",
                "",
                "G4",
                "",
                "H4",
                "",
            };

            var game = new Game(gameBoard,
                displayLineAction: simulator.HandleDisplayLineAction,
                clearDisplayAction: simulator.HandleClearDisplayScreenAction,
                userInputSelector: simulator.HandleUserInputSelector);


            simulator.RunGame(game);
            await simulator.Wait();

            Assert.True(simulator.HasExitedGameLoop);
            Assert.AreEqual(simulator.ClearedScreenActionsCount, 8);
            Assert.True(simulator.DisplayedLines.Count(x => x.Contains("BATTLESHIPS GAME")) == 8);
            Assert.True(simulator.DisplayedLines.Count(x => x == "Please type coordinates to shoot at: ") == 7);
            Assert.True(simulator.DisplayedLines.Count(x => x == "HIT! Destroyer Press enter to get next shoot...") == 3);
            Assert.True(simulator.DisplayedLines.Count(x => x == "MISS.  Press enter to get next shoot...") == 1);
            Assert.True(simulator.DisplayedLines.Count(x => x == "HIT! Destroyer Sunken! Press enter to get next shoot...") == 1);
            Assert.True(simulator.DisplayedLines.Count(x => x == "Game ended. You won unfair battle. Congratulations!") == 1);
            Assert.True(simulator.DisplayedLines.Count(x => x == "Incorrect A1 notation: incorrectNotation. Press enter to retry...") == 1);
            Assert.True(simulator.DisplayedLines.Count(x => x == "You already shot at those coordinates, please try another ones. Press enter to retry...") == 1);
        }
    }
}
