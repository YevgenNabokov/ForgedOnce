﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Metadata.Interfaces
{
    public interface ISingleNodeSnapshot : ISnapshot
    {
        MetadataRoot ResolveRoot();
    }
}
