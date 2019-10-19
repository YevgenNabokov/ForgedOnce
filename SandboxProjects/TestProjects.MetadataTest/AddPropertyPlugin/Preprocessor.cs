using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddPropertyPlugin
{
    public class Preprocessor : IPluginPreprocessor<Metadata>
    {
        public Metadata GenerateMetadata(CodeFile input, IMetadataReader metadataReader)
        {
            return new Metadata();
        }
    }
}
