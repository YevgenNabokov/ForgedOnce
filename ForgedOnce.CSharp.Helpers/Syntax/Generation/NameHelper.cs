using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.CSharp.Helpers.Syntax.Generation
{
    public static class NameHelper
    {
        public static string GetSafeVariableName(string name)
        {
            return Constants.CSharpKeywords.Contains(name)
                ? $"@{name}"
                : name;
        }

        public static string FirstCharToLower(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            return name.Substring(0, 1).ToLower() + (name.Length > 1 ? name.Substring(1) : string.Empty);
        }

        public static string FirstCharToUpper(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            return name.Substring(0, 1).ToUpper() + (name.Length > 1 ? name.Substring(1) : string.Empty);
        }
    }
}
