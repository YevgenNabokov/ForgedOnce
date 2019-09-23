using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.Workspace;
using Game08.Sdk.LTS.Builder.Interfaces;
using Game08.Sdk.LTS.Model.DefinitionTree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript
{
    public class CodeFileLtsModel : CodeFile
    {        
        public override string Language => Languages.LimitedTypeScript;

        public CodeFileLtsModel(string id, string name, ILtsTypeRepository ltsTypeRepository)
            : base(id, name)
        {
            this.TypeRepository = ltsTypeRepository;
        }

        public bool IsDefinition;

        public FileRoot Model;

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
