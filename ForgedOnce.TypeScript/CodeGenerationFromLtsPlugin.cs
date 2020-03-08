using ForgedOnce.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.TypeScript
{
    public abstract class CodeGenerationFromLtsPlugin<TSettings, TInputParameters> : CodeGenerationPlugin<TSettings, TInputParameters, CodeFileLtsModel>
    {
    }
}
