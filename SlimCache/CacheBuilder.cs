using System.Collections.Generic;
using Intervals;
using SlimCache.Comparers;
using SlimCache.Utilities;
using VariantAnnotation.Caches;
using VariantAnnotation.Interface.AnnotatedPositions;
using VariantAnnotation.IO.Caches;

namespace SlimCache
{
    public sealed class CacheBuilder
    {
        private readonly CacheHeader _header;

        private readonly HashSet<IGene>             _geneSet;
        private readonly HashSet<ITranscriptRegion> _transcriptRegionSet;
        private readonly HashSet<IInterval>         _mirnaSet;
        private readonly HashSet<string>            _peptideSet;

        private readonly IntervalArray<ITranscript>[]       _transcriptIntervalArrays;
        private readonly IntervalArray<IRegulatoryRegion>[] _regulatoryRegionArrays;

        public CacheBuilder(CacheHeader header, IntervalArray<IRegulatoryRegion>[] regulatoryRegionArrays)
        {
            _header                 = header;
            _regulatoryRegionArrays = regulatoryRegionArrays;

            _geneSet             = new HashSet<IGene>(GeneComparer.DefaultInstance);
            _transcriptRegionSet = new HashSet<ITranscriptRegion>(TranscriptRegionComparer.DefaultInstance);
            _mirnaSet            = new HashSet<IInterval>(IntervalComparer.DefaultInstance);
            _peptideSet          = new HashSet<string>();

            _transcriptIntervalArrays = new IntervalArray<ITranscript>[regulatoryRegionArrays.Length];
        }

        public void Add(ITranscript transcript)
        {
            _geneSet.Add(transcript.Gene);
            Add(_peptideSet,          transcript.Translation?.PeptideSeq);
            Add(_transcriptRegionSet, transcript.TranscriptRegions);
            Add(_mirnaSet,            transcript.MicroRnas);
        }

        public void Add(int refIndex, IntervalArray<ITranscript> transcriptIntervalArray) =>
            _transcriptIntervalArrays[refIndex] = transcriptIntervalArray;

        internal static void Add<T>(HashSet<T> itemSet, T[] newItems)
        {
            if (newItems == null) return;
            foreach (T item in newItems) itemSet.Add(item);
        }

        internal static void Add(HashSet<string> strings, string s)
        {
            if (string.IsNullOrEmpty(s)) return;
            strings.Add(s);
        }

        public TranscriptCacheData GetCacheData()
        {
            IGene[]             genes             = _geneSet.Sort();
            ITranscriptRegion[] transcriptRegions = _transcriptRegionSet.Sort();
            IInterval[]         mirnas            = _mirnaSet.Sort();
            string[]            peptideSeqs       = _peptideSet.Sort();

            return new TranscriptCacheData(_header, genes, transcriptRegions, mirnas, peptideSeqs,
                _transcriptIntervalArrays, _regulatoryRegionArrays);
        }
    }
}