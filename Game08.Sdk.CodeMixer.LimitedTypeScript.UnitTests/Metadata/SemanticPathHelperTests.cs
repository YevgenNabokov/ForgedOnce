using FluentAssertions;
using Game08.Sdk.CodeMixer.LimitedTypeScript.Metadata;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game08.Sdk.CodeMixer.LimitedTypeScript.UnitTests.Metadata
{
    [TestFixture]
    public class SemanticPathHelperTests
    {
        [Test]
        public void PathLevelSettings_AreConsistent()
        {
            foreach (var key in SemanticPathHelper.PathLevelIdentifierGetters.Keys)
            {
                SemanticPathHelper.PathLevelTypeNames.Keys.Should().Contain(key);
            }

            foreach (var key in SemanticPathHelper.PathLevelTypeNames.Keys)
            {
                SemanticPathHelper.PathLevelIdentifierGetters.Keys.Should().Contain(key);
            }
        }
    }
}
