﻿using Game08.Sdk.CodeMixer.Core.Interfaces;
using System.Collections.Generic;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class CodeGenerationPipeline : ICodeGenerationPipeline
    {
        public List<Batch> Batches = new List<Batch>();

        public ICodeStreamProvider InputCodeStreamProvider;

        public IPipelineEnvironment WorkspaceEnvironment;

        public MetadataStore MetadataStore;

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
            List<ICodeStream> result = new List<ICodeStream>();            
            List<CodeFile> storableOutputs = new List<CodeFile>();

            foreach (var stage in batch.Stages)
            {
                this.MetadataStore.CurrentStageName = stage.Stage.StageName;
                this.MetadataStore.CurrentPluginId = stage.Stage.PluginId;

                var outputs = stage.Stage.Execute(stage.InputSelector.Select(inputs), this.MetadataStore, this.MetadataStore);
                result.AddRange(outputs);
                storableOutputs.AddRange(stage.FinalOutputSelector.Select(outputs));                
            }

            this.WorkspaceEnvironment.CodeStreamsDiscarded(inputs);
            this.WorkspaceEnvironment.CodeStreamsEmitted(result);
            this.WorkspaceEnvironment.StoreForOutput(storableOutputs);
            this.WorkspaceEnvironment.RefreshAndRecompile();
            this.MetadataStore.ResolveNames();

            return result;
        }
    }
}
