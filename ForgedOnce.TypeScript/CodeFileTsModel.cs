using ForgedOnce.Core;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Environment.Workspace;
using ForgedOnce.TypeScript.Helpers;
using ForgedOnce.TypeScript.Metadata;
using ForgedOnce.TsLanguageServices.ModelBuilder.DefinitionTree;
using ForgedOnce.TsLanguageServices.ModelBuilder.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.TypeScript
{
    public class CodeFileTsModel : CodeFile
    {
        private bool isDefinition;
        private FileRoot model;

        public override string Language => Languages.LimitedTypeScript;

        public CodeFileTsModel(string id, string name, ILtsTypeRepository ltsTypeRepository)
            : base(id, name)
        {
            this.TypeRepository = ltsTypeRepository;
            this.NodePathService = new TsNodePathService(this);
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

        internal void SetModelOverrideReadonly(FileRoot model)
        {
            this.model = model;
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

        public override void MakeReadOnly()
        {
            base.MakeReadOnly();
            this.Model?.MakeReadonly();
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
