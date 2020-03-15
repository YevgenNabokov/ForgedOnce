using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace ForgedOnce.Environment.Workspace
{
    public class WorkspaceCodeStreamProvider : ICodeStreamProvider
    {
        private readonly string language;
        private readonly string name;
        private readonly ICodeFileResolver codeFileResolver;
        private readonly ICodeFileLocationSelector<CodeFileLocation> locationsSelector;

        public WorkspaceCodeStreamProvider(
            string language,
            string name,
            ICodeFileResolver codeFileResolver,
            ICodeFileLocationSelector<CodeFileLocation> locationsSelector)
        {
            this.language = language;
            this.name = name;
            this.codeFileResolver = codeFileResolver;
            this.locationsSelector = locationsSelector;
        }

        public IEnumerable<ICodeStream> RetrieveCodeStreams()
        {
            Dictionary<CodeFileLocation, CodeFile> codeFiles = new Dictionary<CodeFileLocation, CodeFile>();
            foreach (var location in this.locationsSelector.GetLocations())
            {
                if (this.codeFileResolver.CanResolveCodeFile(this.language, location))
                {
                    if (!codeFiles.ContainsKey(location))
                    {
                        codeFiles.Add(location, this.codeFileResolver.ResolveCodeFile(this.language, location));
                    }
                }
            }

            return new ICodeStream[] { new CodeStream(this.language, this.name, codeFiles.Values) };
        }
    }
}
