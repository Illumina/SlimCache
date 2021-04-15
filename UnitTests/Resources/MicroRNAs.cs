using Intervals;

namespace UnitTests.Resources
{
    public static class MicroRNAs
    {
        public static readonly IInterval[] NM_000546_fake = new IInterval[]
        {
            new Interval(1000, 2000),
            new Interval(2000, 3000),
            new Interval(3000, 4000)
        };
    }
}