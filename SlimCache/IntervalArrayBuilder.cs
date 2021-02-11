using System.Collections.Generic;
using Intervals;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace SlimCache
{
    public sealed class IntervalArrayBuilder
    {
        private readonly List<Interval<ITranscript>> _transcripts = new();

        public void Clear() => _transcripts.Clear();

        public void Add(ITranscript transcript) =>
            _transcripts.Add(new Interval<ITranscript>(transcript.Start, transcript.End, transcript));

        public void Add(Interval<ITranscript> transcriptInterval) => _transcripts.Add(transcriptInterval);

        public IntervalArray<ITranscript> GetIntervalArray() => new(_transcripts.ToArray());
    }
}