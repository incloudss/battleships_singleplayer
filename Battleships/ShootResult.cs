namespace Battleships;

public class ShootResult
{
    private ShootResult(bool wasHit, string shipName, bool shipSunken)
    {
        WasHit = wasHit;
        ShipName = shipName;
        ShipSunken = shipSunken;
    }

    public bool WasHit { get; }
    public string ShipName { get; }
    public bool ShipSunken { get; }
    public static ShootResult Hit(string shipName, bool shipSunken) => new(true, shipName, shipSunken);
    public static ShootResult Miss() => new(false, null, false);
}