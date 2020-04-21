using ForgedOnce.Core;
using ForgedOnce.Environment.Workspace;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.TypeScript.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using ForgedOnce.TsLanguageServices.ModelBuilder.DefinitionTree;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Pipeline;
using ForgedOnce.TsLanguageServices.ModelBuilder.SyntaxTools;

namespace ForgedOnce.TypeScript
{
    public class CodeFileCompilationHandlerTs : ICodeFileCompilationHandler
    {
        private readonly IPipelineExecutionInfo pipelineExecutionInfo;
        private readonly ILogger logger;
        private SearchVisitor search = new SearchVisitor();

        private UpdateTypeReferencesVisitor referenceUpdater = new UpdateTypeReferencesVisitor();

        private List<CodeFileTsModel> codeFiles = new List<CodeFileTsModel>();

        public CodeFileCompilationHandlerTs(IPipelineExecutionInfo pipelineExecutionInfo, ILogger logger)
        {
            this.pipelineExecutionInfo = pipelineExecutionInfo;
            this.logger = logger;
        }

        public ShadowFilter ShadowFilter { get; set; }

        public void RefreshAndRecompile()
        {
            foreach (var codeFile in this.codeFiles)
            {
                this.UpdateCodeFileTypeLocation(codeFile);
            }
        }

        public void Register(CodeFile codeFile)
        {
            var ltsCodeFile = codeFile as CodeFileTsModel;
            if (!this.codeFiles.Contains(ltsCodeFile))
            {
                this.codeFiles.Add(ltsCodeFile);
            }
        }

        public bool SupportsCodeLanguage(string language)
        {
            return Languages.LimitedTypeScript == language;
        }

        public void Unregister(CodeFile codeFile)
        {
            var ltsCodeFile = codeFile as CodeFileTsModel;
            if (this.codeFiles.Contains(ltsCodeFile))
            {
                this.codeFiles.Remove(ltsCodeFile);
            }
        }

        private void UpdateCodeFileTypeLocation(CodeFileTsModel codeFileTsModel)
        {
            CloningDefinitionTreeVisitor cloner = new CloningDefinitionTreeVisitor();
            if (codeFileTsModel.Model != null)
            {
                var codeFileTsModelClone = cloner.CloneNode(codeFileTsModel.Model);
                foreach (var node in search.FindNodes<NamedTypeDefinition>(codeFileTsModelClone))
                {
                    var result = codeFileTsModel.TypeRepository.UpdateTypeDefinitionFile(node.TypeKey, codeFileTsModel.GetPath());
                    node.TypeKey = result.NewTypeDefinitionId;
                    foreach (var f in this.codeFiles)
                    {
                        var modelCopy = cloner.CloneNode(f.Model);
                        this.referenceUpdater.Visit(modelCopy, result.ReferneceIdUpdates);
                        f.SetModelOverrideReadonly(modelCopy);
                    }
                }

                codeFileTsModel.SetModelOverrideReadonly(codeFileTsModelClone);
            }
        }
    }
}
