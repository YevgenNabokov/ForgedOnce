using FluentAssertions;
using Game08.Sdk.CodeMixer.Core.Metadata;
using Game08.Sdk.CodeMixer.Core.Metadata.Changes;
using Game08.Sdk.CodeMixer.Core.Metadata.Storage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.UnitTests.Metadata.Integration
{
    [TestFixture]
    public class MetadataStoreTests
    {
        [Test]
        public void GeneratedBy_ExactMatch()
        {
            var subject = new MetadataStore();

            var path = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
                new PathLevel("Level2", "Type2"),
                new PathLevel("Level3", "Type3"),
                new PathLevel("Level4", "Type4"),
            });

            var symbol = new SemanticSymbol(path);

            var testTag = "TEST_TAG";
            subject.Write(new Generated(symbol, 0, "stage1", "plugin1", null, new Dictionary<string, string>() { { testTag, string.Empty } }));

            NodeRecord record;
            var result = subject.SymbolIsGeneratedBy(symbol, new ActivityFrame(null, "plugin1"), out record);

            result.Should().BeTrue();
            record.Should().NotBeNull();
            record.Tags.Should().NotBeNull();
            record.Tags.Count.Should().Be(1);
            record.Tags.Should().ContainKey(testTag);
        }

        [Test]
        public void GeneratedBy_Indirect()
        {
            var subject = new MetadataStore();

            var path1 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
                new PathLevel("Level2", "Type2"),                
            });

            var symbol1 = new SemanticSymbol(path1);

            var path12 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level12", "Type1"),
                new PathLevel("Level2", "Type2"),
            });

            var path2 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level12", "Type1"),
                new PathLevel("Level2", "Type2"),
                new PathLevel("Level3", "Type3"),
                new PathLevel("Level4", "Type4"),
            });

            var symbol2 = new SemanticSymbol(path12);

            var symbol3 = new SemanticSymbol(path2);

            var testTag = "TEST_TAG";
            subject.Write(new Generated(symbol1, 0, "stage1", "plugin1", null, new Dictionary<string, string>() { { testTag, string.Empty } }));
            var testTag2 = "TEST_TAG2";
            subject.Write(new SourcingFrom(symbol1, symbol2, 1, "stage2", "plugin2", null, new Dictionary<string, string>() { { testTag2, string.Empty } }));

            NodeRecord record;
            var result = subject.SymbolIsGeneratedBy(symbol3, new ActivityFrame(null, "plugin1"), out record);

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

            var path0 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
            });

            var symbol01 = new SemanticSymbol(path0);

            var path1 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
                new PathLevel("Level2", "Type2"),
            });

            var path02 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level12", "Type1"),
            });

            var path12 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level12", "Type1"),
                new PathLevel("Level2", "Type2"),
            });

            var symbol1 = new SemanticSymbol(path1);

            var path2 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level12", "Type1"),
                new PathLevel("Level2", "Type2"),
                new PathLevel("Level3", "Type3"),
                new PathLevel("Level4", "Type4"),
            });

            var symbol02 = new SemanticSymbol(path02);

            var symbol2 = new SemanticSymbol(path12);

            var symbol3 = new SemanticSymbol(path2);

            var testTag = "TEST_TAG";
            subject.Write(new Generated(symbol1, 0, "stage1", "plugin1", null, new Dictionary<string, string>() { { testTag, string.Empty } }));
            var testTag2 = "TEST_TAG2";
            subject.Write(new SourcingFrom(symbol02, symbol01, 1, "stage2", "plugin2", null, new Dictionary<string, string>() { { testTag2, string.Empty } }));

            subject.Refine(symbol2);

            NodeRecord record;
            var result = subject.SymbolIsGeneratedBy(symbol3, new ActivityFrame(null, "plugin1"), out record);

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

            var path1 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
                new PathLevel("Level2", "Type2"),
            });

            var path12 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level12", "Type1"),
                new PathLevel("Level2", "Type2"),
            });

            var symbol1 = new SemanticSymbol(path1);

            var path2 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level12", "Type1"),
                new PathLevel("Level2", "Type2"),
                new PathLevel("Level3", "Type3"),
                new PathLevel("Level4", "Type4"),
            });

            var symbol2 = new SemanticSymbol(path12);

            var symbol3 = new SemanticSymbol(path2);

            subject.Write(new Generated(symbol1, 0, "stage1", "plugin1", null, new Dictionary<string, string>()));
            subject.Write(new SourcingFrom(symbol1, symbol2, 1, "stage2", "plugin2", null, new Dictionary<string, string>()));
            subject.Write(new Generated(symbol3, 1, "stage3", "plugin3", null, new Dictionary<string, string>()));

            NodeRecord record;
            var result = subject.SymbolIsGeneratedBy(symbol3, new ActivityFrame(null, "plugin1"), out record);

            result.Should().BeFalse();
            record.Should().BeNull();
        }

        [Test]
        public void GeneratedBy_Indirect3()
        {
            var subject = new MetadataStore();

            var path01 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
            });

            var path02 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
                new PathLevel("Level2", "Type2"),
            });

            var path11 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level11", "Type1"),
            });

            var path12 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level11", "Type1"),
                new PathLevel("Level2", "Type2"),
            });

            var path13 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level11", "Type1"),
                new PathLevel("Level2", "Type2"),
                new PathLevel("Level3", "Type3")
            });

            var path21 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level12", "Type1"),
            });

            var path22 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level12", "Type1"),
                new PathLevel("Level2", "Type2"),
            });

            var path23 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level12", "Type1"),
                new PathLevel("Level2", "Type2"),
                new PathLevel("Level3", "Type3")
            });

            var path24 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level12", "Type1"),
                new PathLevel("Level2", "Type2"),
                new PathLevel("Level3", "Type3"),
                new PathLevel("Level4", "Type4"),
            });

            var symbol01 = new SemanticSymbol(path01);
            var symbol02 = new SemanticSymbol(path02);

            var symbol11 = new SemanticSymbol(path11);
            var symbol12 = new SemanticSymbol(path12);
            var symbol13 = new SemanticSymbol(path13);

            var symbol21 = new SemanticSymbol(path21);
            var symbol22 = new SemanticSymbol(path22);
            var symbol23 = new SemanticSymbol(path23);
            var symbol24 = new SemanticSymbol(path24);

            var testTag = "TEST_TAG";
            subject.Write(new Generated(symbol02, 0, "stage1", "plugin1", null, new Dictionary<string, string>() { { testTag, string.Empty } }));
            subject.Write(new SourcingFrom(symbol01, symbol11, 1, "stage2", "plugin2", null, new Dictionary<string, string>()));
            var testTag2 = "TEST_TAG2";
            subject.Write(new Generated(symbol13, 1, "stage3", "plugin3", null, new Dictionary<string, string>() { { testTag2, string.Empty } }));

            subject.Refine(symbol12);
            subject.Refine(symbol13);

            subject.Write(new SourcingFrom(symbol11, symbol21, 2, "stage4", "plugin4", null, new Dictionary<string, string>()));

            subject.Refine(symbol22);
            subject.Refine(symbol23);
            subject.Refine(symbol24);

            NodeRecord record1;
            var result1 = subject.SymbolIsGeneratedBy(symbol23, new ActivityFrame(null, "plugin3"), out record1);
            NodeRecord record2;
            var result2 = subject.SymbolIsGeneratedBy(symbol24, new ActivityFrame(null, "plugin3"), out record2);
            NodeRecord record3;
            var negResult3 = subject.SymbolIsGeneratedBy(symbol23, new ActivityFrame(null, "plugin1"), out record3);

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
