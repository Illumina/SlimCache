using Intervals;

namespace UnitTests
{
    public static class IntervalExtensions
    {
        public static Interval<T> GetInterval<T>(this T item) where T : IInterval => new(item.Start, item.End, item);
    }
}