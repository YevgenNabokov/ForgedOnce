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

            var symbol = new SemanticSymbol(0, path);

            subject.Write(new Generated(symbol, "stage1", "plugin1", null, new HashSet<string>()));

            var result = subject.SymbolIsGeneratedBy(symbol, new ActivityFrame(null, "plugin1"));

            result.Should().BeTrue();
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

            var symbol1 = new SemanticSymbol(0, path1);

            var path2 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
                new PathLevel("Level2", "Type2"),
                new PathLevel("Level3", "Type3"),
                new PathLevel("Level4", "Type4"),
            });

            var symbol2 = new SemanticSymbol(1, path1);

            var symbol3 = new SemanticSymbol(1, path2);

            subject.Write(new Generated(symbol1, "stage1", "plugin1", null, new HashSet<string>()));
            subject.Write(new SourcingFrom(symbol1, symbol2, "stage2", "plugin2", null, new HashSet<string>()));

            var result = subject.SymbolIsGeneratedBy(symbol3, new ActivityFrame(null, "plugin1"));

            result.Should().BeTrue();
        }

        [Test]
        public void GeneratedBy_Indirect2()
        {
            var subject = new MetadataStore();

            var path0 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),                
            });

            var symbol01 = new SemanticSymbol(0, path0);

            var path1 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
                new PathLevel("Level2", "Type2"),
            });

            var symbol1 = new SemanticSymbol(0, path1);

            var path2 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
                new PathLevel("Level2", "Type2"),
                new PathLevel("Level3", "Type3"),
                new PathLevel("Level4", "Type4"),
            });

            var symbol02 = new SemanticSymbol(1, path0);

            var symbol2 = new SemanticSymbol(1, path1);

            var symbol3 = new SemanticSymbol(1, path2);

            subject.Write(new Generated(symbol1, "stage1", "plugin1", null, new HashSet<string>()));
            subject.Write(new SourcingFrom(symbol02, symbol01, "stage2", "plugin2", null, new HashSet<string>()));

            subject.Refine(symbol2);

            var result = subject.SymbolIsGeneratedBy(symbol3, new ActivityFrame(null, "plugin1"));

            result.Should().BeTrue();
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

            var symbol1 = new SemanticSymbol(0, path1);

            var path2 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
                new PathLevel("Level2", "Type2"),
                new PathLevel("Level3", "Type3"),
                new PathLevel("Level4", "Type4"),
            });

            var symbol2 = new SemanticSymbol(1, path1);

            var symbol3 = new SemanticSymbol(1, path2);

            subject.Write(new Generated(symbol1, "stage1", "plugin1", null, new HashSet<string>()));
            subject.Write(new SourcingFrom(symbol1, symbol2, "stage2", "plugin2", null, new HashSet<string>()));
            subject.Write(new Generated(symbol3, "stage3", "plugin3", null, new HashSet<string>()));

            var result = subject.SymbolIsGeneratedBy(symbol3, new ActivityFrame(null, "plugin1"));

            result.Should().BeFalse();
        }
    }
}
