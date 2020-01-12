﻿using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Pipeline
{
    public class StageContainer
    {
        public ICodeFileSelector InputSelector;

        public Stage Stage;

        public ICodeFileSelector FinalOutputSelector;

        public Dictionary<string, ICodeFileDestination> CodeFileDestinations;

        public Dictionary<string, string> OutputCodeStreamRenames;

        public bool CleanDestinations;
    }
}
