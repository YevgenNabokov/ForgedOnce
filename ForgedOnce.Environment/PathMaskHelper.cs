using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ForgedOnce.Environment
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
            var parts = relativePathMask.Split(fileSystem.Path.DirectorySeparatorChar);
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

            return fileSystem.Path.Combine(startDirectory, string.Join(fileSystem.Path.DirectorySeparatorChar.ToString(), parts.Skip(levelUpParts)));
        }

        public static string GetRelativePath(string absoluteRootPath, string absolutePath, IFileSystem fileSystem)
        {
            var rootDirPath = fileSystem.Path.GetDirectoryName(absoluteRootPath);

            var commonRoot = GetCommonRootPath(rootDirPath, absolutePath, fileSystem);

            var levelUps = fileSystem.Path.Combine(rootDirPath.Substring(commonRoot.Length)
                .Split(fileSystem.Path.DirectorySeparatorChar)
                .Where(p => !string.IsNullOrEmpty(p))
                .Select(p => "..").ToArray());

            return fileSystem.Path.Combine(levelUps, absolutePath.Substring(commonRoot.Length));
        }

        public static string GetCommonRootPath(string path1, string path2, IFileSystem fileSystem)
        {
            string result = string.Empty;
            var parts1 = fileSystem.Path.GetDirectoryName(path1).Split(fileSystem.Path.DirectorySeparatorChar);
            var parts2 = fileSystem.Path.GetDirectoryName(path2).Split(fileSystem.Path.DirectorySeparatorChar);
            var b1 = new StringBuilder();
            var b2 = new StringBuilder();

            for (var i = 0; i < parts1.Length; i++)
            {
                if(i > 0)
                {
                    b1.Append(fileSystem.Path.DirectorySeparatorChar);
                }

                b1.Append(parts1[i]);

                if (parts2.Length > i)
                {
                    if (i > 0)
                    {
                        b2.Append(fileSystem.Path.DirectorySeparatorChar);
                    }

                    b2.Append(parts2[i]);
                }
                else
                {
                    return result;
                }

                if (b1.ToString() == b2.ToString())
                {
                    result = b1.ToString();
                }
                else
                {
                    return result;
                }
            }

            return result;
        }

        private static Regex FileMaskToRegex(string mask)
        {
            String convertedMask = "^" + Regex.Escape(mask).Replace("\\*", ".*").Replace("\\?", ".") + "$";
            return new Regex(convertedMask, RegexOptions.IgnoreCase);
        }
    }
}
