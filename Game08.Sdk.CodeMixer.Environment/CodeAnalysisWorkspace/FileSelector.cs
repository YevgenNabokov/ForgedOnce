using Game08.Sdk.CodeMixer.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.CodeAnalysisWorkspace
{
    public class FileSelector : IFileSelector
    {
        private readonly string[] filePaths;

        public FileSelector(string[] filePaths)
        {
            this.filePaths = filePaths;
        }

        public IEnumerable<string> GetFiles(IFileSystem fileSystem, string basePath)
        {
            throw new NotImplementedException();
        }
    }
}
