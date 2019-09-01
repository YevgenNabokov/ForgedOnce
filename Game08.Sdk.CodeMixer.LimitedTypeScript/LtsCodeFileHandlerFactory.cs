using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public class LtsCodeFileHandlerFactory : ICodeFileHandlerFactory
    {
        public ICodeFileEnvironmentHandler Create(IWorkspaceManager workspaceManager, IFileSystem fileSystem, JObject configuration = null)
        {
            return new CodeFileEnvironmentHandlerLts(fileSystem);
        }
    }
}
