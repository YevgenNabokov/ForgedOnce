using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Workspace;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Builders
{
    public class WorkspaceCodeFileLocationProviderBuilder : IBuilder<ICodeFileLocationProvider>
    {
        public const string PathKey = "path";

        private readonly IWorkspaceManagerBase workspaceManager;

        public string Name => null;

        public WorkspaceCodeFileLocationProviderBuilder(IWorkspaceManagerBase workspaceManager)
        {
            this.workspaceManager = workspaceManager;
        }

        public ICodeFileLocationProvider Build(JObject configuration)
        {
            if (!configuration.ContainsKey(PathKey))
            {
                throw new InvalidOperationException($"Settings for {nameof(WorkspaceCodeFileLocationProviderBuilder)} should contain {PathKey}.");
            }

            var path = configuration[PathKey].Value<string>();

            return new WorkspaceCodeFileLocationProvider(this.workspaceManager, path);
        }
    }
}
