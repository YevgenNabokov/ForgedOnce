using ForgedOnce.Core.Interfaces;
using ForgedOnce.Environment.Workspace;
using ForgedOnce.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Environment.Builders
{
    public class WorkspaceCodeFileDestinationBuilder : IBuilder<ICodeFileDestination>
    {
        public const string PathKey = "path";

        private readonly IPipelineWorkspaceManagers workspaces;

        public string Name => null;

        public WorkspaceCodeFileDestinationBuilder(IPipelineWorkspaceManagers workspaces)
        {
            this.workspaces = workspaces;
        }

        public ICodeFileDestination Build(JObject configuration)
        {
            if (!configuration.ContainsKey(PathKey))
            {
                throw new InvalidOperationException($"Settings for {nameof(WorkspaceCodeFileDestinationBuilder)} should contain {PathKey}.");
            }

            var path = configuration[PathKey].Value<string>();

            return new WorkspaceCodeFileDestination(this.workspaces, path);
        }
    }
}
