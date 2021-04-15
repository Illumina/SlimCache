using System.Collections.Generic;
using System.Linq;
using Intervals;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace UnitTests.TestComparers
{
    public class RegulatoryRegionIntervalArrayComparer : EqualityComparer<IntervalArray<IRegulatoryRegion>>
    {
        public static readonly RegulatoryRegionIntervalArrayComparer DefaultInstance = new();

        public override bool Equals(IntervalArray<IRegulatoryRegion> x, IntervalArray<IRegulatoryRegion> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Array.SequenceEqual(y.Array, RegulatoryRegionIntervalComparer.DefaultInstance);
        }

        public override int GetHashCode(IntervalArray<IRegulatoryRegion> x) =>
            x.Array.GetArrayHashCode(RegulatoryRegionIntervalComparer.DefaultInstance);
    }
}