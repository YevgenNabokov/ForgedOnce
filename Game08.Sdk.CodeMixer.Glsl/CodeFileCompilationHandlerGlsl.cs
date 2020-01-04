using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl
{
    public class CodeFileCompilationHandlerGlsl : ICodeFileCompilationHandler
    {
        private readonly IPipelineExecutionInfo pipelineExecutionInfo;
        private readonly ILogger logger;
        private List<CodeFileGlsl> codeFiles = new List<CodeFileGlsl>();

        public CodeFileCompilationHandlerGlsl(IPipelineExecutionInfo pipelineExecutionInfo, ILogger logger)
        {
            this.pipelineExecutionInfo = pipelineExecutionInfo;
            this.logger = logger;
        }

        public void RefreshAndRecompile()
        {
            foreach (var codeFile in this.codeFiles)
            {
                codeFile.ShaderFile.RebuildSemanticContext();                
            }
        }

        public void Register(CodeFile codeFile)
        {
            var glslCodeFile = codeFile as CodeFileGlsl;
            if (!this.codeFiles.Contains(glslCodeFile))
            {
                this.codeFiles.Add(glslCodeFile);
            }
        }

        public bool SupportsCodeLanguage(string language)
        {
            return Languages.Glsl == language;
        }

        public void Unregister(CodeFile codeFile)
        {
            var glslCodeFile = codeFile as CodeFileGlsl;
            if (this.codeFiles.Contains(glslCodeFile))
            {
                this.codeFiles.Remove(glslCodeFile);
            }
        }
    }
}
