using ForgedOnce.Core;
using ForgedOnce.Environment.Workspace;
using ForgedOnce.Environment.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using ForgedOnce.Environment.Workspace.CodeAnalysis;
using ForgedOnce.Core.Interfaces;
using System.Linq;
using ForgedOnce.Core.Logging;
using ForgedOnce.Core.Pipeline;
using System.Threading.Tasks;

namespace ForgedOnce.CSharp
{
    public class CodeFileCompilationHandlerCSharp : ICodeFileCompilationHandler
    {
        private IWorkspaceManager workspaceManager;
        private readonly IPipelineExecutionInfo pipelineExecutionInfo;
        private readonly ILogger logger;
        private WorkspaceCompilationHandler compilationHandler;

        private List<CodeFileCSharp> codeFiles = new List<CodeFileCSharp>();

        public CodeFileCompilationHandlerCSharp(IWorkspaceManager workspaceManager, IPipelineExecutionInfo pipelineExecutionInfo, ILogger logger)
        {
            this.workspaceManager = workspaceManager;
            this.pipelineExecutionInfo = pipelineExecutionInfo;
            this.logger = logger;
            this.compilationHandler = new WorkspaceCompilationHandler(workspaceManager);
        }

        public ShadowFilter ShadowFilter { get; set; }

        public void RefreshAndRecompile()
        {
            List<string> projectsToRebuild = new List<string>();

            var codeFilesToRebuild = this.ShadowFilter == null ? this.codeFiles : this.codeFiles.Where(f => !this.ShadowFilter.IsMatch(f.Location));

            var shadowed = this.ShadowCodeFiles();

            foreach (var file in codeFilesToRebuild)
            {
                var projName = (file.Location as WorkspaceCodeFileLocation).DocumentPath.ProjectName;

                if (!projectsToRebuild.Contains(projName))
                {
                    projectsToRebuild.Add(projName);
                }
            }

            Dictionary<string, Compilation> compilations = this.compilationHandler.CompileProjects(projectsToRebuild);

            this.AssertCompilationResults(compilations);

            foreach (var file in codeFilesToRebuild)
            {
                var location = file.Location as WorkspaceCodeFileLocation;                
                var document = this.workspaceManager.FindDocument(location.DocumentPath);

                file.SyntaxTree = document.GetSyntaxTreeAsync().Result;
                file.SemanticModel = compilations[document.Project.Name].GetSemanticModel(file.SyntaxTree);
            }

            this.UnshadowCodeFiles(shadowed);
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

        private Dictionary<WorkspaceCodeFileLocation, string> ShadowCodeFiles()
        {
            Dictionary<WorkspaceCodeFileLocation, string> result = new Dictionary<WorkspaceCodeFileLocation, string>();
            if (this.ShadowFilter != null)
            {
                List<WorkspaceCodeFileLocation> locations = new List<WorkspaceCodeFileLocation>();
                foreach (var location in this.workspaceManager.CodeFileLocations)
                {
                    if (this.ShadowFilter.IsMatch(location))
                    {
                        locations.Add(location);
                    }
                }

                foreach (var location in locations)
                {
                    var doc = this.workspaceManager.FindDocument(location.DocumentPath);
                    var text = doc.GetTextAsync().Result.ToString();
                    this.workspaceManager.RemoveCodeFile(location.DocumentPath);
                    result.Add(location, text);
                }
            }

            return result;
        }

        private void UnshadowCodeFiles(Dictionary<WorkspaceCodeFileLocation, string> files)
        {
            foreach (var f in files)
            {
                this.workspaceManager.AddCodeFile(f.Key.DocumentPath, f.Value, f.Key.FilePath);
            }
        }

        private void AssertCompilationResults(Dictionary<string, Compilation> compilations)
        {
            StringBuilder logMessage = new StringBuilder();
            StringBuilder exceptionMessage = new StringBuilder();
            foreach(var c in compilations)
            {
                var projectHadErrors = false;
                var diagnostics = c.Value.GetDiagnostics();
                if (diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error))
                {
                    exceptionMessage.Append($"{c.Key} ({diagnostics.Count(d => d.Severity == DiagnosticSeverity.Error)});");
                }

                foreach (var d in diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
                {
                    if (!projectHadErrors)
                    {
                        logMessage.Append($"Project {c.Key} had compilation errors:\r\n");
                        projectHadErrors = true;
                    }

                    logMessage.Append($"\t{d.GetMessage()}\r\n");
                }
            }

            if (logMessage.Length > 0)
            {
                this.logger.Write(new LogRecord(MessageSeverity.Warning, logMessage.ToString()));

                exceptionMessage.Insert(0, "CSharp projects have compilation errors: ");

                throw new InvalidOperationException(exceptionMessage.ToString());
            }
        }
    }
}
