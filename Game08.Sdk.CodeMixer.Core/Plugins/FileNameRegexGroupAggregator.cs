using Game08.Sdk.CodeMixer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Game08.Sdk.CodeMixer.Core.Plugins
{
    public class FileNameRegexGroupAggregator<TFile> : IFileGroupAggregator<TFile> where TFile : CodeFile
    {
        private readonly GroupSetting[] groupSettings;
        private readonly string unmatchedGroupName;

        public FileNameRegexGroupAggregator(GroupSetting[] groupSettings, string unmatchedGroupName)
        {
            this.groupSettings = groupSettings;
            this.unmatchedGroupName = unmatchedGroupName;
        }

        public IEnumerable<IFileGroup<TFile, GroupItemDetails>> Aggregate(IEnumerable<TFile> input)
        {
            var unmatchedGroup = new FileGroup<TFile, GroupItemDetails>()
            {
                Name = this.unmatchedGroupName
            };

            Dictionary<string, FileGroup<TFile, GroupItemDetails>> groups = new Dictionary<string, FileGroup<TFile, GroupItemDetails>>();

            foreach (var file in input)
            {
                var matched = false;
                foreach (var groupSetting in this.groupSettings)
                {
                    var match = Regex.Match(file.Name, groupSetting.Regex);
                    if (match.Success)
                    {
                        this.AddOutputGroup(match.Groups[groupSetting.NameMatchGroupIndex].Value, groupSetting.Tag, file, groups);
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    unmatchedGroup.Files.Add(file, new GroupItemDetails());
                }
            }

            if (unmatchedGroup.Files.Count > 0)
            {
                groups.Add(unmatchedGroup.Name, unmatchedGroup);
            }

            return groups.Values;
        }

        private void AddOutputGroup(string name, string tag, TFile file, Dictionary<string, FileGroup<TFile, GroupItemDetails>> groups)
        {
            if (!groups.ContainsKey(name))
            {
                groups.Add(name, new FileGroup<TFile, GroupItemDetails>()
                {
                    Name = name
                });
            }

            groups[name].Files.Add(file, new GroupItemDetails(new[] { tag }));
        }

        public class GroupSetting
        {
            public GroupSetting(string regex, int nameMatchGroupIndex, string tag)
            {
                this.Regex = regex;
                this.NameMatchGroupIndex = nameMatchGroupIndex;
                this.Tag = tag;
            }

            public string Regex { get; private set; }

            public int NameMatchGroupIndex { get; private set; }

            public string Tag { get; private set; }
        }
    }
}
