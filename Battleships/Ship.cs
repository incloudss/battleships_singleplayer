using System.Collections.Generic;
using System.Linq;

namespace Battleships;

public class Battleship : Ship
{
    public Battleship(ShipPosition position) : base(position, segmentsCount: 5) { }
}

public class Destroyer : Ship
{
    public Destroyer(ShipPosition position) : base(position, segmentsCount: 4) { }
}

public abstract class Ship
{
    protected Ship(ShipPosition position, int segmentsCount)
    {
        _segmentsCount = segmentsCount;
        _position = position;
    }

    public string Name => GetType().Name;
    public bool Occupies(Coordinates coords) => GetOccupiedCoords().Any(x => x == coords);
    public void MarkHit() { _hitSegmentsCount += 1; }
    public bool IsSunken => _segmentsCount == _hitSegmentsCount;
    public IEnumerable<Coordinates> GetOccupiedCoords()
    {
        yield return _position.SternCoordinates;

        var segmentPosition = _position.SternCoordinates;
        for (var i = 0; i < _segmentsCount - 1; i++)
        {
            segmentPosition = segmentPosition.GetAdjacent(_position.Bearing);
            yield return segmentPosition;
        }
    }

    private readonly int _segmentsCount;
    private int _hitSegmentsCount;
    private readonly ShipPosition _position;
}