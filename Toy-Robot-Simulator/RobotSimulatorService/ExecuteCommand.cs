namespace RobotSimulatorService
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public interface IExecuteCommand
    {
        Point Place(Point originPlace, Orientation orientation);
        void Move();
        void Right();
        void Left();
        (Point coordinate, Orientation orientation) Report();
    }

    public class ExecuteCommand : IExecuteCommand
    {
        private Orientation currentOrientation;
        private Point currentCoordinate = new Point { X = -1, Y = -1 };
        private readonly Point axisMaxLimit;
        private readonly List<Point> tableTopCoordinates;

        public ExecuteCommand(ITableTop tableTop)
        {
            tableTopCoordinates = tableTop.DrawTableTop().Cast<Point>().ToList();
            axisMaxLimit = tableTop.DrawTableTop()[0, 0];
        }

        public Point Place(Point originPlace, Orientation orientation)
        {
            if (tableTopCoordinates.Any(p => p == originPlace))
            {
                currentCoordinate = originPlace;
                currentOrientation = orientation;
            }
            return currentCoordinate;
        }

        public void Move()
        {
            if (tableTopCoordinates.Any(p => p.X == currentCoordinate.X && p.Y == currentCoordinate.Y))
            {
                var coordinate = tableTopCoordinates.Single(p => p.X == currentCoordinate.X && p.Y == currentCoordinate.Y);

                // one unit forward in the direction currently facing
                switch (currentOrientation)
                {
                    case Orientation.NORTH:
                        // Y increase
                        if (coordinate.Y + 1 <= axisMaxLimit.Y)
                        {
                            currentCoordinate = new Point { X = coordinate.X, Y = coordinate.Y + 1 };
                        }
                        break;
                    case Orientation.SOUTH:
                        // Y decrease
                        if (coordinate.Y - 1 >= 0)
                        {
                            currentCoordinate = new Point { X = coordinate.X, Y = coordinate.Y - 1 };

                        }
                        break;
                    case Orientation.EAST:
                        // X increase
                        if (coordinate.X + 1 <= axisMaxLimit.X)
                        {
                            currentCoordinate = new Point { X = coordinate.X + 1, Y = coordinate.Y };
                        }
                        break;
                    case Orientation.WEST:
                        // X decrease
                        if (coordinate.X - 1 >= 0)
                        {
                            currentCoordinate = new Point { X = coordinate.X - 1, Y = coordinate.Y };
                        }
                        break;
                }
            }
        }

        public void Right()
        {
            if (tableTopCoordinates.Any(p => p.X == currentCoordinate.X && p.Y == currentCoordinate.Y))
            {
                switch (currentOrientation)
                {
                    case Orientation.NORTH:
                        // X increase
                        currentOrientation = Orientation.EAST;
                        break;
                    case Orientation.SOUTH:
                        // X decrease
                        currentOrientation = Orientation.WEST;
                        break;
                    case Orientation.EAST:
                        currentOrientation = Orientation.SOUTH;
                        // Y decrease
                        break;
                    case Orientation.WEST:
                        // Y increase
                        currentOrientation = Orientation.NORTH;
                        break;
                }
            }
        }

        public void Left()
        {
            if (tableTopCoordinates.Any(p => p.X == currentCoordinate.X && p.Y == currentCoordinate.Y))
            {
                switch (currentOrientation)
                {
                    case Orientation.NORTH:
                        // X decrease
                        currentOrientation = Orientation.WEST;
                        break;
                    case Orientation.SOUTH:
                        // X increase
                        currentOrientation = Orientation.EAST;
                        break;
                    case Orientation.EAST:
                        currentOrientation = Orientation.NORTH;
                        // Y increase
                        break;
                    case Orientation.WEST:
                        // Y decrease
                        currentOrientation = Orientation.SOUTH;
                        break;
                }
            }
        }

        public (Point coordinate, Orientation orientation) Report() => (currentCoordinate, currentOrientation);
    }
}
