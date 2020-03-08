using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Environment
{
    public class TextLogger : ILogger
    {
        private readonly IFileSystem fileSystem;
        private string logFilePath;

        public TextLogger(IFileSystem fileSystem, string logFilePath = null)
        {
            this.fileSystem = fileSystem;
            this.logFilePath = logFilePath;
        }

        public string InitializedFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.logFilePath))
                {
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
                    this.logFilePath = this.fileSystem.Path.Combine(this.fileSystem.Path.GetTempPath(), $"CG_{timestamp}_{Guid.NewGuid()}.log");
                }

                return this.logFilePath;
            }
        }

        public void Write(LogRecord logRecord)
        {
            try
            {
                this.fileSystem.File.AppendAllText(this.InitializedFilePath, $"{System.Environment.NewLine}{logRecord.ToString()}");
            }
            catch
            {
            }
        }

        public void WriteMany(IEnumerable<LogRecord> logRecords)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                foreach (var r in logRecords)
                {
                    builder.Append(System.Environment.NewLine);
                    builder.Append(r.ToString());
                }

                this.fileSystem.File.AppendAllText(this.InitializedFilePath, builder.ToString());
            }
            catch
            {
            }
        }        
    }
}
