namespace RobotSimulatorService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICommandFileParser
    {
        List<string> GetCommandsList(string setOfCompleteCommands);
    }

    public class CommandFileParser : ICommandFileParser
    {
        public List<string> GetCommandsList(string setOfCompleteCommands)
        {
            List<string> commands = new List<string>();

            if (!string.IsNullOrWhiteSpace(setOfCompleteCommands))
            {
                commands = setOfCompleteCommands.Split(Command.REPORT.ToString(), StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            return commands;
        }
    }
}
