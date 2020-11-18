namespace RobotSimulatorTest
{
    using RobotSimulatorService;
    using System.Drawing;
    using Xunit;

    public class ExecuteCommandTest
    {
        private readonly ITableTop tableTop = new TableTop();
        private readonly IExecuteCommand executeCommand;

        public ExecuteCommandTest()
        {
            executeCommand = new ExecuteCommand(tableTop);
        }

        public static TheoryData<Point, Orientation> validPlace
        {
            get
            {
                var data = new TheoryData<Point, Orientation>();
                data.Add(new Point { X = 0, Y = 0 }, Orientation.EAST);
                data.Add(new Point { X = 1, Y = 3 }, Orientation.WEST);
                data.Add(new Point { X = 2, Y = 2 }, Orientation.SOUTH);
                data.Add(new Point { X = 3, Y = 1 }, Orientation.NORTH);
                data.Add(new Point { X = 4, Y = 0 }, Orientation.EAST);
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(validPlace))]
        public void Place_ValidOrigin_ShouldReturnValidCoordinates(Point origin, Orientation face)
        {
            var currentCoordinates = executeCommand.Place(origin, face);

            Assert.Equal<Point>(origin, currentCoordinates);
        }

        public static TheoryData<Point, Orientation> inValidPlace
        {
            get
            {
                var data = new TheoryData<Point, Orientation>();
                data.Add(new Point { X = 10, Y = 0 }, Orientation.EAST);
                data.Add(new Point { X = 1, Y = 7 }, Orientation.WEST);
                data.Add(new Point { X = 20, Y = 2 }, Orientation.SOUTH);
                data.Add(new Point { X = 30, Y = 10 }, Orientation.NORTH);
                data.Add(new Point { X = 4, Y = 10 }, Orientation.EAST);
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(inValidPlace))]
        public void Place_InvalidOrigin_ShouldReturnInValidCoordinates(Point origin, Orientation face)
        {
            var currentCoordinates = executeCommand.Place(origin, face);

            Assert.Equal<Point>(new Point { X = -1, Y = -1 }, currentCoordinates);
        }

        public static TheoryData<Point, Orientation, int, (Point, Orientation)> validMove
        {
            get
            {
                var data = new TheoryData<Point, Orientation, int, (Point, Orientation)>();
                data.Add(new Point { X = 0, Y = 0 }, Orientation.EAST, 4, (new Point { X = 4, Y = 0 }, Orientation.EAST));
                data.Add(new Point { X = 1, Y = 3 }, Orientation.WEST, 1, (new Point { X = 0, Y = 3 }, Orientation.WEST));
                data.Add(new Point { X = 2, Y = 2 }, Orientation.SOUTH, 2, (new Point { X = 2, Y = 0 }, Orientation.SOUTH));
                data.Add(new Point { X = 3, Y = 1 }, Orientation.NORTH, 3, (new Point { X = 3, Y = 4 }, Orientation.NORTH));
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(validMove))]
        public void Move_Valid_ShouldMove_NumberOfUnits_InTheDirectionOfOrientation(Point origin, Orientation face, int numberOfMoves, (Point, Orientation) expectedOutcome)
        {
            var currentCoordinates = executeCommand.Place(origin, face);

            for (int i = 0; i < numberOfMoves; i++)
            {
                executeCommand.Move();
            }
            var actual = executeCommand.Report();

            Assert.Equal<(Point, Orientation)>(expectedOutcome, actual);
        }

        public static TheoryData<Point, Orientation, int, (Point, Orientation)> invalidMove
        {
            get
            {
                var data = new TheoryData<Point, Orientation, int, (Point, Orientation)>();
                data.Add(new Point { X = 0, Y = 0 }, Orientation.EAST, 14, (new Point { X = 4, Y = 0 }, Orientation.EAST));
                data.Add(new Point { X = 1, Y = 3 }, Orientation.WEST, 11, (new Point { X = 0, Y = 3 }, Orientation.WEST));
                data.Add(new Point { X = 2, Y = 2 }, Orientation.SOUTH, 12, (new Point { X = 2, Y = 0 }, Orientation.SOUTH));
                data.Add(new Point { X = 3, Y = 1 }, Orientation.NORTH, 13, (new Point { X = 3, Y = 4 }, Orientation.NORTH));
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(invalidMove))]
        public void Move_Invalid_ShouldNotMoveAtAll(Point origin, Orientation face, int numberOfMoves, (Point, Orientation) expectedOutcome)
        {
            var currentCoordinates = executeCommand.Place(origin, face);

            for (int i = 0; i < numberOfMoves; i++)
            {
                executeCommand.Move();
            }
            var actual = executeCommand.Report();

            Assert.Equal<(Point, Orientation)>(expectedOutcome, actual);
        }

        public static TheoryData<Point, Orientation, Orientation> rightTurn
        {
            get
            {
                var data = new TheoryData<Point, Orientation, Orientation>();
                data.Add(new Point { X = 0, Y = 0 }, Orientation.EAST, Orientation.SOUTH);
                data.Add(new Point { X = 1, Y = 3 }, Orientation.WEST, Orientation.NORTH);
                data.Add(new Point { X = 2, Y = 2 }, Orientation.SOUTH, Orientation.WEST);
                data.Add(new Point { X = 3, Y = 1 }, Orientation.NORTH, Orientation.EAST);
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(rightTurn))]
        public void Right_ShouldTurn_90degree_AntiClockwise(Point origin, Orientation face, Orientation newOrientation)
        {
            var currentCoordinates = executeCommand.Place(origin, face);

            executeCommand.Right();

            Assert.Equal<(Point, Orientation)>((origin, newOrientation), executeCommand.Report());
        }

        public static TheoryData<Point, Orientation, Orientation> leftTurn
        {
            get
            {
                var data = new TheoryData<Point, Orientation, Orientation>();
                data.Add(new Point { X = 0, Y = 0 }, Orientation.EAST, Orientation.NORTH);
                data.Add(new Point { X = 1, Y = 3 }, Orientation.WEST, Orientation.SOUTH);
                data.Add(new Point { X = 2, Y = 2 }, Orientation.SOUTH, Orientation.EAST);
                data.Add(new Point { X = 3, Y = 1 }, Orientation.NORTH, Orientation.WEST);
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(leftTurn))]
        public void Left_ShouldTurn_90degree_Clockwise(Point origin, Orientation face, Orientation newOrientation)
        {
            var currentCoordinates = executeCommand.Place(origin, face);

            executeCommand.Left();

            Assert.Equal<(Point, Orientation)>((origin, newOrientation), executeCommand.Report());
        }
    }
}
