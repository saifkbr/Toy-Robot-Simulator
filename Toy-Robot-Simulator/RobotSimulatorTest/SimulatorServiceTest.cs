namespace RobotSimulatorTest
{
    using Moq;
    using RobotSimulatorService;
    using System.Drawing;
    using Xunit;

    public class SimulatorServiceTest
    {
        private readonly Mock<IExecuteCommand> mockExecuteCommand;
        private readonly ISimulatorService simulatorService;

        public SimulatorServiceTest()
        {
            mockExecuteCommand = new Mock<IExecuteCommand>();
            simulatorService = new SimulatorService(mockExecuteCommand.Object);
        }

        [Theory]
        [InlineData("PLACE 0,0,NORTH,MOVE,REPORT", 0, 1, Orientation.NORTH, "0,1,NORTH")]
        [InlineData("PLACE 0,0,NORTH,LEFT,REPORT", 0, 0, Orientation.WEST, "0,0,WEST")]
        [InlineData("PLACE 1,2,EAST,MOVE,MOVE,LEFT,MOVE,REPORT", 3, 3, Orientation.NORTH, "3,3,NORTH")]
        public void Simulate_ValidCommand_ShouldReturnNewCoordinatesOrFace(string input, int x, int y, Orientation orientation, string expected)
        {
            mockExecuteCommand.Setup(x => x.Place(It.IsAny<Point>(), It.IsAny<Orientation>())).Returns(new Point { X = 0, Y = 0 });
            mockExecuteCommand.Setup(x => x.Report()).Returns((new Point { X = x, Y = y }, orientation));

            var reportText = simulatorService.Simulate(input);

            Assert.Equal(expected, reportText);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("0,0,NORTH,LEFT,REPORT", "")]
        [InlineData("JUST,KEEP,ON,GOING", "")]
        public void Simulate_InValidCommand_ShouldNotChangeOriginAndFace(string input, string expected)
        {
            mockExecuteCommand.Setup(x => x.Place(It.IsAny<Point>(), It.IsAny<Orientation>())).Returns(new Point { X = 0, Y = 0 });

            var reportText = simulatorService.Simulate(input);

            Assert.Equal(expected, reportText);
        }
    }
}
