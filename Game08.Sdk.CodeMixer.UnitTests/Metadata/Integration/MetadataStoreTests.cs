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

        [Test]
        public void GeneratedBy_Indirect3()
        {
            var subject = new MetadataStore();

            var path1 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
            });

            var path2 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
                new PathLevel("Level2", "Type2"),
            });

            var path3 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
                new PathLevel("Level2", "Type2"),
                new PathLevel("Level3", "Type3")                
            });

            var path4 = new SemanticPath("Lang1", new PathLevel[]
            {
                new PathLevel("Level1", "Type1"),
                new PathLevel("Level2", "Type2"),
                new PathLevel("Level3", "Type3"),
                new PathLevel("Level4", "Type4"),
            });

            var symbol01 = new SemanticSymbol(0, path1);
            var symbol02 = new SemanticSymbol(0, path2);

            var symbol11 = new SemanticSymbol(1, path1);
            var symbol12 = new SemanticSymbol(1, path2);
            var symbol13 = new SemanticSymbol(1, path3);

            var symbol21 = new SemanticSymbol(2, path1);
            var symbol22 = new SemanticSymbol(2, path2);
            var symbol23 = new SemanticSymbol(2, path3);
            var symbol24 = new SemanticSymbol(2, path4);

            subject.Write(new Generated(symbol02, "stage1", "plugin1", null, new HashSet<string>()));
            subject.Write(new SourcingFrom(symbol01, symbol11, "stage2", "plugin2", null, new HashSet<string>()));
            subject.Write(new Generated(symbol13, "stage3", "plugin3", null, new HashSet<string>()));

            subject.Refine(symbol12);
            subject.Refine(symbol13);

            subject.Write(new SourcingFrom(symbol11, symbol21, "stage4", "plugin4", null, new HashSet<string>()));

            subject.Refine(symbol22);
            subject.Refine(symbol23);
            subject.Refine(symbol24);            

            var result1 = subject.SymbolIsGeneratedBy(symbol23, new ActivityFrame(null, "plugin3"));
            var result2 = subject.SymbolIsGeneratedBy(symbol24, new ActivityFrame(null, "plugin3"));
            var negResult3 = subject.SymbolIsGeneratedBy(symbol23, new ActivityFrame(null, "plugin1"));

            result1.Should().BeTrue();
            result2.Should().BeTrue();
            negResult3.Should().BeFalse();
        }
    }
}
