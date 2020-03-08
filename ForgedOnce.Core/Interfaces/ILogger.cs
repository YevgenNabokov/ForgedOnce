using ForgedOnce.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ILogger
    {
        void Write(LogRecord logRecord);

        void WriteMany(IEnumerable<LogRecord> logRecords);
    }
}
