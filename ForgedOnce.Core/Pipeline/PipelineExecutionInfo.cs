using ForgedOnce.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Pipeline
{
    public class PipelineExecutionInfo : IPipelineExecutionInfo
    {
        public int CurrentBatchIndex { get; set; }

        public string CurrentStageName { get; set; }
    }
}
