using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp
{
    public class CSharpCodeFileHandlerFactory : ICodeFileHandlerFactory
    {
        public ICodeFileEnvironmentHandler Create(IWorkspaceManager workspaceManager, IFileSystem fileSystem, IPipelineExecutionInfo pipelineExecutionInfo, ILogger logger, JObject configuration = null)
        {
            return new CodeFileEnvironmentHandlerCSharp(workspaceManager, pipelineExecutionInfo, logger);
        }
    }
}
