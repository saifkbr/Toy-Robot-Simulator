namespace RobotSimulatorTest
{
    using RobotSimulatorService;
    using Xunit;

    public class TableTopTest
    {
        [Fact]
        public void DrawTableTop_ShouldReturn_5by5_Default_TableTop()
        {
            ITableTop tableTop = new TableTop();

            var actualTableTop = tableTop.DrawTableTop();

            Assert.Equal(5, actualTableTop.GetLength(0));
            Assert.Equal(5, actualTableTop.GetLength(1));
        }
    }
}
