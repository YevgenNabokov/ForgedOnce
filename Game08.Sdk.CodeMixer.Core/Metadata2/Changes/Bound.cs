﻿using Game08.Sdk.CodeMixer.Core.Metadata2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Metadata2.Changes
{
    public class Bound : RecordBase
    {
        public Bound(ISingleNodeScope item1, ISingleNodeScope item2, int batchindex, string stageName, string pluginId, object pluginMetadata, IDictionary<string, string> tags)
            :base(batchindex, stageName, pluginId, pluginMetadata, tags)
        {
            this.Scopes.Add(item1);
            this.Scopes.Add(item2);
            this.Item1 = item1;
            this.Item2 = item2;
        }

        public ISingleNodeScope Item1
        {
            get;

            private set;
        }

        public ISingleNodeScope Item2
        {
            get;

            private set;
        }
    }
}
