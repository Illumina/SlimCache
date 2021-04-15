using System;
using System.Collections.Generic;
using VariantAnnotation.Caches.DataStructures;

namespace UnitTests.TestComparers
{
    public class PredictionEntryComparer : EqualityComparer<Prediction.Entry>
    {
        public static readonly PredictionEntryComparer DefaultInstance = new();

        public override bool Equals(Prediction.Entry x, Prediction.Entry y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Score     == y.Score &&
                   x.EnumIndex == y.EnumIndex;
        }

        public override int GetHashCode(Prediction.Entry x) => HashCode.Combine(x.Score, x.EnumIndex);
    }
}