using ForgedOnce.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
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
