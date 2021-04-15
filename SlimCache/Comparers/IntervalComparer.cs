using System;
using System.Collections.Generic;
using Intervals;

namespace SlimCache.Comparers
{
    internal sealed class IntervalComparer : EqualityComparer<IInterval>
    {
        public static readonly IntervalComparer DefaultInstance = new();

        public override bool Equals(IInterval x, IInterval y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x          == null || y        == null) return false;
            return x.Start == y.Start && x.End == y.End;
        }

        public override int GetHashCode(IInterval x) => HashCode.Combine(x.Start, x.End);
    }
}