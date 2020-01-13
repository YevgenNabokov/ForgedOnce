﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface ICodeFileDestination
    {
        CodeFileLocation GetLocation(string fileName);

        void Clear();
    }
}