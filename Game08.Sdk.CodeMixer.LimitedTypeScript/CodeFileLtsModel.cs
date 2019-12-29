using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.CodeMixer.Environment.Workspace;
using Game08.Sdk.CodeMixer.LimitedTypeScript.Helpers;
using Game08.Sdk.CodeMixer.LimitedTypeScript.Metadata;
using Game08.Sdk.LTS.Builder.DefinitionTree;
using Game08.Sdk.LTS.Builder.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public class CodeFileLtsModel : CodeFile
    {
        private bool isDefinition;
        private FileRoot model;

        public override string Language => Languages.LimitedTypeScript;

        public CodeFileLtsModel(string id, string name, ILtsTypeRepository ltsTypeRepository)
            : base(id, name)
        {
            this.TypeRepository = ltsTypeRepository;
            this.NodePathService = new LtsNodePathService(this);
            this.Model = new FileRoot();
        }

        public bool IsDefinition
        {
            get => isDefinition;
            set { this.EnsureFileIsEditable(); isDefinition = value; }
        }

        public FileRoot Model
        {
            get => model;
            set { this.EnsureFileIsEditable(); model = value; }
        }

        public INodePathService<Node> NodePathService
        {
            get;
            private set;
        }

        public ILtsTypeRepository TypeRepository
        {
            get;
            private set;
        }

        public string GetPath()
        {
            if (this.Location != null)
            {
                if (this.Location is WorkspaceCodeFileLocation)
                {
                    var wLocation = this.Location as WorkspaceCodeFileLocation;
                    if (wLocation.DocumentPath != null)
                    {
                        return string.Format("{0}\\{1}\\{2}", wLocation.DocumentPath.ProjectName, string.Join("\\", wLocation.DocumentPath.ProjectFolders), this.Name);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.Location.FilePath))
                    {
                        return this.Location.FilePath;
                    }
                }
            }

            return this.Name;
        }

        protected override string GetSourceCodeText()
        {
            if (this.Model != null)
            {
                return JsonConvert.SerializeObject(this.Model);
            }

            return null;
        }

        protected override void SourceCodeTextUpdated(string newSourceCode)
        {
            this.Model = JsonConvert.DeserializeObject<FileRoot>(newSourceCode);
        }
    }
}
