using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild.Storage
{
    public class MsBuildProject
    {
        private readonly string MsBuildProjectNamePropertyName = "MSBuildProjectName";

        public MsBuildProject(Project project)
        {
            this.Project = project;
        }

        public Project Project
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                var nameProp = this.Project.GetProperty(this.MsBuildProjectNamePropertyName);
                if (nameProp == null)
                {
                    throw new InvalidOperationException($"Property {MsBuildProjectNamePropertyName} was not found in project.");
                }

                return nameProp.EvaluatedValue;
            }
        }

        public string FullPath
        {
            get
            {
                return this.Project.FullPath;
            }
        }

        public IEnumerable<ProjectItem> FindProjectItems(string fullItemPath)
        {
            List<ProjectItem> result = new List<ProjectItem>();

            foreach (var item in this.Project.Items)
            {
                if (this.GetProjectItemFullPath(item.EvaluatedInclude) == Path.GetFullPath(fullItemPath))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private string GetProjectItemFullPath(string itemPath)
        {
            return Path.IsPathRooted(itemPath) ? Path.GetFullPath(itemPath) : Path.GetFullPath(Path.Combine(Path.GetDirectoryName(this.FullPath), itemPath));
        }
    }
}
