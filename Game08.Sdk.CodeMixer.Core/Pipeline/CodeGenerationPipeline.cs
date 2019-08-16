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

        public IPipelineEnvironment WorkspaceEnvironment;

        public MetadataStore MetadataStore;

        public void Execute()
        {
            var inputs = this.InputCodeStreamProvider.RetrieveCodeStreams();

            this.ExecuteAllWaves(new[] { new Wave(this.EntryStages, inputs) });
        }

        private void ExecuteAllWaves(IEnumerable<Wave> waves)
        {
            List<Wave> currentWaves = new List<Wave>(waves);

            while (currentWaves.Count > 0)
            {
                var next = this.ExecuteWave(currentWaves[0]);
                currentWaves.RemoveAt(0);
                currentWaves.AddRange(next);
            }
        }

        private IEnumerable<Wave> ExecuteWave(Wave wave)
        {
            List<Wave> result = new List<Wave>();
            List<ICodeStream> waveOutputs = new List<ICodeStream>();
            List<CodeFile> storableOutputs = new List<CodeFile>();

            foreach (var stage in wave.Stages)
            {
                this.MetadataStore.CurrentStageName = stage.Stage.StageName;
                this.MetadataStore.CurrentPluginId = stage.Stage.PluginId;

                var outputs = stage.Stage.Execute(stage.InputSelector.Select(wave.Inputs), this.MetadataStore);
                waveOutputs.AddRange(outputs);
                storableOutputs.AddRange(stage.FinalOutputSelector.Select(outputs));
                var next = new Wave(stage.NextStages, outputs);
                result.Add(next);
            }

            this.WorkspaceEnvironment.CodeStreamsDiscarded(wave.Inputs);
            this.WorkspaceEnvironment.CodeStreamsEmitted(waveOutputs);
            this.WorkspaceEnvironment.StoreForOutput(storableOutputs);
            this.WorkspaceEnvironment.RefreshAndRecompile();
            this.MetadataStore.ResolveNames();

            return result;
        }
    }
}
