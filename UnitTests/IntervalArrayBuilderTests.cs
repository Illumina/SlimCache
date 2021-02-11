using Intervals;
using SlimCache;
using UnitTests.Resources;
using VariantAnnotation.Interface.AnnotatedPositions;
using Xunit;

namespace UnitTests
{
    public sealed class IntervalArrayBuilderTests
    {
        [Fact]
        public void GetIntervalArray_ExpectedResults()
        {
            var transcriptBuilder = new IntervalArrayBuilder();

            transcriptBuilder.Add(Transcripts.NM_000546);
            transcriptBuilder.Add(Transcripts.ENST00000610292);

            transcriptBuilder.Add(new Interval<ITranscript>(Transcripts.NM_002524.Start, Transcripts.NM_002524.End,
                Transcripts.NM_002524));

            IntervalArray<ITranscript> observed = transcriptBuilder.GetIntervalArray();
            Assert.Equal(3, observed.Array.Length);

            transcriptBuilder.Clear();
            observed = transcriptBuilder.GetIntervalArray();
            Assert.Empty(observed.Array);
        }
    }
}