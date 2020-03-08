using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Core.Pipeline
{
    public class Batch
    {
        public int Index;

        public string Name;

        public List<StageContainer> Stages = new List<StageContainer>();

        public List<string> PersistInputCodeStreams;
    }
}
