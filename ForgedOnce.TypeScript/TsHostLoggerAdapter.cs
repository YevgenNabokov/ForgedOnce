using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Logging;
using ForgedOnce.TsLanguageServices.Host.Interfaces;
using ForgedOnce.TsLanguageServices.Host.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.TypeScript
{
    public class TsHostLoggerAdapter : ITsHostLogger
    {
        private readonly ILogger logger;
        private readonly LogMessageSeverity? logLevel;

        public TsHostLoggerAdapter(ILogger logger, LogMessageSeverity? logLevel = null)
        {
            this.logger = logger;
            this.logLevel = logLevel;
        }

        public void WriteLog(LogMessageSeverity severity, string message)
        {
            if (!this.logLevel.HasValue || (int)severity >= (int)this.logLevel.Value)
            {
                MessageSeverity mappedSeverity = MessageSeverity.Information;

                switch (severity)
                {
                    case LogMessageSeverity.Debug: { mappedSeverity = MessageSeverity.Debug; break; }
                    case LogMessageSeverity.Information: { mappedSeverity = MessageSeverity.Information; break; }
                    case LogMessageSeverity.Warning: { mappedSeverity = MessageSeverity.Warning; break; }
                    case LogMessageSeverity.Error: { mappedSeverity = MessageSeverity.Error; break; }
                }

                this.Write(mappedSeverity, message);
            }
        }

        private void Write(MessageSeverity severity, string message)
        {
            this.logger.Write(
                new LogRecord(
                    severity,
$@">> TypeScript Host Log Output:
{message}
"
                    ));
        }
    }
}
