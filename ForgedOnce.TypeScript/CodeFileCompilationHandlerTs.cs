using ForgedOnce.Core;
using ForgedOnce.Environment.Workspace;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.TypeScript.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Pipeline;

namespace ForgedOnce.TypeScript
{
    public class CodeFileCompilationHandlerTs : ICodeFileCompilationHandler
    {
        private readonly IPipelineExecutionInfo pipelineExecutionInfo;
        private readonly ILogger logger;
        private SearchVisitor search = new SearchVisitor();

        private List<CodeFileTs> codeFiles = new List<CodeFileTs>();

        public CodeFileCompilationHandlerTs(IPipelineExecutionInfo pipelineExecutionInfo, ILogger logger)
        {
            this.pipelineExecutionInfo = pipelineExecutionInfo;
            this.logger = logger;
        }

        public ShadowFilter ShadowFilter { get; set; }

        public void RefreshAndRecompile()
        {
        }

        public void Register(CodeFile codeFile)
        {
            var ltsCodeFile = codeFile as CodeFileTs;
            if (!this.codeFiles.Contains(ltsCodeFile))
            {
                this.codeFiles.Add(ltsCodeFile);
            }
        }

        public bool SupportsCodeLanguage(string language)
        {
            return Languages.TypeScript == language;
        }

        public void Unregister(CodeFile codeFile)
        {
            var ltsCodeFile = codeFile as CodeFileTs;
            if (this.codeFiles.Contains(ltsCodeFile))
            {
                this.codeFiles.Remove(ltsCodeFile);
            }
        }
    }
}
