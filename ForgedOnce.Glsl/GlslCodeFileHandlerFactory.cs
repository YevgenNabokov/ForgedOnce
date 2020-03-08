using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Glsl
{
    public class GlslCodeFileHandlerFactory : ICodeFileEnvironmentHandlerFactory
    {
        public ICodeFileEnvironmentHandler Create(IWorkspaceManager workspaceManager, IFileSystem fileSystem, IPipelineExecutionInfo pipelineExecutionInfo, ILogger logger, JObject configuration = null)
        {
            return new CodeFileEnvironmentHandlerGlsl(pipelineExecutionInfo, fileSystem, logger);
        }
    }
}
