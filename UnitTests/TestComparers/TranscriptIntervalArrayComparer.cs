using System.Collections.Generic;
using System.Linq;
using Intervals;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace UnitTests.TestComparers
{
    public class TranscriptIntervalArrayComparer : EqualityComparer<IntervalArray<ITranscript>>
    {
        public static readonly TranscriptIntervalArrayComparer DefaultInstance = new();

        public override bool Equals(IntervalArray<ITranscript> x, IntervalArray<ITranscript> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;

            bool[] results = new bool[x.Array.Length];
            for (int i = 0; i < x.Array.Length; i++)
                results[i] = TranscriptIntervalComparer.DefaultInstance.Equals(x.Array[i], y.Array[i]);

            return x.Array.SequenceEqual(y.Array, TranscriptIntervalComparer.DefaultInstance);
        }

        public override int GetHashCode(IntervalArray<ITranscript> x) =>
            x.Array.GetArrayHashCode(TranscriptIntervalComparer.DefaultInstance);
    }
}