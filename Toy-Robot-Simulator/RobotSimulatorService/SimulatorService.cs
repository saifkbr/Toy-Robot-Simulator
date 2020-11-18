namespace RobotSimulatorService
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public interface ISimulatorService
    {
        string Simulate(string commands);
    }

    public class SimulatorService : ISimulatorService
    {
        private readonly IExecuteCommand executeCommand;

        public SimulatorService(IExecuteCommand executeCommand)
        {
            this.executeCommand = executeCommand;
        }

        public string Simulate(string commandString)
        {
            var reportString = string.Empty;

            if (string.IsNullOrWhiteSpace(commandString) ||
                !commandString.Contains(Command.PLACE.ToString()) ||
                !commandString.Contains(Command.REPORT.ToString()))
            {
                return reportString;
            }

            var commandQueue = ValidCommandSequence(commandString);
            var origin = new Point { X = -1, Y = -1 };
            Orientation orientation = default;

            do
            {
                if (Enum.TryParse(commandQueue.Dequeue(), true, out Command command))
                {
                    // If this is the very first valid command "PLACE", extract coordinates and face
                    if (command.Equals(Command.PLACE))
                    {
                        PlaceCommand(commandQueue, command, ref origin, ref orientation);
                    }

                    if (origin.X != -1 && origin.Y != -1)
                    {
                        switch (command)
                        {
                            case Command.PLACE:
                                origin = executeCommand.Place(origin, orientation);
                                break;
                            case Command.MOVE:
                                executeCommand.Move();
                                break;
                            case Command.LEFT:
                                executeCommand.Left();
                                break;
                            case Command.RIGHT:
                                executeCommand.Right();
                                break;
                            case Command.REPORT:
                                var report = executeCommand.Report();
                                reportString = $"{report.coordinate.X},{report.coordinate.Y},{report.orientation}";
                                break;
                        }
                    }
                }

            } while (commandQueue.Count > 0);

            return reportString;
        }

        private void PlaceCommand(Queue<string> commandQueue, Command command, ref Point origin, ref Orientation orientation)
        {
            if (!int.TryParse(commandQueue.Dequeue(), out int x))
            {
                x = -1;
            }

            if (!int.TryParse(commandQueue.Dequeue(), out int y))
            {
                y = -1;
            }

            Enum.TryParse(commandQueue.Dequeue(), true, out Orientation face);

            origin = new Point { X = x, Y = y };
            orientation = face;
        }

        private Queue<string> ValidCommandSequence(string commands)
        {
            var commandQueue = new Queue<string>(commands.ToUpper().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray());
            commandQueue.Enqueue(Command.REPORT.ToString());

            while (!commandQueue.Peek().Equals(Command.PLACE.ToString()))
            {
                commandQueue.Dequeue();
            }

            return commandQueue;
        }
    }
}
