using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface IFileSelector
    {
        IEnumerable<string> GetFiles(IFileSystem fileSystem, string basePath);
    }
}
