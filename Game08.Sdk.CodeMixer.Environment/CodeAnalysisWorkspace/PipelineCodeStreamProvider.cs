using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace
{
    public class PipelineCodeStreamProvider : ICodeStreamProvider
    {
        private readonly IEnumerable<WorkspaceCodeStreamProvider> codeStreamProviders;

        public PipelineCodeStreamProvider(IEnumerable<WorkspaceCodeStreamProvider> codeStreamProviders)
        {
            this.codeStreamProviders = codeStreamProviders;
        }

        public IEnumerable<ICodeStream> RetrieveCodeStreams()
        {
            List<ICodeStream> result = new List<ICodeStream>();

            foreach (var provider in this.codeStreamProviders)
            {
                result.Add(provider.RetrieveCodeStream());
            }

            return result;
        }
    }
}
