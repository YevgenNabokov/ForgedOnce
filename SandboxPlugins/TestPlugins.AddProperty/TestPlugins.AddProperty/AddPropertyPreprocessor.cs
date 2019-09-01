﻿using Game08.Sdk.CodeMixer.Core;
using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestPlugins.AddProperty
{
    public class AddPropertyPreprocessor : IPluginPreprocessor<AddPropertyMetadata>
    {
        public AddPropertyMetadata GenerateMetadata(CodeFile input, IMetadataReader metadataReader)
        {
            return new AddPropertyMetadata();
        }
    }
}