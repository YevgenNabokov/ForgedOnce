using ForgedOnce.Core;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Environment.Workspace;
using ForgedOnce.TypeScript.Helpers;
using ForgedOnce.TypeScript.Metadata;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.AstModel;
using ForgedOnce.TsLanguageServices.Host.Interfaces;
using System.Linq;
using ForgedOnce.TsLanguageServices.FullSyntaxTree.TransportModel;

namespace ForgedOnce.TypeScript
{
    public class CodeFileTs : CodeFile
    {
        private bool isDefinition;
        private StRoot model;
        private readonly ITsHost tsHost;

        public override string Language => Languages.TypeScript;

        public CodeFileTs(string id, string name, ITsHost tsHost)
            : base(id, name)
        {
            this.NodePathService = new TsNodePathService(this);
            this.Model = new StRoot();
            this.tsHost = tsHost;
        }

        public bool IsDefinition
        {
            get => isDefinition;
            set { this.EnsureFileIsEditable(); isDefinition = value; }
        }

        public StRoot Model
        {
            get => model;
            set { this.EnsureFileIsEditable(); model = value; }
        }

        internal void SetModelOverrideReadonly(StRoot model)
        {
            this.model = model;
        }

        public INodePathService<IStNode> NodePathService
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
                var statemenets = this.Model.statements.Select(s => (IStatement)s.GetTransportModelNode()).ToArray();

                return this.tsHost.Print(statemenets, ScriptKind.Unknown);
            }

            return null;
        }

        protected override void SourceCodeTextUpdated(string newSourceCode)
        {
            var statements = this.tsHost.Parse(newSourceCode, ScriptKind.Unknown);
            ModelConverter converter = new ModelConverter();
            var astStatements = statements.Select(s => (IStStatement)converter.ConvertFromNode(s)).ToArray();

            var root = new StRoot();

            foreach (var statement in astStatements)
            {
                root.statements.Add(statement);
            }

            this.Model = root;
        }
    }
}
