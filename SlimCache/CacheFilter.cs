using System;
using System.Collections.Generic;
using System.Linq;
using CacheUtils.TranscriptCache;
using Intervals;
using SlimCache.Comparers;
using VariantAnnotation.Caches;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace SlimCache
{
    public static class CacheFilter
    {
        public static TranscriptCacheData FilterTranscripts(this TranscriptCacheData cacheData,
                                                            Source                   desiredTranscriptSource)
        {
            Console.Write("- filtering transcripts... ");

            int numRefSeqs  = cacheData.TranscriptIntervalArrays.Length;
            int numFiltered = 0;

            var intervalComparer         = new IntervalComparer();
            var transcriptRegionComparer = new TranscriptRegionComparer();
            var geneComparer             = new GeneComparer();

            var geneSet             = new HashSet<IGene>(geneComparer);
            var transcriptRegionSet = new HashSet<ITranscriptRegion>(transcriptRegionComparer);
            var mirnaSet            = new HashSet<IInterval>(intervalComparer);
            var peptideSet          = new HashSet<string>();

            var desiredTranscriptIntervalArrays =
                new IntervalArray<ITranscript>[numRefSeqs];
            var desiredTranscripts = new List<Interval<ITranscript>>();

            for (var refIndex = 0; refIndex < numRefSeqs; refIndex++)
            {
                IntervalArray<ITranscript> intervalArray = cacheData.TranscriptIntervalArrays[refIndex];
                if (intervalArray == null) continue;

                desiredTranscripts.Clear();

                foreach (Interval<ITranscript> interval in intervalArray.Array)
                {
                    ITranscript transcript = interval.Value;

                    if (transcript.Source != desiredTranscriptSource)
                    {
                        numFiltered++;
                        continue;
                    }

                    desiredTranscripts.Add(interval);
                    geneSet.Add(transcript.Gene);
                    AddString(peptideSet, transcript.Translation?.PeptideSeq);
                    AddTranscriptRegions(transcriptRegionSet, transcript.TranscriptRegions);
                    AddIntervals(mirnaSet, transcript.MicroRnas);
                }

                desiredTranscriptIntervalArrays[refIndex] =
                    new IntervalArray<ITranscript>(desiredTranscripts.ToArray());
            }

            Console.WriteLine($"{numFiltered:N0} transcripts removed.");

            Console.Write("- removing duplicate genes, transcript regions, miRNAs, and peptide sequences... ");
            IGene[]             genes             = GetUniqueGenes(geneSet);
            ITranscriptRegion[] transcriptRegions = GetUniqueTranscriptRegions(transcriptRegionSet);
            IInterval[]         mirnas            = GetUniqueIntervals(mirnaSet);
            string[]            peptideSeqs       = GetUniqueStrings(peptideSet);
            Console.WriteLine("finished.");

            return new TranscriptCacheData(cacheData.Header, genes, transcriptRegions, mirnas, peptideSeqs,
                desiredTranscriptIntervalArrays,
                cacheData.RegulatoryRegionIntervalArrays);
        }

        private static string[] GetUniqueStrings(HashSet<string> peptideSet) =>
            peptideSet.Count > 0 ? peptideSet.OrderBy(x => x).ToArray() : null;

        private static IInterval[] GetUniqueIntervals(HashSet<IInterval> mirnaSet) =>
            mirnaSet.Count > 0 ? mirnaSet.SortInterval().ToArray() : null;

        private static ITranscriptRegion[] GetUniqueTranscriptRegions(HashSet<ITranscriptRegion> transcriptRegionSet) =>
            transcriptRegionSet.Count > 0 ? transcriptRegionSet.SortInterval().ToArray() : null;

        private static IGene[] GetUniqueGenes(HashSet<IGene> geneSet) =>
            geneSet.Count > 0 ? geneSet.Sort().ToArray() : null;

        private static void AddIntervals(HashSet<IInterval> intervalSet, IInterval[] intervals)
        {
            if (intervals == null) return;
            foreach (IInterval interval in intervals) intervalSet.Add(interval);
        }

        private static void AddTranscriptRegions(HashSet<ITranscriptRegion> transcriptRegionSet,
                                                 ITranscriptRegion[]        regions)
        {
            if (regions == null) return;
            foreach (var region in regions) transcriptRegionSet.Add(region);
        }

        private static void AddString(HashSet<string> stringSet, string s)
        {
            if (string.IsNullOrEmpty(s)) return;
            stringSet.Add(s);
        }
    }
}