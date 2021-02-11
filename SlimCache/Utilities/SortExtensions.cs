using System.Collections.Generic;
using System.Linq;
using Intervals;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace SlimCache.Utilities
{
    public static class SortExtensions
    {
        public static IGene[] Sort(this HashSet<IGene> genes) => genes.Count == 0
            ? null
            : genes.OrderBy(x => x.Chromosome.Index).ThenBy(x => x.Start).ThenBy(x => x.End).ToArray();

        public static ITranscriptRegion[] Sort(this HashSet<ITranscriptRegion> transcriptRegions) =>
            transcriptRegions.Count == 0
                ? null
                : transcriptRegions.OrderBy(x => x.Start).ThenBy(x => x.End).ToArray();

        public static IInterval[] Sort(this HashSet<IInterval> intervals) => intervals.Count == 0
            ? null
            : intervals.OrderBy(x => x.Start).ThenBy(x => x.End).ToArray();

        public static string[] Sort(this HashSet<string> intervals) => intervals.Count == 0
            ? null
            : intervals.OrderBy(x => x).ToArray();
    }
}