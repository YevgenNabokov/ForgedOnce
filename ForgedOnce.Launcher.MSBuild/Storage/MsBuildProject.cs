using ForgedOnce.Environment;
using ForgedOnce.Environment.Workspace;
using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Launcher.MSBuild.Storage
{
    public class MsBuildProject
    {
        private readonly string MsBuildProjectNamePropertyName = "MSBuildProjectName";
        private readonly string UsingNETSdkDefaultsPropertyName = "UsingNETSdkDefaults";
        private readonly string EnableDefaultItemsPropertyName = "EnableDefaultItems";
        private readonly string EnableDefaultCompileItemsPropertyName = "EnableDefaultCompileItems";
        private readonly string EnableDefaultEmbeddedResourceItemsPropertyName = "EnableDefaultEmbeddedResourceItems";
        private readonly string EnableDefaultNoneItemsPropertyName = "EnableDefaultNoneItems";

        private readonly IFileSystem fileSystem;
        private List<MsBuildItem> items = new List<MsBuildItem>();

        public MsBuildProject(Project project, IFileSystem fileSystem)
        {
            this.Project = project;
            this.fileSystem = fileSystem;
            foreach (var item in project.ItemsIgnoringCondition)
            {
                this.items.Add(new MsBuildItem(item, this));
            }
        }

        protected Project Project
        {
            get;
            private set;
        }

        public IEnumerable<MsBuildItem> Items
        {
            get
            {
                return this.items;
            }
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

        public bool UsingNETSdkDefaults
        {
            get
            {
                var prop = this.Project.GetProperty(this.UsingNETSdkDefaultsPropertyName);
                if (prop == null)
                {
                    return false;
                }

                bool result;
                bool.TryParse(prop.EvaluatedValue, out result);
                return result;
            }
        }

        public string FullPath
        {
            get
            {
                return this.Project.FullPath;
            }
        }

        public IEnumerable<MsBuildItem> FindProjectItems(string fullItemPath)
        {
            List<MsBuildItem> result = new List<MsBuildItem>();

            foreach (var item in this.Items)
            {
                if (item.FullPath == Path.GetFullPath(fullItemPath))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public IEnumerable<MsBuildItem> AddItem(string itemType, string unevaluatedInclude)
        {
            var result = new List<MsBuildItem>();
            if (this.IsUsingDefaultItemsForItemType(itemType))
            {
                string fullPath = null;
                string projectDirectory = this.fileSystem.Path.GetDirectoryName(this.FullPath);
                if (this.fileSystem.Path.IsPathRooted(unevaluatedInclude))
                {
                    fullPath = this.fileSystem.Path.GetFullPath(unevaluatedInclude);
                }
                else
                {
                    fullPath = this.fileSystem.Path.GetFullPath(this.fileSystem.Path.Combine(projectDirectory, unevaluatedInclude));
                }
                
                if (!PathMaskHelper.DirectoryIsBaseOf(projectDirectory, fullPath))
                {
                    throw new InvalidOperationException($"Project {this.Name} is using default items for {itemType}, item path {fullPath} should be under project path {projectDirectory}.");
                }

                var itemDirectory = this.fileSystem.Path.GetDirectoryName(fullPath);
                DocumentPath docPath = new DocumentPath(
                    this.Name,
                    itemDirectory.Remove(0, projectDirectory.Length).Split(new char[] { this.fileSystem.Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries),
                    this.fileSystem.Path.GetFileName(fullPath));

                if (!this.fileSystem.File.Exists(fullPath))
                {
                    this.fileSystem.File.WriteAllText(fullPath, string.Empty);
                }

                var newItem = new MsBuildItem(docPath, fullPath, itemType, this);
                this.items.Add(newItem);
                result.Add(newItem);
            }
            else
            {
                foreach (var item in this.Project.AddItem(itemType, unevaluatedInclude))
                {
                    var newItem = new MsBuildItem(item, this);
                    this.items.Add(newItem);
                    result.Add(newItem);
                }
            }

            return result;
        }

        public void RemoveItem(MsBuildItem item)
        {
            if (!this.IsUsingDefaultItemsForItemType(item.ItemType))
            {
                this.Project.RemoveItem(item.Item);
            }

            if (this.fileSystem.File.Exists(item.FullPath))
            {
                this.fileSystem.File.Delete(item.FullPath);
            }

            this.items.Remove(item);
        }        

        public void Save()
        {
            this.Project.Save();
        }

        public bool IsUsingDefaultCompileItems
        {
            get
            {
                return this.EnableDefaultItems && this.EnableDefaultCompileItems;
            }
        }

        public bool IsUsingDefaultEmbeddedResourceItems
        {
            get
            {
                return this.EnableDefaultItems && this.EnableDefaultEmbeddedResourceItems;
            }
        }

        public bool IsUsingDefaultNoneItems
        {
            get
            {
                return this.EnableDefaultItems && this.EnableDefaultNoneItems;
            }
        }

        protected bool EnableDefaultItems
        {
            get
            {
                var prop = this.Project.GetProperty(this.EnableDefaultItemsPropertyName);
                if (prop == null)
                {
                    return this.UsingNETSdkDefaults;
                }

                bool result;
                bool.TryParse(prop.EvaluatedValue, out result);
                return result;
            }
        }

        protected bool EnableDefaultCompileItems
        {
            get
            {
                var prop = this.Project.GetProperty(this.EnableDefaultCompileItemsPropertyName);
                if (prop == null)
                {
                    return this.UsingNETSdkDefaults;
                }

                bool result;
                bool.TryParse(prop.EvaluatedValue, out result);
                return result;
            }
        }

        protected bool EnableDefaultEmbeddedResourceItems
        {
            get
            {
                var prop = this.Project.GetProperty(this.EnableDefaultEmbeddedResourceItemsPropertyName);
                if (prop == null)
                {
                    return this.UsingNETSdkDefaults;
                }

                bool result;
                bool.TryParse(prop.EvaluatedValue, out result);
                return result;
            }
        }

        protected bool EnableDefaultNoneItems
        {
            get
            {
                var prop = this.Project.GetProperty(this.EnableDefaultNoneItemsPropertyName);
                if (prop == null)
                {
                    return this.UsingNETSdkDefaults;
                }

                bool result;
                bool.TryParse(prop.EvaluatedValue, out result);
                return result;
            }
        }

        protected bool IsUsingDefaultItemsForItemType(string itemType)
        {
            switch (itemType)
            {
                case "Compile": return this.IsUsingDefaultCompileItems;
                case "None": return this.IsUsingDefaultNoneItems;
                default: return false;
            }
        }
    }
}
