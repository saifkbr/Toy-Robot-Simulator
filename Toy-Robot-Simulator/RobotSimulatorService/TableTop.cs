namespace RobotSimulatorService
{
    using System.Drawing;

    public interface ITableTop
    {
        Point[,] DrawTableTop();
    }

    public class TableTop : ITableTop
    {
        public Point[,] DrawTableTop()
        {
            int row = 5, column = 5;
            var points = new Point[row, column];
            int maxRow = row - 1, maxCol = column - 1;

            for (int i = maxRow; i >= 0; i--)
            {
                for (int j = maxCol; j >= 0; j--)
                {
                    points[i, j] = new Point { X = maxCol - j, Y = maxRow - i };
                }
            }

            return points;
        }
    }
}
