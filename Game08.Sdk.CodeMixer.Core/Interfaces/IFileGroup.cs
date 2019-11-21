using Game08.Sdk.CodeMixer.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface IFileGroup<TCodeFile, TGroupItemDetails> where TCodeFile : CodeFile where TGroupItemDetails : GroupItemDetails
    {
        string Name
        {
            get;
        }

        Dictionary<TCodeFile, TGroupItemDetails> Files { get; }
    }
}
