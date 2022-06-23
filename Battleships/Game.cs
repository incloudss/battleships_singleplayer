using System;

namespace Battleships
{
    public class Game
    {
        private readonly GameBoard _gameBoard;
        private readonly Action<string> _displayLineAction;
        private readonly Action _clearDisplayAction;
        private readonly Func<string> _userInputSelector;

        public Game(GameBoard gameBoard, 
            Action<string> displayLineAction,
            Func<string> userInputSelector,
            Action clearDisplayAction)
        {
            _gameBoard = gameBoard;
            _displayLineAction = displayLineAction;
            _userInputSelector = userInputSelector;
            _clearDisplayAction = clearDisplayAction;
        }

        public void RunLoop()
        {
            do //game loop
            {
                _clearDisplayAction();
                _displayLineAction(_gameBoard.View);

                if (_gameBoard.AreAllShipsSunken) break;

                _displayLineAction("Please type coordinates to shoot at: ");

                try
                {
                    var coordinates = Coordinates.FromA1Notation(_userInputSelector());
                    var shotResult = _gameBoard.ShootAt(coordinates);
                    _displayLineAction((shotResult.WasHit ? "HIT! " : "MISS. ")
                                       + shotResult.ShipName
                                       + (shotResult.ShipSunken ? " Sunken!" : "")
                                       + " Press enter to get next shoot...");
                    _userInputSelector();
                }
                catch (ArgumentException e)
                {
                    _displayLineAction(e.Message + ". Press enter to retry...");
                    _userInputSelector();
                }

            } while (true);

            _displayLineAction("Game ended. You won unfair battle. Congratulations!");
        }
    }
}
