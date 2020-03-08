using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Logging
{
    public class ErrorLogRecord : LogRecord
    {
        public ErrorLogRecord(string message, Exception exception = null)
            :base(MessageSeverity.Error, message)
        {
            this.Exception = exception;
        }

        public Exception Exception { get; private set; }

        public override string ToString()
        {
            var exceptionText = this.FormatException(this.Exception);
            if (!String.IsNullOrEmpty(exceptionText))
            {
                exceptionText = $"{Environment.NewLine}{exceptionText}";
            }
            return $"{base.ToString()}{exceptionText}" ;
        }

        protected string FormatException(Exception exception, string lineLead = "")
        {
            if (exception != null)
            {
                StringBuilder result = new StringBuilder();
                result.Append($"{exception.ToString()}");

                if (exception.InnerException != null)
                {
                    result.Append($"{Environment.NewLine}InnerException:{Environment.NewLine}");
                    result.Append(this.FormatException(exception.InnerException, $"{lineLead}\t"));
                }

                result.Insert(0, lineLead);
                return result.Replace(Environment.NewLine, $"{lineLead}{Environment.NewLine}").ToString();
            }

            return String.Empty;
        }
    }
}
