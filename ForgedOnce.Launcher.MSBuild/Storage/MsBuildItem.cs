﻿using ForgedOnce.Environment.Workspace;
using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ForgedOnce.Launcher.MSBuild.Storage
{
    public class MsBuildItem
    {
        private readonly string LinkMetadataName = "Link";

        private readonly MsBuildProject project;
        private DocumentPath documentPath;
        private readonly string fullPath = null;
        private readonly string itemType = null;

        public MsBuildItem(ProjectItem projectItem, MsBuildProject project)
        {
            this.Item = projectItem;
            this.project = project;
        }

        public MsBuildItem(DocumentPath documentPath, string fullPath, string itemType, MsBuildProject project)
        {
            this.documentPath = documentPath;
            this.fullPath = fullPath;
            this.itemType = itemType;
            this.project = project;
        }

        internal ProjectItem Item
        {
            get;
            private set;
        }

        public string ItemType
        {
            get
            {
                return this.itemType ?? this.Item.ItemType;
            }
        }

        public string FullPath
        {
            get
            {
                return this.fullPath ?? (
                    Path.IsPathRooted(this.Item.EvaluatedInclude) 
                    ? Path.GetFullPath(this.Item.EvaluatedInclude) 
                    : Path.GetFullPath(Path.Combine(Path.GetDirectoryName(this.Item.Project.FullPath), this.Item.EvaluatedInclude)));
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
