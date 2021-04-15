using System;
using System.Collections.Generic;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace UnitTests.TestComparers
{
    public class RnaEditComparer : EqualityComparer<IRnaEdit>
    {
        public static readonly RnaEditComparer DefaultInstance = new();

        public override bool Equals(IRnaEdit x, IRnaEdit y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Start == y.Start &&
                   x.End   == y.End   &&
                   x.Bases == y.Bases;
        }

        public override int GetHashCode(IRnaEdit x) => HashCode.Combine(x.Start, x.End, x.Bases);
    }
}