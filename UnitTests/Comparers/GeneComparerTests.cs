using SlimCache.Comparers;
using UnitTests.Resources;
using Xunit;

namespace UnitTests.Comparers
{
    public class GeneComparerTests
    {
        [Fact]
        public void Equal_ExpectedResults()
        {
            var c = GeneComparer.DefaultInstance;

            Assert.True(c.Equals(null,       null));
            Assert.True(c.Equals(Genes.TP53, Genes.TP53));
            Assert.True(c.Equals(Genes.TP53, Genes.TP53b));

            Assert.False(c.Equals(null,       Genes.TP53));
            Assert.False(c.Equals(Genes.TP53, null));

            Assert.False(c.Equals(Genes.TP53, Genes.KRAS));
        }

        [Fact]
        public void GetHashCode_ExpectedResults()
        {
            var c = GeneComparer.DefaultInstance;

            Assert.Equal(c.GetHashCode(Genes.TP53), c.GetHashCode(Genes.TP53));
            Assert.Equal(c.GetHashCode(Genes.TP53), c.GetHashCode(Genes.TP53b));
            Assert.NotEqual(c.GetHashCode(Genes.TP53), c.GetHashCode(Genes.KRAS));
        }
    }
}