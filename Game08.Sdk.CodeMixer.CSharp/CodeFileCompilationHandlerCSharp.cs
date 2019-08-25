using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.CSharp
{
    public class CodeFileCompilationHandlerCSharp : ICodeFileCompilationHandler
    {
        private IWorkspaceManager workspaceManager;

        private List<CodeFileCSharp> codeFiles = new List<CodeFileCSharp>();

        public CodeFileCompilationHandlerCSharp(IWorkspaceManager workspaceManager)
        {
            this.workspaceManager = workspaceManager;
        }

        public void RefreshAndRecompile()
        {
            List<Guid> projectsToRebuild = new List<Guid>();

            foreach (var file in this.codeFiles)
            {
                var projId = (file.Location as WorkspaceCodeFileLocation).ProjectId;

                if (!projectsToRebuild.Contains(projId))
                {
                    projectsToRebuild.Add(projId);
                }
            }

            var chains = this.workspaceManager.GetProjectsDependencyChains(projectsToRebuild);

            Dictionary<Guid, Compilation> compilations = new Dictionary<Guid, Compilation>();

            foreach (var chain in chains)
            {
                foreach (var id in chain)
                {
                    if (!compilations.ContainsKey(id))
                    {
                        var project = this.workspaceManager.FindProject(id);
                        var compilation = project.GetCompilationAsync().Result;
                        compilations.Add(id, compilation);
                    }
                }
            }

            foreach (var file in this.codeFiles)
            {
                var docId = (file.Location as WorkspaceCodeFileLocation).DocumentId;
                var document = this.workspaceManager.FindDocument(docId);

                file.SyntaxTree = document.GetSyntaxTreeAsync().Result;
                file.SemanticModel = compilations[document.Project.Id.Id].GetSemanticModel(file.SyntaxTree);
            }
        }

        public void Register(CodeFile codeFile)
        {
            if (codeFile != null)
            {
                if (!(codeFile is CodeFileCSharp))
                {
                    throw new NotSupportedException($"{nameof(CodeFileCompilationHandlerCSharp)} supports only {typeof(CodeFileCSharp)}.");
                }

                this.codeFiles.Add(codeFile as CodeFileCSharp);
            }
        }

        public bool SupportsCodeLanguage(string language)
        {
            return language == Languages.CSharp;
        }

        public void Unregister(CodeFile codeFile)
        {
            if (codeFile != null)
            {
                if (!(codeFile is CodeFileCSharp))
                {
                    throw new NotSupportedException($"{nameof(CodeFileCompilationHandlerCSharp)} supports only {typeof(CodeFileCSharp)}.");
                }

                this.codeFiles.Remove(codeFile as CodeFileCSharp);
            }
        }
    }
}
