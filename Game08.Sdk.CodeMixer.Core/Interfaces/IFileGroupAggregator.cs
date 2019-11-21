using Game08.Sdk.CodeMixer.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface IFileGroupAggregator<TCodeFile> where TCodeFile : CodeFile
    {
        IEnumerable<IFileGroup<TCodeFile, GroupItemDetails>> Aggregate(IEnumerable<TCodeFile> input);
    }
}
