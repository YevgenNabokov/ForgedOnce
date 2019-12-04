using Game08.Sdk.CodeMixer.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface ILogger
    {
        void Write(LogRecord logRecord);

        void WriteMany(IEnumerable<LogRecord> logRecords);
    }
}
