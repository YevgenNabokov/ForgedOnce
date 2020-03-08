using ForgedOnce.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.CSharp
{
    public abstract class CodeGenerationFromCSharpPlugin<TSettings, TInputParameters> : CodeGenerationPlugin<TSettings, TInputParameters, CodeFileCSharp>
    {
    }
}
