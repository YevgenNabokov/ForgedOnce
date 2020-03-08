using ForgedOnce.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Plugins
{
    public class FileGroup<TCodeFile, TGroupItemDetails> : IFileGroup<TCodeFile, TGroupItemDetails> where TCodeFile : CodeFile where TGroupItemDetails : GroupItemDetails
    {
        public string Name { get; set; }

        public Dictionary<TCodeFile, TGroupItemDetails> Files { get; } = new Dictionary<TCodeFile, TGroupItemDetails>();
    }
}
