using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;
using System.Text.RegularExpressions;

namespace Game08.Sdk.CodeMixer.Environment
{
    public static class PathMaskHelper
    {
        public static bool PathMatchMask(string path, string mask)
        {
            var regex = FileMaskToRegex(mask);
            return regex.IsMatch(path);
        }

        public static bool PathMatchMask(string path, string mask, string basePath, IFileSystem fileSystem)
        {
            throw new NotImplementedException();
        }

        private static Regex FileMaskToRegex(string mask)
        {
            String convertedMask = "^" + Regex.Escape(mask).Replace("\\*", ".*").Replace("\\?", ".") + "$";
            return new Regex(convertedMask, RegexOptions.IgnoreCase);
        }
    }
}
