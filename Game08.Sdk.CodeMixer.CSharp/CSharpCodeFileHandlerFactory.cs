using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp
{
    public class CSharpCodeFileHandlerFactory : ICodeFileHandlerFactory
    {
        public ICodeFileEnvironmentHandler Create(IWorkspaceManager workspaceManager, JObject configuration = null)
        {
            return new CodeFileEnvironmentHandlerCSharp(workspaceManager);
        }
    }
}
