using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships;

public class GameBoard
{
    public static readonly int BoardSize = 10;
    private readonly ICollection<Ship> _ships;
    private readonly IDictionary<Coordinates, FieldState> _fieldsState = new Dictionary<Coordinates, FieldState>();

    public GameBoard(ICollection<Ship> ships)
    {
        if (!ships.Any())
            throw new ArgumentException("Trying to create board with no ships on it");

        if (!CanPlace(ships))
            throw new ArgumentException("One of the ships has incorrect position");

        _ships = ships;
    }
    public bool AreAllShipsSunken => _ships.All(x => x.IsSunken);
    public string View => string.Join(Environment.NewLine, ViewLines());
    public ShotResult ShootAt(Coordinates coords)
    {
        if (_fieldsState.ContainsKey(coords))
            throw new ArgumentException("You already shot at those coordinates, please try another ones");

        if(IsOffBoard(coords))
            throw new ArgumentException("The coordinates are not within board");

        var hitShip = _ships.FirstOrDefault(x => x.Occupies(coords));

        if (hitShip == null)
        {
            _fieldsState.Add(coords, FieldState.Miss);
            return ShotResult.Miss();
        }

        hitShip.MarkHit();
        _fieldsState.Add(coords, FieldState.Hit);
        return ShotResult.Hit(hitShip.Name, hitShip.IsSunken);
    }

    private bool IsOffBoard(Coordinates coords) =>
        coords.X < 0 || coords.X >= BoardSize || coords.Y < 0 || coords.Y >= BoardSize;
    private IEnumerable<string> ViewLines()
    {
        yield return "BATTLESHIPS GAME";
        yield return "  1  2  3  4  5  6  7  8  9  10";
        for (int y = 0; y < BoardSize; y++)
        {
            var sb = new StringBuilder();
            sb.Append(Char.ConvertFromUtf32(y + 65) + " ");
            for (int x = 0; x < BoardSize; x++)
            {
                if (_fieldsState.TryGetValue(new Coordinates(x, y), out var fieldState))
                    sb.Append(fieldState == FieldState.Hit ? "x " : "o ");
                else
                    sb.Append("  ");

                sb.Append("|");
            }
            yield return sb.ToString();
            yield return "  ------------------------------";
        }
    }

    private bool CanPlace(ICollection<Ship> ships)
    {
        var occupiedCoordinates = ships.SelectMany(x => x.GetOccupiedCoords()).ToList();

        if (occupiedCoordinates.Any(IsOffBoard) || 
            occupiedCoordinates.Distinct().Count() != occupiedCoordinates.Count)
            return false;

        return true;
    }
}