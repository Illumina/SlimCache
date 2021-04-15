using System;
using System.Collections.Generic;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace UnitTests.TestComparers
{
    public class TranslationComparer : EqualityComparer<ITranslation>
    {
        public static readonly TranslationComparer DefaultInstance = new();

        public override bool Equals(ITranslation x, ITranslation y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;

            return CodingRegionComparer.DefaultInstance.Equals(x.CodingRegion, y.CodingRegion) &&
                   x.ProteinId.WithVersion == y.ProteinId.WithVersion                          &&
                   x.PeptideSeq            == y.PeptideSeq;
        }

        public override int GetHashCode(ITranslation x)
        {
            var hashCode = new HashCode();
            hashCode.Add(x.CodingRegion, CodingRegionComparer.DefaultInstance);
            hashCode.Add(x.ProteinId.WithVersion);
            hashCode.Add(x.PeptideSeq);
            return hashCode.ToHashCode();
        }
    }
}