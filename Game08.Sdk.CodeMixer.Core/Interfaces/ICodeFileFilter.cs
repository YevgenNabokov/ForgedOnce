using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface ICodeFileFilter
    {
        IEnumerable<CodeFile> Filter(IEnumerable<CodeFile> files);
    }
}
