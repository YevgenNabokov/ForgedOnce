using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core
{
    public class CodeStream : ICodeStream
    {
        protected List<CodeFile> codeFiles = new List<CodeFile>();

        private ICodeFileFactory codeFileFactory;

        private readonly ICodeFileLocationProvider codeFileLocationProvider;

        public CodeStream(string language, string name, ICodeFileFactory codeFileFactory, ICodeFileLocationProvider codeFileLocationProvider = null)
        {
            this.Language = language;
            this.Name = name;
            this.codeFileFactory = codeFileFactory;
            this.codeFileLocationProvider = codeFileLocationProvider;
        }

        public CodeStream(string language, string name, IEnumerable<CodeFile> codeFiles)
        {
            this.Language = language;
            this.Name = name;
            this.codeFiles = new List<CodeFile>(codeFiles);
        }

        public IEnumerable<CodeFile> Files
        {
            get
            {
                return this.codeFiles;
            }
        }

        public string Language { get; protected set; }

        public string Name { get; protected set; }

        public bool IsReadonly
        {
            get
            {
                return this.codeFileFactory == null;
            }
        }

        public virtual CodeFile CreateCodeFile(string name)
        {
            if (this.IsReadonly)
            {
                throw new InvalidOperationException("Cannot create code file, this is readonly code stream.");
            }

            var result = this.codeFileFactory.CreateCodeFile(name);
            if (this.codeFileLocationProvider != null)
            {
                result.Location = this.codeFileLocationProvider.GetLocation(name);
            }

            this.codeFiles.Add(result);
            return result;
        }
    }
}
