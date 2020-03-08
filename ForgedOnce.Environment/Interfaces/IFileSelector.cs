using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;

namespace ForgedOnce.Environment.Interfaces
{
    public interface IFileSelector
    {
        IEnumerable<string> GetFiles(IFileSystem fileSystem, string basePath);
    }
}
