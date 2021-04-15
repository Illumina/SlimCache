using System;
using System.Collections.Generic;
using Intervals;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace UnitTests.TestComparers
{
    public class RegulatoryRegionIntervalComparer : EqualityComparer<Interval<IRegulatoryRegion>>
    {
        public static readonly RegulatoryRegionIntervalComparer DefaultInstance = new();

        public override bool Equals(Interval<IRegulatoryRegion> x, Interval<IRegulatoryRegion> y)
        {
            return x.Begin == y.Begin &&
                   x.End   == y.End   &&
                   RegulatoryRegionComparer.DefaultInstance.Equals(x.Value, y.Value);
        }

        public override int GetHashCode(Interval<IRegulatoryRegion> x)
        {
            var hashCode = new HashCode();
            hashCode.Add(x.Begin);
            hashCode.Add(x.End);
            hashCode.Add(x.Value, RegulatoryRegionComparer.DefaultInstance);
            return hashCode.ToHashCode();
        }
    }
}