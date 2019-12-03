using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Glsl
{
    public class CodeFileStorageHandlerGlsl : ICodeFileStorageHandler
    {
        private readonly IFileSystem fileSystem;
        private List<CodeFile> codeFiles = new List<CodeFile>();

        public CodeFileStorageHandlerGlsl(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void Add(CodeFile codeFile)
        {
            if (!this.codeFiles.Contains(codeFile))
            {
                this.codeFiles.Add(codeFile);
            }
        }

        public void Remove(CodeFile codeFile)
        {
            if (this.codeFiles.Contains(codeFile))
            {
                this.codeFiles.Remove(codeFile);
            }
        }

        public void ResolveCodeFile(CodeFile codeFile, bool resolveSourceCodeText = true, bool resolveLocation = true)
        {
            if (this.fileSystem.File.Exists(codeFile.Location.FilePath))
            {
                codeFile.SourceCodeText = this.fileSystem.File.ReadAllText(codeFile.Location.FilePath);
            }            
        }

        public string ResolveCodeFileName(CodeFileLocation location)
        {
            if (this.fileSystem.File.Exists(location.FilePath))
            {
                return this.fileSystem.Path.GetFileName(location.FilePath);
            }

            return null;
        }
    }
}
