using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
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
            return PathMatchMask(path, GetAbsolutePathMask(mask, basePath, fileSystem));
        }

        public static string GetAbsolutePathMask(string relativePathMask, string basePath, IFileSystem fileSystem)
        {
            var startDirectory = basePath;
            var parts = relativePathMask.Split(fileSystem.Path.PathSeparator);
            int levelUpParts = 0;
            foreach (var part in parts)
            {
                if (part == "..")
                {
                    startDirectory = fileSystem.Directory.GetParent(startDirectory).FullName;
                    levelUpParts++;
                }
                else
                {
                    break;
                }
            }

            return fileSystem.Path.Combine(startDirectory, string.Join(fileSystem.Path.PathSeparator.ToString(), parts.Skip(levelUpParts)));
        }

        private static Regex FileMaskToRegex(string mask)
        {
            String convertedMask = "^" + Regex.Escape(mask).Replace("\\*", ".*").Replace("\\?", ".") + "$";
            return new Regex(convertedMask, RegexOptions.IgnoreCase);
        }
    }
}
