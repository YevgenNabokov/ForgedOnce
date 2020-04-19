using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Logging;
using ForgedOnce.Core.Metadata.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForgedOnce.Core.Pipeline
{
    public class CodeGenerationPipeline : ICodeGenerationPipeline
    {
        protected PipelineExecutionInfo pipelineExecutionInfo;

        public List<Batch> Batches = new List<Batch>();

        public ICodeStreamProvider InputCodeStreamProvider;

        public IPipelineEnvironment PipelineEnvironment;

        public MetadataStore MetadataStore;

        private readonly ILogger logger;

        public CodeGenerationPipeline(ILogger logger)
        {
            this.MetadataStore = new MetadataStore();
            this.pipelineExecutionInfo = new PipelineExecutionInfo();
            this.logger = logger;
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
            this.logger.Write(new StageTopLevelInfoRecord("Starting pipeline execution."));

            try
            {
                var inputs = this.InputCodeStreamProvider.RetrieveCodeStreams();

                this.Prepare(this.Batches);

                this.ExecuteAllBatches(this.Batches, inputs);
            }
            catch (Exception ex)
            {
                this.logger.Write(new ErrorLogRecord("Error occurred during pipeline execution.", ex));
                throw;
            }
        }

        private void Prepare(IEnumerable<Batch> batches)
        {
            foreach (var batch in batches)
            {
                foreach (var stage in batch.Stages)
                {
                    if (stage.CleanDestinations)
                    {
                        foreach (var destination in stage.CodeFileDestinations)
                        {
                            destination.Value.Clear();
                        }
                    }
                }
            }
        }

        private void ExecuteAllBatches(IEnumerable<Batch> batches, IEnumerable<ICodeStream> inputs)
        {
            var batchInput = inputs;

            foreach (var batch in batches)
            {
                this.MakeCodeFilesReadOnly(batchInput);
                batchInput = this.ExecuteBatch(batch, batchInput);
            }
        }

        private IEnumerable<ICodeStream> ExecuteBatch(Batch batch, IEnumerable<ICodeStream> inputs)
        {
            this.logger.Write(new StageSubLevelInfoRecord($"Starting batch {batch.Name}."));

            this.ApplyShadowing(batch);

            this.pipelineExecutionInfo.CurrentBatchIndex = batch.Index;
            this.PipelineEnvironment.RefreshAndRecompile();

            List<ICodeStream> result = new List<ICodeStream>();            
            List<CodeFile> storableOutputs = new List<CodeFile>();

            foreach (var stage in batch.Stages)
            {
                this.pipelineExecutionInfo.CurrentStageName = stage.Stage.StageName;

                var codeStreamFactory = new CodeStreamFactory(this.PipelineEnvironment, stage.CodeFileDestinations);

                var inputCodeFiles = stage.InputSelector.Select(inputs);
                inputCodeFiles = inputCodeFiles.Where(c => !this.PipelineEnvironment.GetShadowFilter(c.Language).IsMatch(c.Location));
                var outputs = stage.Stage.Execute(inputCodeFiles, this.MetadataStore, this.MetadataStore, codeStreamFactory, this.pipelineExecutionInfo);
                if (stage.FinalOutputSelector != null)
                {
                    storableOutputs.AddRange(stage.FinalOutputSelector.Select(outputs));
                }

                foreach (var o in outputs)
                {
                    var outputName = stage.OutputCodeStreamRenames.ContainsKey(o.Name) ? stage.OutputCodeStreamRenames[o.Name] : o.Name;
                    result.Add(new CodeStream(o.Language, outputName, o.Files));
                }
            }

            List<ICodeStream> persistentInputs = new List<ICodeStream>();
            List<ICodeStream> inputsToDiscard = new List<ICodeStream>();
            foreach (var i in inputs)
            {
                if (batch.PersistInputCodeStreams != null && batch.PersistInputCodeStreams.Contains(i.Name))
                {
                    persistentInputs.Add(i);
                }
                else
                {
                    inputsToDiscard.Add(i);
                }
            }

            this.PipelineEnvironment.CodeStreamsDiscarded(inputsToDiscard);
            this.PipelineEnvironment.CodeStreamsCompleted(result);
            this.PipelineEnvironment.StoreForOutput(storableOutputs);
            this.CommitMetadata();

            result.AddRange(persistentInputs);

            return result;
        }

        private void ApplyShadowing(Batch batch)
        {
            if (batch.Shadow != null)
            {
                foreach (var s in batch.Shadow)
                {
                    this.PipelineEnvironment.GetShadowFilter(s.Language).Shadow(s.Filter);
                }
            }

            if (batch.Unshadow != null)
            {
                foreach (var s in batch.Unshadow)
                {
                    this.PipelineEnvironment.GetShadowFilter(s.Language).Unshadow(s.Filter);
                }
            }
        }

        private void CommitMetadata()
        {
            this.MetadataStore.Commit();
        }

        private void MakeCodeFilesReadOnly(IEnumerable<ICodeStream> codeStreams)
        {
            foreach (var stream in codeStreams)
            {
                foreach (var file in stream.Files)
                {
                    file.MakeReadOnly();
                }
            }
        }
    }
}
