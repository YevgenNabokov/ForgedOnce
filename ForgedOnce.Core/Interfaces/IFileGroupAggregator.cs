using ForgedOnce.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface IFileGroupAggregator<TCodeFile> where TCodeFile : CodeFile
    {
        IEnumerable<IFileGroup<TCodeFile, GroupItemDetails>> Aggregate(IEnumerable<TCodeFile> input);
    }
}
