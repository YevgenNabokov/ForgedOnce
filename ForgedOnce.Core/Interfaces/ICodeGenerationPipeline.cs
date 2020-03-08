using ForgedOnce.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ICodeGenerationPipeline
    {
        IPipelineExecutionInfo PipelineExecutionInfo { get; }

        void Execute();

        IEnumerable<CodeFile> GetOutputs();
    }
}
