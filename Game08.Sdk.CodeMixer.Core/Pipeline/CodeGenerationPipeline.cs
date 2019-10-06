using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Storage;
using System.Collections.Generic;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class CodeGenerationPipeline : ICodeGenerationPipeline
    {
        protected PipelineExecutionInfo pipelineExecutionInfo;

        public List<Batch> Batches = new List<Batch>();

        public ICodeStreamProvider InputCodeStreamProvider;

        public IPipelineEnvironment PipelineEnvironment;

        public MetadataStore MetadataStore;

        public CodeGenerationPipeline()
        {
            this.MetadataStore = new MetadataStore();
            this.pipelineExecutionInfo = new PipelineExecutionInfo();
        }

        public IPipelineExecutionInfo PipelineExecutionInfo
        {
            get
            {
                return this.pipelineExecutionInfo;
            }            
        }

        public IEnumerable<CodeFile> GetOutputs()
        {
            return this.PipelineEnvironment.GetOutputs();
        }

        public void Execute()
        {
            var inputs = this.InputCodeStreamProvider.RetrieveCodeStreams();

            this.ExecuteAllBatches(this.Batches, inputs);
        }

        private void ExecuteAllBatches(IEnumerable<Batch> batches, IEnumerable<ICodeStream> inputs)
        {
            var batchInput = inputs;

            foreach (var batch in batches)
            {
                batchInput = this.ExecuteBatch(batch, batchInput);
            }
        }

        private IEnumerable<ICodeStream> ExecuteBatch(Batch batch, IEnumerable<ICodeStream> inputs)
        {
            this.pipelineExecutionInfo.CurrentBatchIndex = batch.Index;
            this.PipelineEnvironment.RefreshAndRecompile();

            List<ICodeStream> result = new List<ICodeStream>();            
            List<CodeFile> storableOutputs = new List<CodeFile>();

            foreach (var stage in batch.Stages)
            {
                this.pipelineExecutionInfo.CurrentStageName = stage.Stage.StageName;

                var codeStreamFactory = new CodeStreamFactory(this.PipelineEnvironment, stage.CodeFileLocationProviders);
                var outputs = stage.Stage.Execute(stage.InputSelector.Select(inputs), this.MetadataStore, this.MetadataStore, codeStreamFactory, this.pipelineExecutionInfo);
                result.AddRange(outputs);
                if (stage.FinalOutputSelector != null)
                {
                    storableOutputs.AddRange(stage.FinalOutputSelector.Select(outputs));
                }
            }

            this.PipelineEnvironment.CodeStreamsDiscarded(inputs);
            this.PipelineEnvironment.CodeStreamsCompleted(result);
            this.PipelineEnvironment.StoreForOutput(storableOutputs);
            this.RefineMetadata(result);

            return result;
        }

        private void RefineMetadata(IEnumerable<ICodeStream> codeStreams)
        {
            foreach (var stream in codeStreams)
            {
                foreach (var codeFile in stream.Files)
                {
                    foreach (var symbol in codeFile.SemanticSymbols)
                    {
                        this.MetadataStore.Refine(symbol);
                    }
                }
            }

            this.MetadataStore.Refresh();
        }
    }
}
