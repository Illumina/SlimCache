using System;
using System.Collections.Generic;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace SlimCache.Comparers
{
    internal sealed class GeneComparer : EqualityComparer<IGene>
    {
        public static readonly GeneComparer DefaultInstance = new();

        public override bool Equals(IGene x, IGene y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Start                    == y.Start                    &&
                   x.End                      == y.End                      &&
                   x.Chromosome.Index         == y.Chromosome.Index         &&
                   x.OnReverseStrand          == y.OnReverseStrand          &&
                   x.Symbol                   == y.Symbol                   &&
                   x.EntrezGeneId.WithVersion == y.EntrezGeneId.WithVersion &&
                   x.EnsemblId.WithVersion    == y.EnsemblId.WithVersion    &&
                   x.HgncId                   == y.HgncId;
        }

        public override int GetHashCode(IGene x)
        {
            var hashCode = new HashCode();
            hashCode.Add(x.Chromosome.Index);
            hashCode.Add(x.Start);
            hashCode.Add(x.End);
            hashCode.Add(x.OnReverseStrand);
            hashCode.Add(x.Symbol);
            hashCode.Add(x.EntrezGeneId.WithVersion);
            hashCode.Add(x.EnsemblId.WithVersion);
            hashCode.Add(x.HgncId);
            return hashCode.ToHashCode();
        }
    }
}