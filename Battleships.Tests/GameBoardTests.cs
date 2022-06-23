using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Battleships.Tests
{
    public class GameBoardTests
    {
        [Test]
        public void GivenCorrectlyPositionedShips_WhenCreatingGameBoard_ThenTheGameboardShouldBeCreated()
        {
            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(3, 4), Bearing.Down)),
                new Destroyer(new ShipPosition(new Coordinates(1, 3), Bearing.Right)),
                new Battleship(new ShipPosition(new Coordinates(8, 9), Bearing.Left)),
            };
            Assert.DoesNotThrow(() => new GameBoard(ships));
        }

        [TestCase(0, 0, Bearing.Up)]
        [TestCase(0, 1, Bearing.Up)]
        [TestCase(0, 2, Bearing.Up)]
        [TestCase(1, 9, Bearing.Down)]
        [TestCase(1, 8, Bearing.Down)]
        [TestCase(1, 7, Bearing.Down)]
        [TestCase(0, 3, Bearing.Left)]
        [TestCase(1, 3, Bearing.Left)]
        [TestCase(2, 3, Bearing.Left)]
        [TestCase(9, 3, Bearing.Right)]
        [TestCase(8, 3, Bearing.Right)]
        [TestCase(7, 3, Bearing.Right)]
        public void GivenShipOutsideOfTheBoard_WhenCreatingGameBoard_ThenExceptionShouldBeThrown(int x, int y, Bearing bearing)
        {
            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(x, y), bearing)),
            };

            Assert.Throws<ArgumentException>(() => new GameBoard(ships));
        }

        [Test]
        public void GivenOverlappingShips_WhenCreatingGameBoard_ThenExceptionShouldBeThrown()
        {
            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(4, 4), Bearing.Down)),
                new Battleship(new ShipPosition(new Coordinates(2, 6), Bearing.Right))
            };

            Assert.Throws<ArgumentException>(() => new GameBoard(ships));
        }

        [Test]
        public void GivenEmptyShips_WhenCreatingGameBoard_ThenExceptionShouldBeThrown()
        {
            Assert.Throws<ArgumentException>(() => new GameBoard(new List<Ship>()));
        }

        [Test]
        public void GivenGameBoard_WhenShootingAtTheSameCoordinatesTwice_ThenExceptionShouldBeThrown()
        {
            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(3, 4), Bearing.Down)),
                new Destroyer(new ShipPosition(new Coordinates(1, 3), Bearing.Right)),
                new Battleship(new ShipPosition(new Coordinates(8, 9), Bearing.Left)),
            };

            var gameBoard = new GameBoard(ships);
            var shootCoords = new Coordinates(0, 0);
            gameBoard.ShootAt(shootCoords);

            Assert.Throws<ArgumentException>(() => gameBoard.ShootAt(shootCoords));
        }

        [Test]
        public void GivenGameBoard_WhenShootingAtTheCoordinatesOutsideOfTheBoard_ThenExceptionShouldBeThrown()
        {
            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(3, 4), Bearing.Down)),
            };

            var gameBoard = new GameBoard(ships);

            Assert.Throws<ArgumentException>(
                () => gameBoard.ShootAt(new Coordinates(GameBoard.BoardSize + 1, 0)));
        }

        [Test]
        public void GivenGameBoard_WhenShootingAndHittingShip_ThenShootResultShouldBeHitWithCorrectShipName()
        {
            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(3, 4), Bearing.Down)),
            };

            var gameBoard = new GameBoard(ships);
            var shootResult = gameBoard.ShootAt(new Coordinates(3, 5));

            Assert.True(shootResult.WasHit);
            Assert.AreEqual(shootResult.ShipName, "Destroyer");
        }

        [Test]
        public void GivenGameBoard_WhenShootingAndSunkingShip_ThenShootResultShouldBeHitAndSunkenWithCorrectShipName()
        {
            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(3, 4), Bearing.Down)),
            };

            var gameBoard = new GameBoard(ships);
            gameBoard.ShootAt(new Coordinates(3, 4));
            gameBoard.ShootAt(new Coordinates(3, 5));
            gameBoard.ShootAt(new Coordinates(3, 6));
            var sunkingShootResult = gameBoard.ShootAt(new Coordinates(3, 7));

            Assert.True(sunkingShootResult.WasHit);
            Assert.True(sunkingShootResult.ShipSunken);
            Assert.AreEqual(sunkingShootResult.ShipName, "Destroyer");
        }

        [Test]
        public void GivenGameBoard_WhenShootingAndMissing_ThenShootResultShouldBeMiss()
        {
            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(3, 4), Bearing.Down)),
            };

            var gameBoard = new GameBoard(ships);
            var shootResult = gameBoard.ShootAt(new Coordinates(3, 1));

            Assert.False(shootResult.WasHit);
        }

        [Test]
        public void GivenGameBoardWithAllSunkenShips_WhenCheckingIfAllShipsAreSunken_ThenItShouldReturnTrue()
        {
            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(3, 4), Bearing.Down)),
                new Destroyer(new ShipPosition(new Coordinates(1, 2), Bearing.Right)),
            };

            var gameBoard = new GameBoard(ships);
            gameBoard.ShootAt(new Coordinates(3, 4));
            gameBoard.ShootAt(new Coordinates(3, 5));
            gameBoard.ShootAt(new Coordinates(3, 6));
            gameBoard.ShootAt(new Coordinates(3, 7));

            gameBoard.ShootAt(new Coordinates(1, 2));
            gameBoard.ShootAt(new Coordinates(2, 2));
            gameBoard.ShootAt(new Coordinates(3, 2));
            gameBoard.ShootAt(new Coordinates(4, 2));

            Assert.True(gameBoard.AreAllShipsSunken);
        }


        [Test]
        public void GivenGameBoardWithNotSunkenShips_WhenCheckingIfAllShipsAreSunken_ThenItShouldReturnTrue()
        {
            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(3, 4), Bearing.Down)),
                new Destroyer(new ShipPosition(new Coordinates(1, 2), Bearing.Right)),
            };

            var gameBoard = new GameBoard(ships);
            gameBoard.ShootAt(new Coordinates(3, 4));
            gameBoard.ShootAt(new Coordinates(3, 5));
            gameBoard.ShootAt(new Coordinates(3, 6));
            gameBoard.ShootAt(new Coordinates(3, 7));

            Assert.False(gameBoard.AreAllShipsSunken);
        }

        [Test]
        public void GivenFreshGameBoard_WhenGettingView_ThenTheViewShouldBeEmptyAndCorrect()
        {
            var expectedView = @"BATTLESHIPS GAME
  1  2  3  4  5  6  7  8  9  10
A   |  |  |  |  |  |  |  |  |  |
  ------------------------------
B   |  |  |  |  |  |  |  |  |  |
  ------------------------------
C   |  |  |  |  |  |  |  |  |  |
  ------------------------------
D   |  |  |  |  |  |  |  |  |  |
  ------------------------------
E   |  |  |  |  |  |  |  |  |  |
  ------------------------------
F   |  |  |  |  |  |  |  |  |  |
  ------------------------------
G   |  |  |  |  |  |  |  |  |  |
  ------------------------------
H   |  |  |  |  |  |  |  |  |  |
  ------------------------------
I   |  |  |  |  |  |  |  |  |  |
  ------------------------------
J   |  |  |  |  |  |  |  |  |  |
  ------------------------------";

            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(3, 4), Bearing.Down)),
            };

            var gameBoard = new GameBoard(ships);
            
            Assert.AreEqual(gameBoard.View, expectedView);
        }

        [Test]
        public void GivenGameBoard_WhenShootingAndMissing_ThenViewShouldDisplayMissAtTheShotCoordinates()
        {
            var expectedView = @"BATTLESHIPS GAME
  1  2  3  4  5  6  7  8  9  10
A o |  |  |  |  |  |  |  |  |  |
  ------------------------------
B   |  |  |  |  |  |  |  |  |  |
  ------------------------------
C   |  |  |  |  |  |  |  |  |  |
  ------------------------------
D   |  |  |  |o |  |o |  |  |  |
  ------------------------------
E   |  |  |  |  |  |  |  |  |  |
  ------------------------------
F   |  |  |  |  |  |  |  |  |  |
  ------------------------------
G   |  |  |  |  |  |  |  |  |  |
  ------------------------------
H   |  |  |  |  |  |  |  |  |  |
  ------------------------------
I   |  |  |  |  |  |  |  |  |  |
  ------------------------------
J   |  |  |  |  |  |  |  |  |  |
  ------------------------------";


            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(new Coordinates(3, 4), Bearing.Down)),
            };

            var gameBoard = new GameBoard(ships);
            gameBoard.ShootAt(Coordinates.FromA1Notation("D5"));
            gameBoard.ShootAt(Coordinates.FromA1Notation("D7"));
            gameBoard.ShootAt(Coordinates.FromA1Notation("A1"));

            Assert.AreEqual(gameBoard.View, expectedView);
        }

        [Test]
        public void GivenGameBoard_WhenShootingAndHitting_ThenViewShouldDisplayHitAtTheShotCoordinates()
        {
            var expectedView = @"BATTLESHIPS GAME
  1  2  3  4  5  6  7  8  9  10
A   |  |  |  |  |  |  |  |  |  |
  ------------------------------
B   |  |  |  |  |  |  |  |x |  |
  ------------------------------
C   |  |  |  |  |  |  |  |x |  |
  ------------------------------
D   |  |  |  |  |  |  |  |  |  |
  ------------------------------
E   |  |  |  |  |  |  |  |  |  |
  ------------------------------
F   |  |  |  |  |  |  |  |  |  |
  ------------------------------
G   |  |  |  |  |  |  |  |  |  |
  ------------------------------
H   |  |  |  |  |  |  |  |  |  |
  ------------------------------
I   |  |  |  |  |  |  |  |  |  |
  ------------------------------
J   |  |  |  |  |  |  |  |  |  |
  ------------------------------";

            var ships = new List<Ship>
            {
                new Destroyer(new ShipPosition(Coordinates.FromA1Notation("A9"), Bearing.Down)),
            };

            var gameBoard = new GameBoard(ships);
            gameBoard.ShootAt(Coordinates.FromA1Notation("B9"));
            gameBoard.ShootAt(Coordinates.FromA1Notation("C9"));

            Assert.AreEqual(gameBoard.View, expectedView);
        }
    }
}