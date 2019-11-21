using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Plugins
{
    public class FileGroup<TCodeFile, TGroupItemDetails> : IFileGroup<TCodeFile, TGroupItemDetails> where TCodeFile : CodeFile where TGroupItemDetails : GroupItemDetails
    {
        public string Name { get; set; }

        public Dictionary<TCodeFile, TGroupItemDetails> Files { get; } = new Dictionary<TCodeFile, TGroupItemDetails>();
    }
}
