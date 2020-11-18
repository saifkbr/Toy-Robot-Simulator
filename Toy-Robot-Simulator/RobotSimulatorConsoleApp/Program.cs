namespace RobotSimulatorConsoleApp
{
    using Microsoft.Extensions.DependencyInjection;
    using RobotSimulatorService;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using static System.Console;

    class Program
    {
        private static IServiceProvider _serviceProvider;

        static async Task Main(string[] args)
        {
            ConfigureDI();

        START:
            Clear();
            WriteLine("*************** Toy Robot Simulator ***************\n\n");
            WriteLine("Please enter Command file full path:");

            var filePath = ReadLine();

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                var commandFile = _serviceProvider.GetService<ICommandFileParser>();
                var fileText = await ProcessReadAsync(filePath);
                var commandsList = commandFile.GetCommandsList(fileText);

                var service = _serviceProvider.GetService<ISimulatorService>();
                foreach (var commandText in commandsList)
                {
                    var report = service.Simulate(commandText);
                    WriteLine("\n" + report);
                }
            }
            WriteLine("\nDo you want to execute another command file? (Y/N)");
            if (ReadLine().Equals("Y", StringComparison.OrdinalIgnoreCase))
                goto START;
            else
                Environment.Exit(0);
        }

        private static async Task<string> ProcessReadAsync(string filePath)
        {
            string fileText = string.Empty;

            try
            {
                if (File.Exists(filePath))
                {
                    fileText = await ReadTextAsync(filePath);
                }
                else
                {
                    WriteLine($"{filePath} NOT FOUND.");
                }
            }
            catch (Exception ex)
            {
                WriteLine($"{ex.Message}");
            }

            return fileText;
        }

        private static async Task<string> ReadTextAsync(string filePath)
        {
            using var sourceStream =
                new FileStream(
                    filePath,
                    FileMode.Open, FileAccess.Read, FileShare.Read,
                    bufferSize: 4096, useAsync: true);

            var sb = new StringBuilder();

            byte[] buffer = new byte[0x1000];
            int numRead;
            while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string text = Encoding.Default.GetString(buffer, 0, numRead);
                sb.Append(text.Replace("\r\n", ""));
            }

            return sb.ToString();
        }

        private static void ConfigureDI()
        {
            _serviceProvider = new ServiceCollection()
           .AddSingleton<ICommandFileParser, CommandFileParser>()
           .AddSingleton<ITableTop, TableTop>()
           .AddSingleton<IExecuteCommand, ExecuteCommand>()
           .AddSingleton<ISimulatorService, SimulatorService>()
           .BuildServiceProvider();
        }
    }
}
