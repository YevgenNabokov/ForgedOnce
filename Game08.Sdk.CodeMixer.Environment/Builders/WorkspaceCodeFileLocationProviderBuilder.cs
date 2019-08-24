using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Builders
{
    public class WorkspaceCodeFileLocationProviderBuilder : IBuilder<ICodeFileLocationProvider>
    {
        private readonly IWorkspaceManager workspaceManager;

        public string Name => null;

        public WorkspaceCodeFileLocationProviderBuilder(IWorkspaceManager workspaceManager)
        {
            this.workspaceManager = workspaceManager;
        }

        public ICodeFileLocationProvider Build(JObject configuration)
        {
            var path = configuration.Value<string>();

            return new WorkspaceCodeFileLocationProvider(this.workspaceManager, path);
        }
    }
}
