using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class CodeStreamFactory : ICodeStreamFactory
    {
        private readonly IPipelineEnvironment pipelineEnvironment;
        private readonly Dictionary<string, ICodeFileLocationProvider> codeFileLocationProviders;

        public CodeStreamFactory(IPipelineEnvironment pipelineEnvironment, Dictionary<string, ICodeFileLocationProvider> codeFileLocationProviders)
        {
            this.pipelineEnvironment = pipelineEnvironment;
            this.codeFileLocationProviders = codeFileLocationProviders;
        }

        public ICodeStream CreateCodeStream(string language, string name)
        {
            if (!this.codeFileLocationProviders.ContainsKey(name))
            {
                throw new InvalidOperationException($"No code file location mapped for code stream {name}.");
            }

            return this.pipelineEnvironment.CreateCodeStream(language, name, this.codeFileLocationProviders[name]);
        }
    }
}
