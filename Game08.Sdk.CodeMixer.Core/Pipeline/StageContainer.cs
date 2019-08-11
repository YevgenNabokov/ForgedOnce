using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class StageContainer
    {
        public ICodeStreamFilter CodeStreamFilter;

        public PipelineStage Stage;

        public List<StageContainer> NextStages;
    }
}
