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

            subject.Write(new Generated(symbol, 0, "stage1", "plugin1", null, new HashSet<string>()));

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

            subject.Write(new Generated(symbol1, 0, "stage1", "plugin1", null, new HashSet<string>()));
            subject.Write(new SourcingFrom(symbol1, symbol2, 1, "stage2", "plugin2", null, new HashSet<string>()));

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

            subject.Write(new Generated(symbol1, 0, "stage1", "plugin1", null, new HashSet<string>()));
            subject.Write(new SourcingFrom(symbol02, symbol01, 1, "stage2", "plugin2", null, new HashSet<string>()));

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

            subject.Write(new Generated(symbol1, 0, "stage1", "plugin1", null, new HashSet<string>()));
            subject.Write(new SourcingFrom(symbol1, symbol2, 1, "stage2", "plugin2", null, new HashSet<string>()));
            subject.Write(new Generated(symbol3, 1, "stage3", "plugin3", null, new HashSet<string>()));

            var result = subject.SymbolIsGeneratedBy(symbol3, new ActivityFrame(null, "plugin1"));

            result.Should().BeFalse();
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

            subject.Write(new Generated(symbol02, 0, "stage1", "plugin1", null, new HashSet<string>()));
            subject.Write(new SourcingFrom(symbol01, symbol11, 1, "stage2", "plugin2", null, new HashSet<string>()));
            subject.Write(new Generated(symbol13, 1, "stage3", "plugin3", null, new HashSet<string>()));

            subject.Refine(symbol12);
            subject.Refine(symbol13);

            subject.Write(new SourcingFrom(symbol11, symbol21, 2, "stage4", "plugin4", null, new HashSet<string>()));

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
