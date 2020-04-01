using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Sandbox.MsBuildRunner
{
    public interface ISandboxProject
    {
        void Run();
        string Name { get; }
    }
}
