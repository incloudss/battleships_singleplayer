using System;

namespace Battleships;

public class ShipPosition
{
    public ShipPosition(Coordinates sternCoordinates, Bearing bearing)
    {
        SternCoordinates = sternCoordinates;
        Bearing = bearing;
    }

    public Coordinates SternCoordinates { get; }
    public Bearing Bearing { get; }

    public static ShipPosition Random(int maxX, int maxY)
    {
        return new ShipPosition(
            Coordinates.Random(maxX, maxY),
            (Bearing)new Random().Next(4));
    }
}