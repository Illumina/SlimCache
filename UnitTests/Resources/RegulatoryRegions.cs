using Intervals;
using VariantAnnotation.AnnotatedPositions.Transcript;
using VariantAnnotation.Caches.DataStructures;
using VariantAnnotation.Interface.AnnotatedPositions;
using VariantAnnotation.Interface.Caches;

namespace UnitTests.Resources
{
    public static class RegulatoryRegions
    {
        public static readonly IRegulatoryRegion ENSR00000089836 = new RegulatoryRegion(Chromosomes.Chr17, 62812, 63683,
            CompactId.Convert("ENSR00000089836"), RegulatoryRegionType.TF_binding_site);

        public static readonly IRegulatoryRegion ENSR00000282026 = new RegulatoryRegion(Chromosomes.Chr17, 64801, 65400,
            CompactId.Convert("ENSR00000089836"), RegulatoryRegionType.CTCF_binding_site);

        public static readonly IRegulatoryRegion ENSR00000089837 = new RegulatoryRegion(Chromosomes.Chr17, 64889, 65435,
            CompactId.Convert("ENSR00000089836"), RegulatoryRegionType.open_chromatin_region);

        public static readonly IntervalArray<IRegulatoryRegion>[] Chr17 = new IntervalArray<IRegulatoryRegion>[Chromosomes.NumRefSeqs];

        static RegulatoryRegions()
        {
            var chr17Regions = new Interval<IRegulatoryRegion>[]
            {
                new(ENSR00000089836.Start, ENSR00000089836.End, ENSR00000089836),
                new(ENSR00000282026.Start, ENSR00000282026.End, ENSR00000282026),
                new(ENSR00000089837.Start, ENSR00000089837.End, ENSR00000089837),
            };

            Chr17[Chromosomes.Chr17.Index] = new IntervalArray<IRegulatoryRegion>(chr17Regions);
        }
    }
}