using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface ICodeFileFactory
    {
        CodeFile CreateCodeFile(string name);
    }
}
