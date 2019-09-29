using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface ICodeGenerationPipeline
    {
        IPipelineExecutionInfo PipelineExecutionInfo { get; }

        void Execute();

        IEnumerable<CodeFile> GetOutputs();
    }
}
