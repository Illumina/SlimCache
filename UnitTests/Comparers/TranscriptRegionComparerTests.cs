using SlimCache.Comparers;
using UnitTests.Resources;
using Xunit;

namespace UnitTests.Comparers
{
    public class TranscriptRegionComparerTests
    {
        [Fact]
        public void Equal_ExpectedResults()
        {
            var c = TranscriptRegionComparer.DefaultInstance;

            Assert.True(c.Equals(null,                          null));
            Assert.True(c.Equals(TranscriptRegions.NM_000546_0, TranscriptRegions.NM_000546_0));
            Assert.True(c.Equals(TranscriptRegions.NM_000546_1, TranscriptRegions.NM_000546_1b));

            Assert.False(c.Equals(null,                          TranscriptRegions.NM_000546_0));
            Assert.False(c.Equals(TranscriptRegions.NM_000546_0, null));

            Assert.False(c.Equals(TranscriptRegions.NM_000546_0, TranscriptRegions.NM_000546_1));
        }

        [Fact]
        public void GetHashCode_ExpectedResults()
        {
            var c = TranscriptRegionComparer.DefaultInstance;

            Assert.Equal(c.GetHashCode(TranscriptRegions.NM_000546_0), c.GetHashCode(TranscriptRegions.NM_000546_0));
            Assert.Equal(c.GetHashCode(TranscriptRegions.NM_000546_1), c.GetHashCode(TranscriptRegions.NM_000546_1b));
            Assert.NotEqual(c.GetHashCode(TranscriptRegions.NM_000546_0), c.GetHashCode(TranscriptRegions.NM_000546_1));
        }
    }
}