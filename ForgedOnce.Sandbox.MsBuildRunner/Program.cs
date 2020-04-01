using ForgedOnce.Launcher.MSBuild;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace ForgedOnce.Sandbox.MsBuildRunner
{
    class Program
    {
        private static List<ISandboxProject> Projects = new List<ISandboxProject>()
        {
            new TestProject1(),
            new TestProject2_Ts(),
            new TestProject3_Glsl(),
            new TestProject4_Metadata()
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Select sandbox project:");
            for (var i = 0; i < Projects.Count; i++)
            {
                Console.WriteLine($"{i}: {Projects[i].Name}");
            }

            Console.WriteLine("Provide index:");
            var input = Console.ReadLine();
            int idx = -1;
            if (Int32.TryParse(input, out idx))
            {
                if (idx >= 0 && idx < Projects.Count)
                {
                    var proj = Projects[idx];
                    Console.WriteLine($"Executing :{proj.Name}");
                    proj.Run();
                    Console.WriteLine("Done.");
                    Console.ReadKey();
                }
            }
        }
    }
}
