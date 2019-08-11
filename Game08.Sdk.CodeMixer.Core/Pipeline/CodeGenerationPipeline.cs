using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class CodeGenerationPipeline
    {
        public List<StageContainer> EntryStages = new List<StageContainer>();

        public ICodeStreamProvider InputCodeStreamProvider;

        public void Execute()
        {
            var input = this.InputCodeStreamProvider.RetrieveCodeStreams();            

            foreach (var stage in this.EntryStages)
            {
                this.ExecuteStage(stage, input);
            }
        }

        private void ExecuteStage(StageContainer stage, IEnumerable<ICodeStream> input)
        {
            List<CodeFile> files = new List<CodeFile>();
            foreach (var stream in stage.CodeStreamFilter.Filter(input))
            {
                files.AddRange(stream.Files);
            }

            var output = stage.Stage.Execute(files);

            foreach (var next in stage.NextStages)
            {
                this.ExecuteStage(next, output);
            }
        }
    }
}
