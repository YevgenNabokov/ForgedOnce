using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Logging
{
    public class LogRecord
    {
        public LogRecord(MessageSeverity severity, string message)
        {
            this.Severity = severity;
            this.Message = message;
        }

        public MessageSeverity Severity { get; private set; }

        public string Message { get; private set; }

        public override string ToString()
        {
            return $"{this.FormatSeverity(this.Severity)}: {this.Message}";
        }

        protected string FormatSeverity(MessageSeverity severity)
        {
            switch (severity)
            {
                case MessageSeverity.Information: return string.Empty;
                case MessageSeverity.Warning: return "WARN!";
                case MessageSeverity.Error: return "!!! ERROR !!!";
            }

            return severity.ToString().ToLower();
        }
    }
}
