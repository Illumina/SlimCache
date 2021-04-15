using System;
using System.Collections.Generic;

namespace UnitTests.TestComparers
{
    public static class HashCodeExtensions
    {
        public static int GetArrayHashCode<T>(this T[] list, IEqualityComparer<T> comparer)
        {
            var hashCode = new HashCode();
            foreach (T entry in list) hashCode.Add(entry, comparer);
            return hashCode.ToHashCode();
        }
    }
}