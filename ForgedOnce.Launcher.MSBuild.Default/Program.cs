using ForgedOnce.Core.Logging;
using ForgedOnce.Environment;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO.Abstractions;

namespace ForgedOnce.Launcher.MSBuild.Default
{
    class Program
    {
        static void Main(string[] args)
        {
            RootCommand rootCommand = new RootCommand(description: "Runs code generation.")
            {
                new Option<string>(
                    aliases: new [] { "--solution", "-s" },
                    description: "The path to the target solution."),
                new Option<string>(
                    aliases: new [] { "--config", "-c" },
                    description: "The path to the code generation pipeline config."),
                new Option<bool>(
                    aliases: new [] { "--log", "-l" },
                    description: "Allows to record log when no error occurred.",
                    getDefaultValue: () => false)
            };

            rootCommand.Handler = CommandHandler.Create<string, string, bool>(Execute);
            rootCommand.Invoke(args);
        }

        static void Execute(string solution, string config, bool log = false)
        {
            var fileSystem = new FileSystem();
            var collectionLogger = new CollectionLogger();

            try
            {
                var launcher = new Launcher(fileSystem, collectionLogger);
                launcher.Launch(solution, config);

                if (log)
                {
                    var textLogger = new TextLogger(fileSystem);
                    textLogger.WriteMany(collectionLogger.Records);
                    Console.WriteLine($"Log file: {textLogger.InitializedFilePath}");
                }
            }
            catch (Exception ex)
            {
                if (collectionLogger.Records.Count == 0 || collectionLogger.Records[collectionLogger.Records.Count - 1].Severity != MessageSeverity.Error)
                {
                    collectionLogger.Write(new ErrorLogRecord("Error occurred.", ex));
                }

                var textLogger = new TextLogger(fileSystem);
                textLogger.WriteMany(collectionLogger.Records);
                Console.WriteLine($"Error occurred: {ex.Message}, more details in log file: {textLogger.InitializedFilePath}");
            }
        }
    }
}
