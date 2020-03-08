using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace ForgedOnce.Launcher.MSBuild.Storage
{
    public class MsBuildSolution
    {
        private readonly IFileSystem fileSystem;

        protected MsBuildSolution(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public static MsBuildSolution Load(string solutionPath, IFileSystem fileSystem)
        {
            MsBuildSolution result = new MsBuildSolution(fileSystem);
            result.SolutionFile = SolutionFile.Parse(solutionPath);
            result.FullPath = solutionPath;
            result.LoadProjects();
            return result;
        }

        public SolutionFile SolutionFile
        {
            get;
            private set;
        }

        public string FullPath
        {
            get;
            private set;
        }

        public ReadOnlyDictionary<Guid, MsBuildProject> Projects
        {
            get;
            private set;
        }

        public MsBuildProject GetProject(string projectName)
        {
            return this.Projects.Values.FirstOrDefault(p => p.Name == projectName);
        }

        private void LoadProjects()
        {
            Dictionary<Guid, MsBuildProject> projects = new Dictionary<Guid, MsBuildProject>();

            var projectCollection = new ProjectCollection();

            foreach (var proj in this.SolutionFile.ProjectsInOrder)
            {
                var loadedProj = new MsBuildProject(new Project(proj.AbsolutePath, null, null, projectCollection), this.fileSystem);
                projects.Add(Guid.Parse(proj.ProjectGuid), loadedProj);
            }

            this.Projects = new ReadOnlyDictionary<Guid, MsBuildProject>(projects);
        }
    }
}
