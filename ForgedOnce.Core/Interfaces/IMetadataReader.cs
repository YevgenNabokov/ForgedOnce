using ForgedOnce.Core.Metadata;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Core.Metadata.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Interfaces
{
    public interface IMetadataReader
    {        
        bool NodeIsGeneratedBy(NodePath nodePath, ActivityFrame activityFrame, out NodeRecord recordMatch);
    }
}
