using System;
using NUnit.Framework;

namespace Battleships.Tests
{
    public class CoordinatesTests
    {
        [TestCase("A1")]
        [TestCase("A10")]
        [TestCase("J1")]
        [TestCase("J10")]
        [TestCase("G10")]
        [TestCase("Z1")]
        [TestCase("a1")]
        public void GivenCorrectA1Notation_WhenCreatingCoordinates_ThenItShouldBeCreated(string value)
        {
            Assert.DoesNotThrow(() => Coordinates.FromA1Notation(value));
        }

        [TestCase("AA1")]
        [TestCase("A-10")]
        [TestCase("1")]
        [TestCase("10")]
        [TestCase("*A1")]
        [TestCase("asdf")]
        public void GivenIncorrectA1Notation_WhenCreatingCoordinates_ThenItShouldThrowException(string value)
        {
            Assert.Throws<ArgumentException>(() => Coordinates.FromA1Notation(value));
        }

        [Test]
        public void GivenCoordinates_WhenGettingAdjacentCoordinate_ThenTheAdjacentCoordinatesShouldBeCorrect()
        {
            var coords = new Coordinates(3, 3);

            var onTheLeft = coords.GetAdjacent(Bearing.Left);
            var onTheRight = coords.GetAdjacent(Bearing.Right);
            var above = coords.GetAdjacent(Bearing.Up);
            var below = coords.GetAdjacent(Bearing.Down);

            Assert.AreEqual(onTheLeft, new Coordinates(2, 3));
            Assert.AreEqual(onTheRight, new Coordinates(4, 3));
            Assert.AreEqual(above, new Coordinates(3, 2));
            Assert.AreEqual(below, new Coordinates(3, 4));
        }
    }
}
