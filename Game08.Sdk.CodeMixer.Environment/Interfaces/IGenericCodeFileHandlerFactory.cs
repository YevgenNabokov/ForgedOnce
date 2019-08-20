using Game08.Sdk.CodeMixer.Core.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface ICodeFileHandlerFactory
    {
        ICodeFileEnvironmentHandler Create(IWorkspaceManager workspaceManager, JObject configuration = null);
    }
}
