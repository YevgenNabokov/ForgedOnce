﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Core.Interfaces
{
    public interface ICodeFileEnvironmentHandler
    {
        bool SupportsCodeLanguage(string language);

        void Add(CodeFile codeFile);

        void Remove(CodeFile codeFile);
    }
}
