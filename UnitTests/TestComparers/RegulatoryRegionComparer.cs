using System;
using System.Collections.Generic;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace UnitTests.TestComparers
{
    public class RegulatoryRegionComparer : EqualityComparer<IRegulatoryRegion>
    {
        public static readonly RegulatoryRegionComparer DefaultInstance = new();

        public override bool Equals(IRegulatoryRegion x, IRegulatoryRegion y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Start             == y.Start             &&
                   x.End               == y.End               &&
                   x.Chromosome.Index  == y.Chromosome.Index  &&
                   x.Id.WithoutVersion == y.Id.WithoutVersion &&
                   x.Type              == y.Type;
        }

        public override int GetHashCode(IRegulatoryRegion x) => HashCode.Combine(x.Chromosome.Index, x.Start, x.End,
            x.Id.WithoutVersion, (byte) x.Type);
    }
}