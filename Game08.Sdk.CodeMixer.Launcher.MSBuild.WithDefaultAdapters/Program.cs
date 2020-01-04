using Game08.Sdk.CodeMixer.Core.Logging;
using Game08.Sdk.CodeMixer.CSharp.MsBuild;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Glsl.MsBuild;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Interfaces;
using Game08.Sdk.CodeMixer.LimitedTypeScript.MsBuild;
using System;
using System.IO.Abstractions;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild.WithDefaultAdapters
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileSystem = new FileSystem();
            var collectionLogger = new CollectionLogger();

            try
            {
                var launcher = new Launcher(fileSystem, collectionLogger);
                launcher.Launch(args[0], args[1]);
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
