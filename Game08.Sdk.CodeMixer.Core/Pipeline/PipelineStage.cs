using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class PipelineStage
    {
        public string PluginId;

        public string StageName;

        public ICodeGenerationPlugin Plugin;        

        public List<ICodeStream> Execute(IEnumerable<CodeFile> inputs)
        {            
            var result = this.Plugin.InitializeOutputs();

            this.Plugin.Execute(inputs);

            return result;
        }
    }
}
