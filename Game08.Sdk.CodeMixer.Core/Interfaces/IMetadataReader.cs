using Game08.Sdk.CodeMixer.Core.Metadata;
using Game08.Sdk.CodeMixer.Core.Metadata.Interfaces;
using Game08.Sdk.CodeMixer.Core.Metadata.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface IMetadataReader
    {        
        bool NodeIsGeneratedBy(NodePath nodePath, ActivityFrame activityFrame, out NodeRecord recordMatch);
    }
}
