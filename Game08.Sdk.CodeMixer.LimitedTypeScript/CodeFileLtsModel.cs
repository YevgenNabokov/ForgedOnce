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
        public override string Language => Languages.LimitedTypeScript;

        public CodeFileLtsModel(string id, string name, ILtsTypeRepository ltsTypeRepository)
            : base(id, name)
        {
            this.TypeRepository = ltsTypeRepository;
            this.SemanticInfoProvider = new SemanticInfoProvider(this);
            this.Model = new FileRoot();
        }

        public bool IsDefinition;

        public FileRoot Model;

        public SemanticInfoProvider SemanticInfoProvider
        {
            get;
            private set;
        }

        public override ISemanticInfoResolver SemanticInfoResolver => this.SemanticInfoProvider;

        public ILtsTypeRepository TypeRepository
        {
            get;
            private set;
        }

        public override IEnumerable<ISemanticSymbol> SemanticSymbols
        {
            get
            {
                if (this.Model != null)
                {
                    var visitor = new SearchVisitor();
                    foreach (var node in visitor.FindNodes<Node>(this.Model, (n) => this.SemanticInfoProvider.CanGetSymbolFor(n)))
                    {
                        yield return this.SemanticInfoProvider.GetSymbolFor(node);
                    }
                }
            }
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
