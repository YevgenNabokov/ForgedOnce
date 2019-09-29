using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class Stage
    {
        public string StageName;

        public ICodeGenerationPlugin Plugin;

        public string PluginId
        {
            get
            {
                return this.Plugin.Signature.Id;
            }
        }

        public List<ICodeStream> Execute(IEnumerable<CodeFile> inputs, IMetadataWriter metadataWriter, IMetadataReader metadataReader, ICodeStreamFactory codeStreamFactory, IPipelineExecutionInfo pipelineExecutionInfo)
        {
            var metadataRecorder = new MetadataRecorder(metadataWriter, pipelineExecutionInfo, this.Plugin.Signature.Id);
            var result = this.Plugin.InitializeOutputs(codeStreamFactory);

            this.Plugin.Execute(inputs, metadataRecorder, metadataReader);

            return result;
        }
    }
}
