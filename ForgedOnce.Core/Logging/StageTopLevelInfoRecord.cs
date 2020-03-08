using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Logging
{
    public class StageTopLevelInfoRecord : LogRecord
    {
        public StageTopLevelInfoRecord(string message)
            : base(MessageSeverity.Information, message)
        {
        }

        public override string ToString()
        {
            return
$@"-------------------------------------------------------------------------------------------------------------
    {this.Message}
-------------------------------------------------------------------------------------------------------------";
        }
    }
}
