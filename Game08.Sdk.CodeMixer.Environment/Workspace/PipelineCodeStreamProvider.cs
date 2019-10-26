using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Workspace
{
    public class PipelineCodeStreamProvider : ICodeStreamProvider
    {
        private readonly IEnumerable<ICodeStreamProvider> codeStreamProviders;

        public PipelineCodeStreamProvider(IEnumerable<ICodeStreamProvider> codeStreamProviders)
        {
            this.codeStreamProviders = codeStreamProviders;
        }

        public IEnumerable<ICodeStream> RetrieveCodeStreams()
        {
            List<ICodeStream> result = new List<ICodeStream>();

            foreach (var provider in this.codeStreamProviders)
            {
                result.AddRange(provider.RetrieveCodeStreams());
            }

            return result;
        }
    }
}
