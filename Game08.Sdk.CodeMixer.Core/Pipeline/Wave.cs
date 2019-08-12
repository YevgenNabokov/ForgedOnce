using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class Wave
    {
        public IEnumerable<StageContainer> Stages;

        public IEnumerable<ICodeStream> Inputs;

        public Wave(IEnumerable<StageContainer> stages, IEnumerable<ICodeStream> inputs)
        {
            this.Stages = stages;
            this.Inputs = inputs;
        }
    }
}
