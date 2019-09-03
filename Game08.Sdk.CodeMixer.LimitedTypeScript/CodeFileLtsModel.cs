using Game08.Sdk.CodeMixer.Core;
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
