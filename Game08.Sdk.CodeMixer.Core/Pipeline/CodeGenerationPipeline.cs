using Game08.Sdk.CodeMixer.Core.Interfaces;
using System.Collections.Generic;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class CodeGenerationPipeline : ICodeGenerationPipeline
    {
        public List<Batch> Batches = new List<Batch>();

        public ICodeStreamProvider InputCodeStreamProvider;

        public IPipelineEnvironment PipelineEnvironment;

        public MetadataStore MetadataStore;

        public CodeGenerationPipeline()
        {
            this.MetadataStore = new MetadataStore();
        }

        public IEnumerable<CodeFile> GetOutputs()
        {
            return this.PipelineEnvironment.GetOutputs();
        }

        public void Execute()
        {
            var inputs = this.InputCodeStreamProvider.RetrieveCodeStreams();
            this.PipelineEnvironment.RefreshAndRecompile();
            this.MetadataStore.ResolveNames();

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
            List<ICodeStream> result = new List<ICodeStream>();            
            List<CodeFile> storableOutputs = new List<CodeFile>();

            foreach (var stage in batch.Stages)
            {
                this.MetadataStore.CurrentStageName = stage.Stage.StageName;
                this.MetadataStore.CurrentPluginId = stage.Stage.PluginId;

                var codeStreamFactory = new CodeStreamFactory(this.PipelineEnvironment, stage.CodeFileLocationProviders);
                var outputs = stage.Stage.Execute(stage.InputSelector.Select(inputs), this.MetadataStore, this.MetadataStore, codeStreamFactory);
                result.AddRange(outputs);
                storableOutputs.AddRange(stage.FinalOutputSelector.Select(outputs));                
            }

            this.PipelineEnvironment.CodeStreamsDiscarded(inputs);
            this.PipelineEnvironment.CodeStreamsCompleted(result);
            this.PipelineEnvironment.StoreForOutput(storableOutputs);
            this.PipelineEnvironment.RefreshAndRecompile();
            this.MetadataStore.ResolveNames();

            return result;
        }
    }
}
