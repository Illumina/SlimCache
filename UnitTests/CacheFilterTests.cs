using Intervals;
using SlimCache;
using UnitTests.Resources;
using VariantAnnotation.Caches;
using VariantAnnotation.Interface.AnnotatedPositions;
using VariantAnnotation.IO.Caches;
using Xunit;

namespace UnitTests
{
    public class CacheFilterTests
    {
        private readonly IntervalArray<ITranscript>[] _transcriptIntervalArrays;

        public CacheFilterTests()
        {
            _transcriptIntervalArrays = new IntervalArray<ITranscript>[Chromosomes.NumRefSeqs];
            var transcriptBuilder = new IntervalArrayBuilder();
            transcriptBuilder.Add(Transcripts.NM_000546);
            transcriptBuilder.Add(Transcripts.ENST00000610292);
            _transcriptIntervalArrays[Chromosomes.Chr17.Index] = transcriptBuilder.GetIntervalArray();
        }
        
        [Fact]
        public void FilterTranscripts_JustRefSeq_ExpectedResults()
        {
            const Source source = Source.RefSeq;
            CacheHeader  header = CacheHeaderUtilities.CreateHeader(source);

            (TranscriptCacheData filteredCacheData, int numFilteredTranscripts) = CacheFilter.FilterTranscripts(header,
                _transcriptIntervalArrays, RegulatoryRegions.Chr17, source);

            Interval<ITranscript>[] chr17Transcripts =
                filteredCacheData.TranscriptIntervalArrays[Chromosomes.Chr17.Index].Array;

            Assert.Single(chr17Transcripts);
            Assert.Equal(1,      numFilteredTranscripts);
            Assert.Equal(source, chr17Transcripts[0].Value.Source);
            Assert.Single(filteredCacheData.Genes);
            Assert.Null(filteredCacheData.Mirnas);
            Assert.Single(filteredCacheData.PeptideSeqs);
            Assert.Equal(3, filteredCacheData.RegulatoryRegionIntervalArrays[Chromosomes.Chr17.Index].Array.Length);
        }

        [Fact]
        public void FilterTranscripts_JustEnsembl_ExpectedResults()
        {
            const Source source = Source.Ensembl;
            CacheHeader  header = CacheHeaderUtilities.CreateHeader(source);

            (TranscriptCacheData filteredCacheData, int numFilteredTranscripts) = CacheFilter.FilterTranscripts(header,
                _transcriptIntervalArrays, RegulatoryRegions.Chr17, source);

            Interval<ITranscript>[] chr17Transcripts =
                filteredCacheData.TranscriptIntervalArrays[Chromosomes.Chr17.Index].Array;

            Assert.Single(chr17Transcripts);
            Assert.Equal(1,      numFilteredTranscripts);
            Assert.Equal(source, chr17Transcripts[0].Value.Source);
            Assert.Single(filteredCacheData.Genes);
            Assert.Null(filteredCacheData.Mirnas);
            Assert.Single(filteredCacheData.PeptideSeqs);
            Assert.Equal(3, filteredCacheData.RegulatoryRegionIntervalArrays[Chromosomes.Chr17.Index].Array.Length);
        }
    }
}