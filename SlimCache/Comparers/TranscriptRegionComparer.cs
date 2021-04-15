using System;
using System.Collections.Generic;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace SlimCache.Comparers
{
    internal sealed class TranscriptRegionComparer : EqualityComparer<ITranscriptRegion>
    {
        public static readonly TranscriptRegionComparer DefaultInstance = new();

        public override bool Equals(ITranscriptRegion x, ITranscriptRegion y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Type      == y.Type      &&
                   x.Id        == y.Id        &&
                   x.Start     == y.Start     &&
                   x.End       == y.End       &&
                   x.CdnaStart == y.CdnaStart &&
                   x.CdnaEnd   == y.CdnaEnd;
        }

        public override int GetHashCode(ITranscriptRegion x) =>
            HashCode.Combine(x.Start, x.End, (byte) x.Type, x.Id, x.CdnaStart, x.CdnaEnd);
    }
}