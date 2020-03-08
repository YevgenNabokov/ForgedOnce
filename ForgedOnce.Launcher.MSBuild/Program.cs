using ForgedOnce.Core.Logging;
using ForgedOnce.Environment;
using System;
using System.IO.Abstractions;

namespace ForgedOnce.Launcher.MSBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            var collectionLogger = new CollectionLogger();
            var fileSystem = new FileSystem();

            try
            {
                CodeGenerationPipelineLauncherMsBuild launcher = new CodeGenerationPipelineLauncherMsBuild(fileSystem, collectionLogger);
                launcher.Execute(args[0], args[1]);
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
