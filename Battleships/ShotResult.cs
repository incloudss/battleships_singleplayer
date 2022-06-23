namespace Battleships;

public class ShotResult
{
    private ShotResult(bool wasHit, string shipName, bool shipSunken)
    {
        WasHit = wasHit;
        ShipName = shipName;
        ShipSunken = shipSunken;
    }

    public bool WasHit { get; }
    public string ShipName { get; }
    public bool ShipSunken { get; }
    public static ShotResult Hit(string shipName, bool shipSunken) => new(true, shipName, shipSunken);
    public static ShotResult Miss() => new(false, null, false);
}