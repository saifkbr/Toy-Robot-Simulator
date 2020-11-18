namespace RobotSimulatorTest
{
    using RobotSimulatorService;
    using System.Reflection;
    using Xunit;

    public class CommandFileParserTest
    {
        [Theory]
        [InlineData(@"Resources.Commands1.txt", 3)]
        [InlineData(@"Resources.Commands2.txt", 3)]
        [InlineData(@"Resources.Commands3.txt", 0)]
        [InlineData(@"Resources.Commands4.txt", 0)]
        public async System.Threading.Tasks.Task ProcessReadAsync_ShouldReturn_ListOfCommandsAsync(string filename, int setOfIndividualCommands)
        {
            ICommandFileParser parser = new CommandFileParser();

            var setOfCompleteCommands = await Assembly.GetExecutingAssembly().GetEmbeddedResourceStreamAsync(filename);

            var commandsList = parser.GetCommandsList(setOfCompleteCommands);

            Assert.Equal(setOfIndividualCommands, commandsList.Count);
        }
    }
}