using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface IPipelineExecutionInfo
    {
        int CurrentBatchIndex { get; }

        string CurrentStageName { get; }
    }
}
