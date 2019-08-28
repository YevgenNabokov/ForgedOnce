﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Environment.Interfaces
{
    public interface ITypeLoader
    {
        Type LoadType(string typeName, string nugetPackageName = null, string nugetPackageVersion = null);
    }
}
