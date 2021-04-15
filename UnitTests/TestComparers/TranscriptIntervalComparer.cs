using System;
using System.Collections.Generic;
using Intervals;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace UnitTests.TestComparers
{
    internal sealed class TranscriptIntervalComparer : EqualityComparer<Interval<ITranscript>>
    {
        public static readonly TranscriptIntervalComparer DefaultInstance = new();

        public override bool Equals(Interval<ITranscript> x, Interval<ITranscript> y)
        {
            return x.Begin == y.Begin &&
                   x.End   == y.End   &&
                   TranscriptComparer.DefaultInstance.Equals(x.Value, y.Value);
        }

        public override int GetHashCode(Interval<ITranscript> x)
        {
            var hashCode = new HashCode();
            hashCode.Add(x.Begin);
            hashCode.Add(x.End);
            hashCode.Add(x.Value, TranscriptComparer.DefaultInstance);
            return hashCode.ToHashCode();
        }
    }
}