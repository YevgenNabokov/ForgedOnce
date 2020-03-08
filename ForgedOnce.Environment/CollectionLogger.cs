using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment
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
