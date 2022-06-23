using System;
using System.Text.RegularExpressions;

namespace Battleships;

public record Coordinates(int X, int Y)
{
    public Coordinates GetAdjacent(Bearing bearing)
    {
        return bearing switch
        {
            Bearing.Down => new Coordinates(X, Y + 1),
            Bearing.Left => new Coordinates(X - 1, Y),
            Bearing.Right => new Coordinates(X + 1, Y),
            Bearing.Up => new Coordinates(X, Y - 1),
            _ => throw new ArgumentOutOfRangeException(nameof(bearing), bearing, null)
        };
    }

    public static Coordinates FromA1Notation(string value)
    {
        var sanitizedValue = value.ToUpper().Trim();
        var regexMatch = Regex.Match(sanitizedValue, "^([A-Z])(\\d+)");
        if (sanitizedValue.Length < 2 || regexMatch.Groups.Count != 3)
            throw new ArgumentException($"Incorrect A1 notation: {value}");

        var letter = regexMatch.Groups[1].Value[0];
        var digit = regexMatch.Groups[2].Value;

        var x = int.Parse(digit) - 1;
        var y = Convert.ToInt32(letter) - 65; //Unicode maps to decimals A -> 65, B -> 66 ... Z -> 90
        return new Coordinates(x, y);
    }

    public static Coordinates Random(int maxX, int maxY)
    {
        var rand = new Random();
        return new Coordinates(rand.Next(maxX), rand.Next(maxY));
    }
}