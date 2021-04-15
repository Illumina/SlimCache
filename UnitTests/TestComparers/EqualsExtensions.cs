using System.Collections.Generic;

namespace UnitTests.TestComparers
{
    public static class EqualsExtensions
    {
        public static bool ArrayEquals<T>(this T[] x, T[] y, IEqualityComparer<T> comparer)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            if (x.Length != y.Length) return false;

            for (var i = 0; i < x.Length; i++)
            {
                if (!comparer.Equals(x[i], y[i])) return false;
            }

            return true;
        }
    }
}