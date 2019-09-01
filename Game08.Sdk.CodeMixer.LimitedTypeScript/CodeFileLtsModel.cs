using Game08.Sdk.CodeMixer.Core;
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

        public CodeFileLtsModel(string id, string name)
            : base(id, name)
        {
        }

        public bool IsDefinition;

        public FileRoot Model;

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
