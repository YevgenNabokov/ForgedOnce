using Game08.Sdk.CodeMixer.Environment.Workspace;
using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild.Storage
{
    public class MsBuildItem
    {
        private readonly string LinkMetadataName = "Link";

        private readonly MsBuildProject project;
        private DocumentPath documentPath;

        public MsBuildItem(ProjectItem projectItem, MsBuildProject project)
        {
            this.Item = projectItem;
            this.project = project;
        }

        public ProjectItem Item
        {
            get;
            private set;
        }

        public string ItemType
        {
            get
            {
                return this.Item.ItemType;
            }
        }

        public string FullPath
        {
            get
            {
                return Path.IsPathRooted(this.Item.EvaluatedInclude) 
                    ? Path.GetFullPath(this.Item.EvaluatedInclude) 
                    : Path.GetFullPath(Path.Combine(Path.GetDirectoryName(this.Item.Project.FullPath), this.Item.EvaluatedInclude));
            }
        }

        public DocumentPath DocumentPath
        {
            get
            {
                if (this.documentPath == null)
                {
                    var inProjectPath = string.Empty;
                    var link = this.Item.Metadata.FirstOrDefault(m => m.Name == LinkMetadataName);
                    if (link != null)
                    {
                        inProjectPath = link.EvaluatedValue;
                    }
                    else
                    {
                        inProjectPath = this.Item.EvaluatedInclude;
                    }

                    var folders = inProjectPath.Split(Path.DirectorySeparatorChar).Where(p => !string.IsNullOrEmpty(p)).ToArray();

                    this.documentPath = new DocumentPath(this.project.Name, folders.Take(folders.Length - 1), Path.GetFileName(inProjectPath));
                }

                return this.documentPath;
            }
        }
    }
}
