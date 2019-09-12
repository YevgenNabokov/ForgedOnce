using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.Launcher.MSBuild.Storage
{
    public class MsBuildProject
    {
        private readonly string MsBuildProjectNamePropertyName = "MSBuildProjectName";

        public MsBuildProject(Project project)
        {
            this.Project = project;
        }

        public Project Project
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                var nameProp = this.Project.GetProperty(this.MsBuildProjectNamePropertyName);
                if (nameProp == null)
                {
                    throw new InvalidOperationException($"Property {MsBuildProjectNamePropertyName} was not found in project.");
                }

                return nameProp.EvaluatedValue;
            }
        }
    }
}
