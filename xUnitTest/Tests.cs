using Xunit;

using ToyRobot.Models;


namespace xUnitTests
{
    public class to_Robot
    {
        /* ... Testing CheckValidity ... */
        [Theory]
        [InlineData(0, 0, "North")]
        [InlineData(1, 1, "south")]
        [InlineData(2, 2, "EAsT")]
        [InlineData(3, 3, "WesT")]
        [InlineData(4, 4, "SOUTH")]
        public void Valid_CheckValidity(int x, int y, string face)
        {
            Robot robot = new Robot();
            Assert.True(robot.CheckValidity(x, y, face));
        }

        [Theory]
        [InlineData(-1, 0, "North")]
        [InlineData(1, 1000, "South")]
        [InlineData(2, 2, "Moon")]
        [InlineData(3, 3, "nowhere")]
        [InlineData(50, -10, "East")]
        [InlineData(-0, -10, "East")]
        public void Invalid_CheckValidity(int x, int y, string face)
        {
            Robot robot = new Robot();
            Assert.False(robot.CheckValidity(x, y, face));
        }

        [Fact]
        public void Test_Digit_to_Face()
        {
            Robot robot = new Robot();
            Assert.True(robot.CheckValidity(0, 0, "0"));
            Assert.True(robot.CheckValidity(0, 0, "16"));
            Assert.True(robot.CheckValidity(0, 0, "-4"));
        }

        /* ... Testing PLACE Action ... */
        [Theory]
        [InlineData(0, 0, "North")]
        [InlineData(1, 1, "south")]
        [InlineData(2, 2, "EAsT")]
        [InlineData(3, 3, "WesT")]
        public void Valid_PlaceAction(int x, int y, string face)
        {
            Robot robot = new Robot();
            Assert.True(robot.Place(x, y, face));
            Assert.True(robot.IsValid());
        }

        [Theory]
        [InlineData(0, 0, "0")]
        [InlineData(1, 1, "-4")]
        [InlineData(2, 2, "16")]
        public void Valid_PlaceAction_DigitFace(int x, int y, string face)
        {
            Robot robot = new Robot();
            Assert.True(robot.Place(x, y, face));
            Assert.True(robot.IsValid());
            Assert.True(robot.Face.ToString() == "NORTH");
        }


        [Theory]
        [InlineData(-1, 0, "North")]
        [InlineData(1, 1000, "South")]
        [InlineData(2, 2, "Moon")]
        [InlineData(3, 3, "nowhere")]
        [InlineData(50, -10, "East")]
        [InlineData(-0, -10, "East")]
        public void Invalid_PlaceAction(int x, int y, string face)
        {
            Robot robot = new Robot();
            Assert.False(robot.Place(x, y, face));
        }


        /* ... Testing MOVE Action ... */
        [Theory]
        [InlineData(0, 0, "North")]
        [InlineData(1, 1, "south")]
        [InlineData(2, 2, "EAsT")]
        [InlineData(3, 3, "WesT")]
        public void Valid_MoveAction(int x, int y, string face)
        {
            Robot robot = new Robot();
            robot.Place(x, y, face);
            Assert.True(robot.Move());
        }

        [Theory]
        [InlineData(0, 0, "South")]
        [InlineData(0, 1, "west")]
        [InlineData(4, 4, "EAsT")]
        [InlineData(0, 4, "North")]
        public void Invalid_MoveAction(int x, int y, string face)
        {
            Robot robot = new Robot();
            robot.Place(x, y, face);
            Assert.False(robot.Move());
        }


        /* ... Testing LEFT Action ... */
        [Fact]
        public void Robot_LEFT_Action()
        {
            Robot robot = new Robot();
            robot.Place(2, 2, "NORTH");

            robot.Right();
            Assert.True(robot.Face.ToString() == "EAST", "North to East");

            robot.Right();
            Assert.True(robot.Face.ToString() == "SOUTH", "East to South");

            robot.Right();
            Assert.True(robot.Face.ToString() == "WEST", "South to West");

            robot.Right();
            Assert.True(robot.Face.ToString() == "NORTH", "West to North");
        }


        /* ... Tests on RIGHT Action ... */
        [Fact]
        public void Robot_RIGHT_Action()
        {
            Robot robot = new Robot();
            robot.Place(2, 2, "NORTH");

            robot.Left();
            Assert.True(robot.Face.ToString() == "WEST", "North to West");

            robot.Left();
            Assert.True(robot.Face.ToString() == "SOUTH", "West to South");

            robot.Left();
            Assert.True(robot.Face.ToString() == "EAST", "South to East");

            robot.Left();
            Assert.True(robot.Face.ToString() == "NORTH", "East to North");
        }


        /* ... Tests on REPORT Action ... */
        [Theory]
        [InlineData(0, 0, "North")]
        [InlineData(1, 1, "south")]
        [InlineData(2, 2, "EAsT")]
        [InlineData(3, 3, "WesT")]
        [InlineData(4, 4, "WesT")]
        public void Valid_ReportAction(int x, int y, string face)
        {
            Robot robot = new Robot();
            robot.Place(x, y, face);
            Assert.True(robot.Report().Length > 0);
        }

    }
}
