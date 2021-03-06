﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Metadata
{
    public class MetadataRoot
    {
        public MetadataRoot(NodePath originalPath, NodePath currentPath)
        {
            this.OriginalPath = originalPath;
            this.CurrentPath = currentPath;
        }

        public NodePath OriginalPath { get; private set; }

        public NodePath CurrentPath { get; private set; }
    }
}
