using ForgedOnce.Core;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface IFileSelector
    {
        IEnumerable<CodeFileLocation> GetFiles(IFileSystem fileSystem, string basePath);
    }
}
