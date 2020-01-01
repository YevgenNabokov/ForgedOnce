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

        private List<MsBuildItem> items = new List<MsBuildItem>();

        public MsBuildProject(Project project)
        {
            this.Project = project;

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
            foreach (var item in this.Project.AddItem(itemType, unevaluatedInclude))
            {
                var newItem = new MsBuildItem(item, this);
                this.items.Add(newItem);
                result.Add(newItem);                
            }

            return result;
        }

        public void RemoveItem(MsBuildItem item)
        {
            this.Project.RemoveItem(item.Item);
            this.items.Remove(item);
        }        

        public void Save()
        {
            this.Project.Save();
        }
    }
}
