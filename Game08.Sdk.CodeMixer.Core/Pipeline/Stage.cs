using Game08.Sdk.CodeMixer.Core.Interfaces;
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

        public List<ICodeStream> Execute(IEnumerable<CodeFile> inputs, IMetadataWriter metadataWriter, IMetadataReader metadataReader, ICodeStreamFactory codeStreamFactory)
        {            
            var result = this.Plugin.InitializeOutputs(codeStreamFactory);

            this.Plugin.Execute(inputs, metadataWriter, metadataReader);

            return result;
        }
    }
}
