using ForgedOnce.Core;
using ForgedOnce.Environment.Workspace;
using ForgedOnce.Environment.Interfaces;
using ForgedOnce.Launcher.MSBuild.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;
using ForgedOnce.Environment;

namespace ForgedOnce.Launcher.MSBuild.Storage
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

        public IEnumerable<WorkspaceCodeFileLocation> CodeFileLocations
        {
            get
            {
                List<WorkspaceCodeFileLocation> result = new List<WorkspaceCodeFileLocation>();

                foreach (var proj in this.Solution.Projects)
                {
                    foreach (var item in proj.Value.Items)
                    {
                        result.Add(
                            new WorkspaceCodeFileLocation()
                            {
                                DocumentPath = item.DocumentPath,
                                FilePath = item.FullPath
                            });
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
            else
            {
                var solutionDirectory = this.fileSystem.Path.GetDirectoryName(this.Solution.FullPath);
                if (!string.IsNullOrEmpty(codeFile.Location.FilePath)
                    && PathMaskHelper.DirectoryIsBaseOf(solutionDirectory, codeFile.Location.FilePath))
                {
                    if (this.msBuildStoreAdapters.Any(a => a.CodeFileSupported(codeFile)))
                    {
                        this.fileSystem.File.WriteAllText(codeFile.Location.FilePath, codeFile.SourceCodeText);
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Unable to save '{codeFile.Name}' mapped to '{codeFile.Location.FilePath}', path should be non-empty and be under directory containing solution '{solutionDirectory}'.");
                }
            }
        }

        public bool DocumentExists(string fullPath)
        {
            return this.Solution.Projects.Any(p => p.Value.Items.Any(i => i.FullPath == fullPath));
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
            else
            {
                var solutionDirectory = this.fileSystem.Path.GetDirectoryName(this.Solution.FullPath);
                if (!string.IsNullOrEmpty(codeFile.Location.FilePath)
                    && PathMaskHelper.DirectoryIsBaseOf(solutionDirectory, codeFile.Location.FilePath))
                {
                    if (this.fileSystem.File.Exists(codeFile.Location.FilePath))
                    {
                        this.fileSystem.File.Delete(codeFile.Location.FilePath);
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Unable to remove '{codeFile.Name}' mapped to '{codeFile.Location.FilePath}', path should be non-empty and be under directory containing solution '{solutionDirectory}'.");
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

        public bool CanResolveCodeFileName(CodeFileLocation location)
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
                if (!string.IsNullOrEmpty(location.FilePath))
                {
                    foreach (var project in this.Solution.Projects)
                    {
                        if (PathMaskHelper.DirectoryIsBaseOf(this.fileSystem.Path.GetDirectoryName(project.Value.FullPath), location.FilePath))
                        {
                            return project.Value;
                        }
                    }
                }                
            }

            return null;
        }
    }
}
