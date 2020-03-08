using FluentAssertions;
using ForgedOnce.Core.Metadata;
using ForgedOnce.Core.Metadata.Storage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForgedOnce.Tests.Metadata.Storage
{
    [TestFixture]
    public class MetadataIndexTests
    {
        [Test]
        public void SimpleNodeAllocation()
        {
            var subject = new MetadataIndex();

            var path = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null),
            });

            var result = subject.AllocateNode(path);

            result.Should().NotBeNull();
            result.Parent.Should().BeNull();
            result.Children.Count.Should().Be(0);
            result.RootIndex.Should().Be(subject);
            result.PathLevels.Count.Should().Be(2);
            result.PathLevels[0].Should().Be(path.Levels[0]);
            result.PathLevels[1].Should().Be(path.Levels[1]);
        }

        [Test]
        public void RepeatedNodeAllocationWithBranch()
        {
            var subject = new MetadataIndex();

            var path = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null),
            });

            var result1 = subject.AllocateNode(path);

            var path2 = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level3", null),
            });

            var result = subject.AllocateNode(path2);

            result.PathLevels.Count.Should().Be(1);
            result.PathLevels[0].Should().Be(path2.Levels[1]);

            result.Parent.Should().NotBeNull();
            var l1 = result.Parent;
            l1.Children.Count.Should().Be(2);
            l1.PathLevels.Count.Should().Be(1);
            l1.PathLevels[0].Should().Be(path.Levels[0]);

            l1.Children.ContainsKey(path.Levels[1]).Should().BeTrue();
            l1.Children[path.Levels[1]].PathLevels.Count.Should().Be(1);
            l1.Children[path.Levels[1]].PathLevels[0].Should().Be(path.Levels[1]);
        }

        [Test]
        public void RepeatedNodeAllocationWithSplit()
        {
            var subject = new MetadataIndex();

            var path = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null),
                new NodePathLevel("Level3", null)
            });

            var result1 = subject.AllocateNode(path);

            var path2 = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null)
            });

            var result = subject.AllocateNode(path2);

            result.PathLevels.Count.Should().Be(2);
            result.PathLevels[0].Should().Be(path.Levels[0]);
            result.PathLevels[1].Should().Be(path.Levels[1]);
            result.Parent.Should().BeNull();
            result.Children.Count.Should().Be(1);
            result.Children[path.Levels[2]].PathLevels.Count.Should().Be(1);
            result.Children[path.Levels[2]].PathLevels[0].Should().Be(path.Levels[2]);
            result.Children[path.Levels[2]].Children.Count.Should().Be(0);
        }

        [Test]
        public void RepeatedNodeAllocationWithExtension()
        {
            var subject = new MetadataIndex();

            var path = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null)
            });

            var result1 = subject.AllocateNode(path);

            var path2 = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null),
                new NodePathLevel("Level3", null)
            });

            var result = subject.AllocateNode(path2);

            result.PathLevels.Count.Should().Be(1);
            result.PathLevels[0].Should().Be(path2.Levels[2]);
            result.Parent.Should().NotBeNull();
            result.Parent.PathLevels.Count.Should().Be(2);
            result.Parent.PathLevels[0].Should().Be(path.Levels[0]);
            result.Parent.PathLevels[1].Should().Be(path.Levels[1]);
        }

        [Test]
        public void RepeatedNodeAllocationWithExtensionOverTwoNodes()
        {
            var subject = new MetadataIndex();

            var path = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null)
            });

            var result1 = subject.AllocateNode(path);

            var path2 = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null),
                new NodePathLevel("Level3", null),
                new NodePathLevel("Level4", null)

            });

            var result2 = subject.AllocateNode(path2);

            var path3 = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null),
                new NodePathLevel("Level3", null),
                new NodePathLevel("Level4", null),
                new NodePathLevel("Level5", null),
                new NodePathLevel("Level6", null)
            });

            var result = subject.AllocateNode(path3);

            result.PathLevels.Count.Should().Be(2);
            result.PathLevels[0].Should().Be(path3.Levels[4]);
            result.PathLevels[1].Should().Be(path3.Levels[5]);
            result.Parent.Should().NotBeNull();
            result.Parent.PathLevels.Count.Should().Be(2);
            result.Parent.PathLevels[0].Should().Be(path3.Levels[2]);
            result.Parent.PathLevels[1].Should().Be(path3.Levels[3]);
            result.Parent.Parent.Should().NotBeNull();
            result.Parent.Parent.PathLevels.Count.Should().Be(2);
            result.Parent.Parent.PathLevels[0].Should().Be(path3.Levels[0]);
            result.Parent.Parent.PathLevels[1].Should().Be(path3.Levels[1]);
        }

        [Test]
        public void GetExactNode()
        {
            var subject = new MetadataIndex();

            var path = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null)
            });

            var result1 = subject.AllocateNode(path);

            var result = subject.GetExactNode(path);

            result.Should().Be(result1);
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void GetExactNodeParent(bool orParent, bool resultShouldBeNull)
        {
            var subject = new MetadataIndex();

            var path = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null),                
            });

            var result1 = subject.AllocateNode(path);

            var path2 = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null),
                new NodePathLevel("Level3", null),
                new NodePathLevel("Level4", null),
            });

            var result2 = subject.AllocateNode(path2);

            var path3 = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null),
                new NodePathLevel("Level3", null),                
            });

            var result = subject.GetExactNode(path3, orParent);

            if (!resultShouldBeNull)
            {
                result.Should().Be(result1);
            }
            else
            {
                result.Should().BeNull();
            }
        }

        [Test]
        public void GetExactNodeSecondNode()
        {
            var subject = new MetadataIndex();

            var path = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null),
            });

            var result1 = subject.AllocateNode(path);

            var path2 = new NodePath("Lang1", new NodePathLevel[]
            {
                new NodePathLevel("Level1", null),
                new NodePathLevel("Level2", null),
                new NodePathLevel("Level3", null),
                new NodePathLevel("Level4", null),
            });

            var result2 = subject.AllocateNode(path2);            

            var result = subject.GetExactNode(path2, true);

            result.Should().Be(result2);
        }
    }
}
