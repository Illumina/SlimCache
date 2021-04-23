using System;
using System.Collections.Generic;
using System.IO;
using CacheUtils.TranscriptCache;
using Genome;
using Intervals;
using IO;
using SlimCache;
using UnitTests.Resources;
using UnitTests.TestComparers;
using VariantAnnotation.Caches;
using VariantAnnotation.Interface.AnnotatedPositions;
using VariantAnnotation.IO.Caches;
using Xunit;

namespace UnitTests
{
    public class CacheIoTests
    {
        [Fact]
        public void ReadWrite_ExpectedResults()
        {
            TranscriptCacheData cacheData    = GetCacheData();
            TranscriptCacheData newCacheData = WriteAndReadTranscripts(cacheData);

            Assert.Equal(cacheData, newCacheData, TranscriptCacheDataComparer.DefaultInstance);
        }

        public static TranscriptCacheData GetCacheData()
        {
            CacheHeader                        header                         = GetCacheHeader();
            IGene[]                            genes                          = GetGenes();
            ITranscriptRegion[]                transcriptRegions              = GetTranscriptRegions();
            IInterval[]                        mirnas                         = GetMirnas();
            string[]                           peptideSeqs                    = GetPeptideSeqs();
            IntervalArray<ITranscript>[]       transcriptIntervalArrays       = GetTranscripts();
            IntervalArray<IRegulatoryRegion>[] regulatoryRegionIntervalArrays = GetRegulatoryRegions();

            return new TranscriptCacheData(header, genes, transcriptRegions, mirnas, peptideSeqs,
                transcriptIntervalArrays, regulatoryRegionIntervalArrays);
        }

        private static IntervalArray<IRegulatoryRegion>[] GetRegulatoryRegions()
        {
            Interval<IRegulatoryRegion> regulatoryRegion  = RegulatoryRegions.ENSR00000089836.GetInterval();
            Interval<IRegulatoryRegion> regulatoryRegion2 = RegulatoryRegions.ENSR00000089837.GetInterval();
            Interval<IRegulatoryRegion> regulatoryRegion3 = RegulatoryRegions.ENSR00000282026.GetInterval();

            return new[]
            {
                null,
                null,
                new IntervalArray<IRegulatoryRegion>(new[] {regulatoryRegion, regulatoryRegion2, regulatoryRegion3})
            };
        }

        private static IntervalArray<ITranscript>[] GetTranscripts()
        {
            Interval<ITranscript> transcript  = Transcripts.NM_002524.GetInterval();
            Interval<ITranscript> transcript2 = Transcripts.NM_033360.GetInterval();
            Interval<ITranscript> transcript3 = Transcripts.NM_000546.GetInterval();
            Interval<ITranscript> transcript4 = Transcripts.ENST00000610292.GetInterval();

            return new[]
            {
                new IntervalArray<ITranscript>(new[] {transcript}),
                new IntervalArray<ITranscript>(new[] {transcript2}),
                new IntervalArray<ITranscript>(new[] {transcript3, transcript4}),
                null
            };
        }

        private static string[] GetPeptideSeqs() => new[]
        {
            Translations.NM_000546.PeptideSeq, Translations.NM_002524.PeptideSeq, Translations.NM_033360.PeptideSeq,
            Translations.ENST00000610292.PeptideSeq
        };

        private static IInterval[] GetMirnas() => MicroRNAs.NM_000546_fake;

        private static ITranscriptRegion[] GetTranscriptRegions()
        {
            var transcriptRegions = new List<ITranscriptRegion>();
            transcriptRegions.AddRange(TranscriptRegions.NM_000546);
            transcriptRegions.AddRange(TranscriptRegions.ENST00000610292);
            transcriptRegions.AddRange(TranscriptRegions.NM_002524);
            transcriptRegions.AddRange(TranscriptRegions.NM_033360);
            return transcriptRegions.ToArray();
        }

        private static IGene[] GetGenes() => new IGene[] {Genes.KRAS, Genes.NRAS, Genes.TP53};

        private static CacheHeader GetCacheHeader()
        {
            var header = new Header(CacheConstants.Identifier, CacheConstants.SchemaVersion, CacheConstants.DataVersion,
                Source.RefSeq, DateTime.Now.Ticks, GenomeAssembly.GRCh37);
            var customHeader = new TranscriptCacheCustomHeader(103, 100);
            return new CacheHeader(header, customHeader);
        }

        private static TranscriptCacheData WriteAndReadTranscripts(TranscriptCacheData cacheData)
        {
            using var ms = new MemoryStream();

            using (var writer = new TranscriptCacheWriter(ms, cacheData.Header, true))
            {
                writer.Write(cacheData);
            }

            ms.Position = 0;
            return CacheLoader.LoadTranscripts(ms, Chromosomes.RefIndexToChromosome);
        }
    }
}