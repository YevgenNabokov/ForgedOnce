using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Pipeline;
using Game08.Sdk.CodeMixer.Environment.Configuration;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Builders
{
    public class PipelineBuilder : IBuilder<ICodeGenerationPipeline>
    {
        private IBuilderProvider builderProvider;

        public string Name => "GenericPipelineBuilder";

        public PipelineBuilder(IBuilderProvider builderProvider)
        {
            this.builderProvider = builderProvider;
        }

        public ICodeGenerationPipeline Build(JObject configuration)
        {
            var reader = new PipelineConfiguration(configuration);
            var result = new CodeGenerationPipeline();



            return result;
        }
    }
}
