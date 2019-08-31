using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProjects.SimplePlugin
{
    public class AddSerializableAttributeToClassPreprocessor : IPluginPreprocessor<AddSerializableAttributeToClassMetadata>
    {
        public AddSerializableAttributeToClassMetadata GenerateMetadata(CodeFile input, IMetadataReader metadataReader)
        {
            return new AddSerializableAttributeToClassMetadata();
        }
    }
}
