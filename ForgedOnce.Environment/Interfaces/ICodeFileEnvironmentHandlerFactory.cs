using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface ICodeFileEnvironmentHandlerFactory
    {
        ICodeFileEnvironmentHandler Create(IWorkspaceManager workspaceManager, IFileSystem fileSystem, IPipelineExecutionInfo pipelineExecutionInfo, ILogger logger, JObject configuration = null);
    }
}
