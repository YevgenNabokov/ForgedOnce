using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment
{
    public class CollectionLogger : ILogger
    {
        public void Write(LogRecord logRecord)
        {
            this.Records.Add(logRecord);
        }

        public void WriteMany(IEnumerable<LogRecord> logRecords)
        {
            this.Records.AddRange(logRecords);
        }

        public List<LogRecord> Records { get; private set; } = new List<LogRecord>();
    }
}
