using System.Collections.Generic;
using Intervals;
using SlimCache;
using UnitTests.Resources;
using VariantAnnotation.Caches;
using VariantAnnotation.Interface.AnnotatedPositions;
using Xunit;

namespace UnitTests
{
    public sealed class CacheBuilderTests
    {
        [Fact]
        public void GetCacheData_ExpectedResults()
        {
            var builder           = new CacheBuilder(CacheHeaderUtilities.CreateHeader(Source.BothRefSeqAndEnsembl), RegulatoryRegions.Chr17);
            var transcriptBuilder = new IntervalArrayBuilder();

            builder.Add(Transcripts.NM_000546);
            builder.Add(Transcripts.NM_002524);
            builder.Add(Transcripts.NM_033360);
            builder.Add(Transcripts.ENST00000610292);

            transcriptBuilder.Add(Transcripts.NM_000546);
            transcriptBuilder.Add(Transcripts.ENST00000610292);
            builder.Add(Chromosomes.Chr17.Index, transcriptBuilder.GetIntervalArray());

            TranscriptCacheData observed = builder.GetCacheData();

            Assert.Equal(3, observed.Genes.Length);
            Assert.Equal(8, observed.TranscriptRegions.Length);
            Assert.Equal(3, observed.Mirnas.Length);
            Assert.Equal(4, observed.PeptideSeqs.Length);
            
            Assert.Equal(Chromosomes.NumRefSeqs, observed.TranscriptIntervalArrays.Length);
            Assert.Equal(2,   observed.TranscriptIntervalArrays[Chromosomes.Chr17.Index].Array.Length);
            
            Assert.Equal(Chromosomes.NumRefSeqs, observed.RegulatoryRegionIntervalArrays.Length);
            Assert.Equal(3,   observed.RegulatoryRegionIntervalArrays[Chromosomes.Chr17.Index].Array.Length);
        }

        [Fact]
        public void Add_Generic_ExpectedResults()
        {
            var intervals    = new HashSet<IInterval>();
            var newIntervals = new IInterval[] {Genes.TP53, Genes.KRAS};

            CacheBuilder.Add(intervals, newIntervals);
            Assert.Equal(2, intervals.Count);

            CacheBuilder.Add(intervals, null);
            Assert.Equal(2, intervals.Count);
        }

        [Fact]
        public void Add_String_ExpectedResults()
        {
            var strings = new HashSet<string>();

            CacheBuilder.Add(strings, "temp");
            Assert.Single(strings);

            CacheBuilder.Add(strings, "");
            Assert.Single(strings);

            CacheBuilder.Add(strings, (string) null);
            Assert.Single(strings);
        }
    }
}