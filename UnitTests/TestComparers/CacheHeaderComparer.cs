using System;
using System.Collections.Generic;
using VariantAnnotation.IO.Caches;

namespace UnitTests.TestComparers
{
    public class CacheHeaderComparer : EqualityComparer<CacheHeader>
    {
        public static readonly CacheHeaderComparer DefaultInstance = new();

        public override bool Equals(CacheHeader x, CacheHeader y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Identifier        == y.Identifier        &&
                   x.SchemaVersion     == y.SchemaVersion     &&
                   x.DataVersion       == y.DataVersion       &&
                   x.Source            == y.Source            &&
                   x.CreationTimeTicks == y.CreationTimeTicks &&
                   x.Assembly          == y.Assembly          &&
                   x.Custom.VepVersion == y.Custom.VepVersion;
        }

        public override int GetHashCode(CacheHeader x) => HashCode.Combine(x.Identifier, x.SchemaVersion, x.DataVersion,
            (byte) x.Source, x.CreationTimeTicks, (byte) x.Assembly, x.Custom.VepVersion);
    }
}