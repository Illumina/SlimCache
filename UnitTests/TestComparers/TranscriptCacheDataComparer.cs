using System;
using System.Collections.Generic;
using System.Linq;
using SlimCache.Comparers;
using VariantAnnotation.Caches;

namespace UnitTests.TestComparers
{
    public class TranscriptCacheDataComparer : EqualityComparer<TranscriptCacheData>
    {
        public static readonly TranscriptCacheDataComparer DefaultInstance = new();

        public override bool Equals(TranscriptCacheData x, TranscriptCacheData y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;

            return CacheHeaderComparer.DefaultInstance.Equals(x.Header, y.Header)                                   &&
                   x.Genes.SequenceEqual(y.Genes, GeneComparer.DefaultInstance)                                     &&
                   x.TranscriptRegions.SequenceEqual(y.TranscriptRegions, TranscriptRegionComparer.DefaultInstance) &&
                   x.Mirnas.SequenceEqual(y.Mirnas, IntervalComparer.DefaultInstance)                               &&
                   x.PeptideSeqs.SequenceEqual(y.PeptideSeqs)                                                       &&
                   x.TranscriptIntervalArrays.SequenceEqual(y.TranscriptIntervalArrays,
                       TranscriptIntervalArrayComparer.DefaultInstance) &&
                   x.RegulatoryRegionIntervalArrays.SequenceEqual(y.RegulatoryRegionIntervalArrays,
                       RegulatoryRegionIntervalArrayComparer.DefaultInstance);
        }

        public override int GetHashCode(TranscriptCacheData x)
        {
            var hashCode = new HashCode();
            hashCode.Add(x.Header, CacheHeaderComparer.DefaultInstance);
            hashCode.Add(x.Genes.GetArrayHashCode(GeneComparer.DefaultInstance));
            hashCode.Add(x.TranscriptRegions.GetArrayHashCode(TranscriptRegionComparer.DefaultInstance));
            hashCode.Add(x.Mirnas.GetArrayHashCode(IntervalComparer.DefaultInstance));
            hashCode.Add(x.PeptideSeqs);
            hashCode.Add(x.TranscriptIntervalArrays.GetArrayHashCode(TranscriptIntervalArrayComparer.DefaultInstance));
            hashCode.Add(
                x.RegulatoryRegionIntervalArrays.GetArrayHashCode(RegulatoryRegionIntervalArrayComparer
                    .DefaultInstance));
            return hashCode.ToHashCode();
        }
    }
}