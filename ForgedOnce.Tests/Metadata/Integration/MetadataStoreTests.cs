using FluentAssertions;
using ForgedOnce.Core.Metadata;
using ForgedOnce.Core.Metadata.Changes;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Core.Metadata.Storage;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Tests.Metadata.Integration
{
    [TestFixture]
    public class MetadataStoreTests
    {
        [Test]
        public void GeneratedBy_ExactMatch()
        {
            var subject = new MetadataStore();

            var path = NodePath.FromString("Lang1:Level1/Level2/Level3/Level4");

            var pathSnapshot = new Mock<ISubTreeSnapshot>();
            pathSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(path, path) });

            var testTag = "TEST_TAG";
            subject.Write(new Generated(pathSnapshot.Object, 0, "stage1", "plugin1", null, new Dictionary<string, string>() { { testTag, string.Empty } }));

            subject.Commit();

            NodeRecord record;
            var result = subject.NodeIsGeneratedBy(path, new ActivityFrame(null, "plugin1"), out record);

            result.Should().BeTrue();
            record.Should().NotBeNull();
            record.Tags.Should().NotBeNull();
            record.Tags.Count.Should().Be(1);
            record.Tags.Should().ContainKey(testTag);
        }

        [Test]
        [TestCase("TEST1", "TEST2", "TEST1", "NONE", SearchCondition.And, false)]
        [TestCase("TEST1", "TEST2", "TEST1", "NONE", SearchCondition.Or, true)]
        [TestCase("TEST1", "TEST2", "TEST1", "TEST2", SearchCondition.And, true)]
        [TestCase("TEST1", "TEST2", "TEST1", null, SearchCondition.And, true)]
        public void GeneratedBy_TagSearch_ExactMatch(string tag1, string tag2, string searchedTag1, string searchedTag2, SearchCondition condition, bool expectedMatch)
        {
            var subject = new MetadataStore();

            var path = NodePath.FromString("Lang1:Level1/Level2/Level3/Level4");

            var pathSnapshot = new Mock<ISubTreeSnapshot>();
            pathSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(path, path) });

            var tags = new Dictionary<string, string>();

            if (tag1 != null)
            {
                tags.Add(tag1, string.Empty);
            }

            if (tag2 != null)
            {
                tags.Add(tag2, string.Empty);
            }

            subject.Write(new Generated(pathSnapshot.Object, 0, "stage1", "plugin1", null, tags));

            subject.Commit();

            var searchedTags = new List<string>();
            if (searchedTag1 != null)
            {
                searchedTags.Add(searchedTag1);
            }

            if (searchedTag2 != null)
            {
                searchedTags.Add(searchedTag2);
            }

            NodeRecord record;
            var result = subject.NodeIsGeneratedBy(path, new ActivityFrame(tagSelector: new TagSelector(searchedTags, condition)), out record);

            result.Should().Be(expectedMatch);
            if (expectedMatch)
            {
                record.Should().NotBeNull();
                record.PluginId.Should().Be("plugin1");
                record.Tags.Should().NotBeNull();
                record.Tags.Count.Should().Be(tags.Count);
                foreach (var key in tags.Keys)
                {
                    record.Tags.Should().ContainKey(key);
                }
            }
        }

        [Test]
        public void GeneratedBy_Indirect()
        {
            var subject = new MetadataStore();

            var path1 = NodePath.FromString("Lang1:Level1/Level2");

            var path1SubTreeSnapshot = new Mock<ISubTreeSnapshot>();
            path1SubTreeSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(path1, path1) });

            var path1NodeSnapshot = new Mock<ISingleNodeSnapshot>();
            path1NodeSnapshot.Setup(s => s.ResolveRoot()).Returns(new MetadataRoot(path1, path1));

            var path12 = NodePath.FromString("Lang1:Level12/Level2");

            var path2 = NodePath.FromString("Lang1:Level12/Level2/Level3/Level4");
            
            var path12SubTreeSnapshot = new Mock<ISubTreeSnapshot>();
            path12SubTreeSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(path12, path12) });

            var testTag = "TEST_TAG";
            subject.Write(new Generated(path1SubTreeSnapshot.Object, 0, "stage1", "plugin1", null, new Dictionary<string, string>() { { testTag, string.Empty } }));
            var testTag2 = "TEST_TAG2";
            subject.Write(new SourcingFrom(path1NodeSnapshot.Object, path12SubTreeSnapshot.Object, 1, "stage2", "plugin2", null, new Dictionary<string, string>() { { testTag2, string.Empty } }));

            subject.Commit();

            NodeRecord record;
            var result = subject.NodeIsGeneratedBy(path2, new ActivityFrame(null, "plugin1"), out record);

            result.Should().BeTrue();
            record.Should().NotBeNull();
            record.Tags.Should().NotBeNull();
            record.Tags.Count.Should().Be(1);
            record.Tags.Should().ContainKey(testTag);
        }

        [Test]
        public void GeneratedBy_Indirect2()
        {
            var subject = new MetadataStore();

            var pathLevel1 = NodePath.FromString("Lang1:Level1");

            var pathLevel1NodeSnapshot = new Mock<ISingleNodeSnapshot>();
            pathLevel1NodeSnapshot.Setup(s => s.ResolveRoot()).Returns(new MetadataRoot(pathLevel1, pathLevel1));

            var pathLevel1Level2 = NodePath.FromString("Lang1:Level1/Level2");

            var pathLevel12 = NodePath.FromString("Lang1:Level12");

            var pathLevel12Level2 = NodePath.FromString("Lang1:Level12/Level2");

            var pathLevel1Level2SubTreeSnapshot = new Mock<ISubTreeSnapshot>();
            pathLevel1Level2SubTreeSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(pathLevel1Level2, pathLevel1Level2) });

            var pathLevel12Level2Level3Level4 = NodePath.FromString("Lang1:Level12/Level2/Level3/Level4");

            var pathLevel12SubTreeSnapshot = new Mock<ISubTreeSnapshot>();
            pathLevel12SubTreeSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(pathLevel12, pathLevel12) });
            pathLevel12SubTreeSnapshot.Setup(s => s.ContainsNode(It.Is<NodePath>(p =>
            p.Equals(pathLevel12Level2)
            ))).Returns(true);

            ////var symbol2 = new SemanticSymbol(path12);

            var testTag = "TEST_TAG";
            subject.Write(new Generated(pathLevel1Level2SubTreeSnapshot.Object, 0, "stage1", "plugin1", null, new Dictionary<string, string>() { { testTag, string.Empty } }));
            var testTag2 = "TEST_TAG2";
            subject.Write(new SourcingFrom(pathLevel1NodeSnapshot.Object, pathLevel12SubTreeSnapshot.Object, 1, "stage2", "plugin2", null, new Dictionary<string, string>() { { testTag2, string.Empty } }));

            subject.Commit();
            ////subject.Refine(symbol2);

            NodeRecord record;
            var result = subject.NodeIsGeneratedBy(pathLevel12Level2Level3Level4, new ActivityFrame(null, "plugin1"), out record);

            result.Should().BeTrue();
            record.Should().NotBeNull();
            record.Tags.Should().NotBeNull();
            record.Tags.Count.Should().Be(1);
            record.Tags.Should().ContainKey(testTag);
        }

        [Test]
        public void GeneratedBy_IndirectNegative()
        {
            var subject = new MetadataStore();

            var path1 = NodePath.FromString("Lang1:Level1/Level2");

            var path12 = NodePath.FromString("Lang1:Level12/Level2");

            var path1SubTreeSnapshot = new Mock<ISubTreeSnapshot>();
            path1SubTreeSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(path1, path1) });

            var path1NodeSnapshot = new Mock<ISingleNodeSnapshot>();
            path1NodeSnapshot.Setup(s => s.ResolveRoot()).Returns(new MetadataRoot(path1, path1));

            var path2 = NodePath.FromString("Lang1:Level12/Level2/Level3/Level4");

            var path12SubTreeSnapshot = new Mock<ISubTreeSnapshot>();
            path12SubTreeSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(path12, path12) });
            
            var path2SubTreeSnapshot = new Mock<ISubTreeSnapshot>();
            path2SubTreeSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(path2, path2) });

            subject.Write(new Generated(path1SubTreeSnapshot.Object, 0, "stage1", "plugin1", null, new Dictionary<string, string>()));
            subject.Write(new SourcingFrom(path1NodeSnapshot.Object, path12SubTreeSnapshot.Object, 1, "stage2", "plugin2", null, new Dictionary<string, string>()));
            subject.Write(new Generated(path2SubTreeSnapshot.Object, 1, "stage3", "plugin3", null, new Dictionary<string, string>()));

            subject.Commit();

            NodeRecord record;
            var result = subject.NodeIsGeneratedBy(path2, new ActivityFrame(null, "plugin1"), out record);

            result.Should().BeFalse();
            record.Should().BeNull();
        }

        [Test]
        public void GeneratedBy_Indirect3()
        {
            var subject = new MetadataStore();

            var pathLevel1 = NodePath.FromString("Lang1:Level1");

            var pathLevel1Level2 = NodePath.FromString("Lang1:Level1/Level2");

            var pathLevel11 = NodePath.FromString("Lang1:Level11");

            var pathLevel11Level2 = NodePath.FromString("Lang1:Level11/Level2");

            var pathLevel11Level2Level3 = NodePath.FromString("Lang1:Level11/Level2/Level3");

            var pathLevel12 = NodePath.FromString("Lang1:Level12");

            var pathLevel12Level2 = NodePath.FromString("Lang1:Level12/Level2");

            var pathLevel12Level2Level3 = NodePath.FromString("Lang1:Level12/Level2/Level3");

            var pathLevel12Level2Level3Level4 = NodePath.FromString("Lang1:Level12/Level2/Level3/Level4");
            
            var pathLevel1NodeSnapshot = new Mock<ISingleNodeSnapshot>();
            pathLevel1NodeSnapshot.Setup(s => s.ResolveRoot()).Returns(new MetadataRoot(pathLevel1, pathLevel1));

            var pathLevel1Level2SubTreeSnapshot = new Mock<ISubTreeSnapshot>();
            pathLevel1Level2SubTreeSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(pathLevel1Level2, pathLevel1Level2) });

            var pathLevel11NodeSnapshot = new Mock<ISingleNodeSnapshot>();
            pathLevel11NodeSnapshot.Setup(s => s.ResolveRoot()).Returns(new MetadataRoot(pathLevel11, pathLevel11));
            var pathLevel11SubTreeSnapshot = new Mock<ISubTreeSnapshot>();
            pathLevel11SubTreeSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(pathLevel11, pathLevel11) });
            pathLevel11SubTreeSnapshot.Setup(s => s.ContainsNode(It.Is<NodePath>(p =>
            p.Equals(pathLevel11Level2)
            ))).Returns(true);

            var pathLevel11Level2Level3SubTreeSnapshot = new Mock<ISubTreeSnapshot>();
            pathLevel11Level2Level3SubTreeSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(pathLevel11Level2Level3, pathLevel11Level2Level3) });

            var pathLevel12SubTreeSnapshot = new Mock<ISubTreeSnapshot>();
            pathLevel12SubTreeSnapshot.Setup(s => s.ResolveRoots()).Returns(new[] { new MetadataRoot(pathLevel12, pathLevel12) });
            pathLevel12SubTreeSnapshot.Setup(s => s.ContainsNode(It.Is<NodePath>(p =>
            p.Equals(pathLevel12Level2)
            || p.Equals(pathLevel12Level2Level3)
            || p.Equals(pathLevel12Level2Level3Level4)
            ))).Returns(true);

            var testTag = "TEST_TAG";
            subject.Write(new Generated(pathLevel1Level2SubTreeSnapshot.Object, 0, "stage1", "plugin1", null, new Dictionary<string, string>() { { testTag, string.Empty } }));
            subject.Write(new SourcingFrom(pathLevel1NodeSnapshot.Object, pathLevel11SubTreeSnapshot.Object, 1, "stage2", "plugin2", null, new Dictionary<string, string>()));
            var testTag2 = "TEST_TAG2";
            subject.Write(new Generated(pathLevel11Level2Level3SubTreeSnapshot.Object, 1, "stage3", "plugin3", null, new Dictionary<string, string>() { { testTag2, string.Empty } }));

            subject.Write(new SourcingFrom(pathLevel11NodeSnapshot.Object, pathLevel12SubTreeSnapshot.Object, 2, "stage4", "plugin4", null, new Dictionary<string, string>()));

            subject.Commit();

            NodeRecord record1;
            var result1 = subject.NodeIsGeneratedBy(pathLevel12Level2Level3, new ActivityFrame(null, "plugin3"), out record1);
            NodeRecord record2;
            var result2 = subject.NodeIsGeneratedBy(pathLevel12Level2Level3Level4, new ActivityFrame(null, "plugin3"), out record2);
            NodeRecord record3;
            var negResult3 = subject.NodeIsGeneratedBy(pathLevel12Level2Level3, new ActivityFrame(null, "plugin1"), out record3);

            result1.Should().BeTrue();
            record1.Should().NotBeNull();
            record1.Tags.Should().NotBeNull();
            record1.Tags.Count.Should().Be(1);
            record1.Tags.Should().ContainKey(testTag2);

            result2.Should().BeTrue();
            record2.Should().NotBeNull();
            record2.Tags.Should().NotBeNull();
            record2.Tags.Count.Should().Be(1);
            record2.Tags.Should().ContainKey(testTag2);

            negResult3.Should().BeFalse();
            record3.Should().BeNull();
        }
    }
}
