﻿using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.CodeMixer.LimitedTypeScript.Helpers;
using Game08.Sdk.LTS.Model.DefinitionTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public class CodeFileCompilationHandlerLts : ICodeFileCompilationHandler
    {
        private SearchVisitor search = new SearchVisitor();

        private UpdateTypeReferencesVisitor referenceUpdater = new UpdateTypeReferencesVisitor();

        private List<CodeFileLtsModel> codeFiles = new List<CodeFileLtsModel>();

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