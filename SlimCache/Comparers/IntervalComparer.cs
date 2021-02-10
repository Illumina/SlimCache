using System.Collections.Generic;
using Intervals;

namespace SlimCache.Comparers
{
    internal sealed class IntervalComparer : EqualityComparer<IInterval>
    {
        public override bool Equals(IInterval x, IInterval y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x          == null || y        == null) return false;
            return x.Start == y.Start && x.End == y.End;
        }

        public override int GetHashCode(IInterval obj)
        {
            unchecked
            {
                return (obj.Start * 397) ^ obj.End;
            }
        }
    }
}