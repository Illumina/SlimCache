using System;
using System.Collections.Generic;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace UnitTests.TestComparers
{
    public class CodingRegionComparer : EqualityComparer<ICodingRegion>
    {
        public static readonly CodingRegionComparer DefaultInstance = new();

        public override bool Equals(ICodingRegion x, ICodingRegion y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Start     == y.Start     &&
                   x.End       == y.End       &&
                   x.CdnaStart == y.CdnaStart &&
                   x.CdnaEnd   == y.CdnaEnd   &&
                   x.Length    == y.Length;
        }

        public override int GetHashCode(ICodingRegion x) =>
            HashCode.Combine(x.Start, x.End, x.CdnaStart, x.CdnaEnd, x.Length);
    }
}