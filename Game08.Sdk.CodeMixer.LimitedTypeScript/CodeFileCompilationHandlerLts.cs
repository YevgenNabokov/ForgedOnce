using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.Workspace;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.CodeMixer.LimitedTypeScript.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Game08.Sdk.LTS.Builder.DefinitionTree;
using Game08.Sdk.CodeMixer.Core.Interfaces;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public class CodeFileCompilationHandlerLts : ICodeFileCompilationHandler
    {
        private readonly IPipelineExecutionInfo pipelineExecutionInfo;
        private SearchVisitor search = new SearchVisitor();

        private UpdateTypeReferencesVisitor referenceUpdater = new UpdateTypeReferencesVisitor();

        private List<CodeFileLtsModel> codeFiles = new List<CodeFileLtsModel>();

        public CodeFileCompilationHandlerLts(IPipelineExecutionInfo pipelineExecutionInfo)
        {
            this.pipelineExecutionInfo = pipelineExecutionInfo;
        }

        public void RefreshAndRecompile()
        {
            foreach (var codeFile in this.codeFiles)
            {
                this.UpdateCodeFileTypeLocation(codeFile);
            }
        }

        public void Register(CodeFile codeFile)
        {
            var ltsCodeFile = codeFile as CodeFileLtsModel;
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
            var ltsCodeFile = codeFile as CodeFileLtsModel;
            if (this.codeFiles.Contains(ltsCodeFile))
            {
                this.codeFiles.Remove(ltsCodeFile);
            }
        }

        private void UpdateCodeFileTypeLocation(CodeFileLtsModel codeFileLtsModel)
        {
            if (codeFileLtsModel.Model != null)
            {
                foreach (var node in search.FindNodes<NamedTypeDefinition>(codeFileLtsModel.Model))
                {
                    var result = codeFileLtsModel.TypeRepository.UpdateTypeDefinitionFile(node.TypeKey, codeFileLtsModel.GetPath());
                    node.TypeKey = result.NewTypeDefinitionId;
                    foreach (var f in this.codeFiles)
                    {
                        this.referenceUpdater.Visit(f.Model, result.ReferneceIdUpdates);
                    }
                }
            }
        }
    }
}
