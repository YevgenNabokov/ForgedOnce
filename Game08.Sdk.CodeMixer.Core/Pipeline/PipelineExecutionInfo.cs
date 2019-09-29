using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class PipelineExecutionInfo : IPipelineExecutionInfo
    {
        public int CurrentBatchIndex { get; set; }

        public string CurrentStageName { get; set; }
    }
}
