﻿using ForgedOnce.Core.Metadata.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Metadata.Changes
{
    public class SourcingFrom : RecordBase
    {
        public SourcingFrom(ISingleNodeSnapshot from, ISubTreeSnapshot subject, int batchindex, string stageName, string pluginId, object pluginMetadata, IDictionary<string, string> tags)
            : base(batchindex, stageName, pluginId, pluginMetadata, tags)
        {
            this.Scopes.Add(from);
            this.Scopes.Add(subject);
            this.From = from;
            this.Subject = subject;
        }

        public ISingleNodeSnapshot From { get; private set; }

        public ISubTreeSnapshot Subject { get; private set; }
    }
}
