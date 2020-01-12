using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.Workspace;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild.Storage
{
    public class MsBuildSolutionStorage : IWorkspaceManagerBase
    {
        private readonly string solutionPath;
        private readonly IEnumerable<IMsBuildCodeFileStoreAdapter> msBuildStoreAdapters;
        private readonly IFileSystem fileSystem;
        private MsBuildSolution solution;

        public MsBuildSolutionStorage(string solutionPath, IEnumerable<IMsBuildCodeFileStoreAdapter> msBuildStoreAdapters, IFileSystem fileSystem)
        {
            this.solutionPath = solutionPath;
            this.msBuildStoreAdapters = msBuildStoreAdapters;
            this.fileSystem = fileSystem;
        }

        public IEnumerable<DocumentPath> DocumentPaths
        {
            get
            {
                List<DocumentPath> result = new List<DocumentPath>();

                foreach (var proj in this.Solution.Projects)
                {
                    foreach (var item in proj.Value.Items)
                    {
                        result.Add(item.DocumentPath);
                    }
                }

                return result;
            }
        }

        private MsBuildSolution Solution
        {
            get
            {
                if (this.solution == null)
                {
                    this.solution = MsBuildSolution.Load(this.solutionPath, this.fileSystem);
                }

                return this.solution;
            }
        }

        public void Add(CodeFile codeFile)
        {
            var targetProj = this.GetContainingProject(codeFile.Location);

            if (targetProj != null)
            {
                foreach (var adapter in this.msBuildStoreAdapters)
                {
                    if (adapter.CodeFileSupported(codeFile))
                    {
                        adapter.AddOrUpdate(codeFile, targetProj);
                        return;
                    }
                }

                throw new InvalidOperationException($"No MsBuild store adapter was found for {codeFile}.");
            }
        }

        public bool DocumentExists(string fullPath)
        {
            return this.Solution.Projects.Any(p => p.Value.Items.Any(i => i.FullPath == fullPath));
        }

        public WorkspaceCodeFileLocation GetDocumentLocationByPath(DocumentPath documentPath)
        {
            var project = this.Solution.GetProject(documentPath.ProjectName);

            if (project != null)
            {
                var item = project.Items.FirstOrDefault(i => i.DocumentPath.Equals(documentPath));

                if (item != null)
                {
                    return new WorkspaceCodeFileLocation()
                    {
                        DocumentPath = item.DocumentPath,
                        FilePath = item.FullPath
                    };
                }
            }

            return null;
        }

        public bool ProjectExists(string projectName)
        {
            return this.Solution.Projects.Any(p => p.Value.Name == projectName);
        }

        public void Remove(CodeFile codeFile)
        {
            var targetProj = this.GetContainingProject(codeFile.Location);
            if (targetProj != null)
            {
                foreach (var adapter in this.msBuildStoreAdapters)
                {
                    if (adapter.CodeFileSupported(codeFile))
                    {
                        adapter.Remove(codeFile, targetProj);
                        return;
                    }
                }
            }
        }

        public void RemoveCodeFile(DocumentPath documentPath)
        {
            MsBuildProject container = null;
            MsBuildItem itemToRemove = null;

            foreach (var proj in this.Solution.Projects)
            {
                if (proj.Value.Name == documentPath.ProjectName)
                {
                    foreach (var item in proj.Value.Items)
                    {
                        if (item.DocumentPath.Equals(documentPath) && this.msBuildStoreAdapters.Any(a => a.ItemSupported(item)))
                        {
                            container = proj.Value;
                            itemToRemove = item;
                            break;
                        }
                    }
                }
            }

            if (itemToRemove != null)
            {
                container.RemoveItem(itemToRemove);
            }
        }

        public void ResolveCodeFile(CodeFile codeFile, bool resolveSourceCodeText = true, bool resolveLocation = true)
        {
            throw new NotImplementedException();
        }

        public string ResolveCodeFileName(CodeFileLocation location)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            foreach(var proj in this.Solution.Projects)
            {
                proj.Value.Save();
            }
        }

        private MsBuildProject GetContainingProject(CodeFileLocation location)
        {
            if (location is WorkspaceCodeFileLocation)
            {
                var workspaceLocation = location as WorkspaceCodeFileLocation;
                return this.Solution.GetProject(workspaceLocation.DocumentPath.ProjectName);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
