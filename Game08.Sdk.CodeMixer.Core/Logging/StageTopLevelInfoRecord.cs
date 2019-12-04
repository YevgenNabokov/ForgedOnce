using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Logging
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
