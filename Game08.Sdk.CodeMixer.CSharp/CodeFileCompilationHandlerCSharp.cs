using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.Workspace;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using Game08.Sdk.CodeMixer.Environment.Workspace.CodeAnalysis;

namespace Game08.Sdk.CodeMixer.CSharp
{
    public class CodeFileCompilationHandlerCSharp : ICodeFileCompilationHandler
    {
        private IWorkspaceManager workspaceManager;

        private WorkspaceCompilationHandler compilationHandler;

        private List<CodeFileCSharp> codeFiles = new List<CodeFileCSharp>();

        public CodeFileCompilationHandlerCSharp(IWorkspaceManager workspaceManager)
        {
            this.workspaceManager = workspaceManager;
            this.compilationHandler = new WorkspaceCompilationHandler(workspaceManager);
        }

        public void RefreshAndRecompile()
        {
            List<string> projectsToRebuild = new List<string>();

            foreach (var file in this.codeFiles)
            {
                var projName = (file.Location as WorkspaceCodeFileLocation).DocumentPath.ProjectName;

                if (!projectsToRebuild.Contains(projName))
                {
                    projectsToRebuild.Add(projName);
                }
            }

            Dictionary<string, Compilation> compilations = this.compilationHandler.CompileProjects(projectsToRebuild);

            foreach (var file in this.codeFiles)
            {
                var location = file.Location as WorkspaceCodeFileLocation;                
                var document = this.workspaceManager.FindDocument(location.DocumentPath);

                file.SyntaxTree = document.GetSyntaxTreeAsync().Result;
                file.SemanticModel = compilations[document.Project.Name].GetSemanticModel(file.SyntaxTree);
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
