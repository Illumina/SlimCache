using System.IO;
using SlimCache.Utilities;
using VariantAnnotation.Interface.AnnotatedPositions;
using Xunit;

namespace UnitTests.Utilities
{
    public class SourceUtilitiesTests
    {
        [Fact]
        public void GetSource_ExpectedResults()
        {
            Assert.Equal(Source.Ensembl, SourceUtilities.GetSource("ensembl"));
            Assert.Equal(Source.Ensembl, SourceUtilities.GetSource("Ensembl"));
            Assert.Equal(Source.RefSeq, SourceUtilities.GetSource("refseq"));
            Assert.Equal(Source.RefSeq, SourceUtilities.GetSource("RefSeq"));
        }
        
        [Fact]
        public void GetSource_UnknownValue_ThrowException()
        {
            Assert.Throws<InvalidDataException>(delegate
            {
                SourceUtilities.GetSource("bob");
            });
        }
    }
}