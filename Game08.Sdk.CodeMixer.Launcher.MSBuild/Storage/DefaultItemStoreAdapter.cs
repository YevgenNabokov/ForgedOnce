using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment;
using Game08.Sdk.CodeMixer.Environment.Workspace;
using Game08.Sdk.CodeMixer.Launcher.MSBuild.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild.Storage
{
    public class DefaultItemStoreAdapter : IMsBuildCodeFileStoreAdapter
    {
        public const string NoneItemTypeName = "None";
        private readonly IFileSystem fileSystem;
        private readonly string itemType;

        public DefaultItemStoreAdapter(IFileSystem fileSystem, string itemType = NoneItemTypeName)
        {
            this.fileSystem = fileSystem;
            this.itemType = itemType;
        }

        public virtual void AddOrUpdate(CodeFile codeFile, MsBuildProject msBuildProject)
        {
            var relativeItemPath = this.GetProjectRelativePath(codeFile.Location, msBuildProject);
            var fullItemPath = this.fileSystem.Path.GetFullPath(this.fileSystem.Path.Combine(this.fileSystem.Path.GetDirectoryName(msBuildProject.FullPath), relativeItemPath));
            var existingItems = msBuildProject.FindProjectItems(fullItemPath).ToList();

            if (existingItems.All(i => i.ItemType != this.itemType))
            {
                msBuildProject.AddItem(this.itemType, relativeItemPath);
            }

            this.UpdateFileContent(fullItemPath, codeFile.SourceCodeText);
        }

        public virtual bool CodeFileSupported(CodeFile codeFile)
        {
            return true;
        }

        public virtual bool ItemSupported(MsBuildItem item)
        {
            return item.ItemType != null && item.ItemType != this.itemType;
        }

        public virtual void Remove(CodeFile codeFile, MsBuildProject msBuildProject)
        {
            var relativeItemPath = this.GetProjectRelativePath(codeFile.Location, msBuildProject);
            var existingItems = msBuildProject.FindProjectItems(relativeItemPath).ToList();

            foreach (var item in existingItems)
            {
                msBuildProject.RemoveItem(item);
            }
        }

        protected string GetProjectRelativePath(CodeFileLocation location, MsBuildProject msBuildProject)
        {
            if (location is WorkspaceCodeFileLocation)
            {
                var wLocation = location as WorkspaceCodeFileLocation;

                if (!string.IsNullOrEmpty(wLocation.DocumentPath?.DocumentName))
                {
                    return this.fileSystem.Path.Combine(string.Join(this.fileSystem.Path.DirectorySeparatorChar.ToString(), wLocation.DocumentPath.ProjectFolders), wLocation.DocumentPath.DocumentName);
                }
            }
            else
            {
                return PathMaskHelper.GetRelativePath(msBuildProject.FullPath, location.FilePath, this.fileSystem);
            }

            return null;
        }

        protected void UpdateFileContent(string fullPath, string contents)
        {
            this.fileSystem.File.WriteAllText(fullPath, contents);
        }
    }
}
