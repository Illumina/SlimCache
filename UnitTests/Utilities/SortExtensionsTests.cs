using System.Collections.Generic;
using Intervals;
using SlimCache.Comparers;
using SlimCache.Utilities;
using UnitTests.Resources;
using VariantAnnotation.Interface.AnnotatedPositions;
using Xunit;

namespace UnitTests.Utilities
{
    public class SortExtensionsTests
    {
        [Fact]
        public void Sort_IGene_ExpectedResults()
        {
            var comparer = GeneComparer.DefaultInstance;
            var genes = new HashSet<IGene>(comparer)
            {
                Genes.TP53,
                Genes.KRAS,
                Genes.TP53b
            };

            IGene[] observed = genes.Sort();

            Assert.Null(new HashSet<IGene>().Sort());
            Assert.Equal(2,          observed.Length);
            Assert.Equal(Genes.KRAS, observed[0], comparer);
            Assert.Equal(Genes.TP53, observed[1], comparer);
        }

        [Fact]
        public void Sort_ITranscriptRegion_ExpectedResults()
        {
            var comparer = TranscriptRegionComparer.DefaultInstance;
            var transcriptRegions = new HashSet<ITranscriptRegion>(comparer)
            {
                TranscriptRegions.NM_000546_1,
                TranscriptRegions.NM_000546_0,
                TranscriptRegions.NM_000546_1b
            };

            ITranscriptRegion[] observed = transcriptRegions.Sort();

            Assert.Null(new HashSet<ITranscriptRegion>().Sort());
            Assert.Equal(2,                             observed.Length);
            Assert.Equal(TranscriptRegions.NM_000546_0, observed[0], comparer);
            Assert.Equal(TranscriptRegions.NM_000546_1, observed[1], comparer);
        }

        [Fact]
        public void Sort_IInterval_ExpectedResults()
        {
            var comparer = IntervalComparer.DefaultInstance;
            var intervals = new HashSet<IInterval>(comparer)
            {
                TranscriptRegions.NM_000546_1,
                TranscriptRegions.NM_000546_0,
                TranscriptRegions.NM_000546_1b
            };

            IInterval[] observed = intervals.Sort();

            Assert.Null(new HashSet<IInterval>().Sort());
            Assert.Equal(2,                             observed.Length);
            Assert.Equal(TranscriptRegions.NM_000546_0, observed[0], comparer);
            Assert.Equal(TranscriptRegions.NM_000546_1, observed[1], comparer);
        }

        [Fact]
        public void Sort_string_ExpectedResults()
        {
            var strings = new HashSet<string>
            {
                "seven",
                "ten",
                "seven",
                "eight"
            };

            string[] observed = strings.Sort();

            Assert.Null(new HashSet<string>().Sort());
            Assert.Equal(3,       observed.Length);
            Assert.Equal("eight", observed[0]);
            Assert.Equal("seven", observed[1]);
        }
    }
}