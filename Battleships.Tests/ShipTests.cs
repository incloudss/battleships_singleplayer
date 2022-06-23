using System.Linq;
using NUnit.Framework;

namespace Battleships.Tests
{
    public class ShipTests
    {
        [Test]
        public void
            GivenShipWithSternAtCoordinatesWithBearing_WhenGettingOccupiedCoords_ThenOccupiedCoordsShouldBeCorrect()
        {
            var ship = new Destroyer(new ShipPosition(new Coordinates(2, 2), Bearing.Right));

            var occupiedPositions = ship.GetOccupiedCoords().ToList();
            Assert.AreEqual(occupiedPositions.Count, 4);

            Assert.True(occupiedPositions.Contains(new Coordinates(2, 2)));
            Assert.True(occupiedPositions.Contains(new Coordinates(3, 2)));
            Assert.True(occupiedPositions.Contains(new Coordinates(4, 2)));
            Assert.True(occupiedPositions.Contains(new Coordinates(5, 2)));
        }

        [Test]
        public void
            GivenShipWithSternAtCoordinatesWithBearing_WhenCheckingIfOccupiesCoordinates_ThenOccupiesResultShouldBeCorrect()
        {
            var ship = new Destroyer(new ShipPosition(new Coordinates(2, 2), Bearing.Right));

            Assert.True(ship.Occupies(new Coordinates(3, 2)));
            Assert.False(ship.Occupies(new Coordinates(3, 3)));
        }

        [Test]
        public void GivenShip_WhenHittingAllOfItsSegments_ThenItShouldBeSunken()
        {
            var ship = new Destroyer(new ShipPosition(new Coordinates(2, 2), Bearing.Right));
            
            Assert.False(ship.IsSunken);

            ship.MarkHit(); //destroyer has 4 segments
            ship.MarkHit();
            ship.MarkHit();
            ship.MarkHit();

            Assert.True(ship.IsSunken);
        }

        [Test]
        public void GivenShip_WhenGettingItsName_ThenItShouldBeCorrect()
        {
            var ship = new Destroyer(new ShipPosition(new Coordinates(2, 2), Bearing.Right));
            Assert.AreEqual(ship.Name, "Destroyer");
        }
    }
}
