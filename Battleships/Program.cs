using System;
using System.Collections.Generic;

namespace Battleships;

internal class Program
{
    static void Main(string[] args)
    {
        GameBoard gameBoard;
        while (true) //random initialization, i am not letting GameBoard to have invalid state, so the exception is thrown
        {
            try
            {
                var max = GameBoard.BoardSize - 1;
                gameBoard = new GameBoard(new List<Ship>
                {
                    new Battleship(ShipPosition.Random(max, max)),
                    new Destroyer(ShipPosition.Random(max, max)),
                    new Destroyer(ShipPosition.Random(max, max)),
                });
                break;
            }
            catch (ArgumentException) { }
        }

        var game = new Game(gameBoard, 
            displayLineAction: Console.WriteLine,
            clearDisplayAction: Console.Clear,
            userInputSelector: Console.ReadLine);

        game.RunLoop();
    }
}