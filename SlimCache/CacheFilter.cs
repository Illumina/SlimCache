using Intervals;
using VariantAnnotation.Caches;
using VariantAnnotation.Interface.AnnotatedPositions;
using VariantAnnotation.IO.Caches;

namespace SlimCache
{
    public static class CacheFilter
    {
        public static (TranscriptCacheData CacheData, int NumFilteredTranscripts) FilterTranscripts(
            CacheHeader                        header,
            IntervalArray<ITranscript>[]       transcriptIntervalArrays,
            IntervalArray<IRegulatoryRegion>[] regulatoryRegionIntervalArrays,
            Source                             desiredTranscriptSource)
        {
            int numRefSeqs             = transcriptIntervalArrays.Length;
            var numFilteredTranscripts = 0;

            var builder           = new CacheBuilder(header, regulatoryRegionIntervalArrays);
            var transcriptBuilder = new IntervalArrayBuilder();

            for (var refIndex = 0; refIndex < numRefSeqs; refIndex++)
            {
                IntervalArray<ITranscript> intervalArray = transcriptIntervalArrays[refIndex];
                if (intervalArray == null) continue;

                transcriptBuilder.Clear();

                foreach (Interval<ITranscript> interval in intervalArray.Array)
                {
                    ITranscript transcript = interval.Value;

                    if (transcript.Source != desiredTranscriptSource)
                    {
                        numFilteredTranscripts++;
                        continue;
                    }

                    transcriptBuilder.Add(interval);
                    builder.Add(transcript);
                }

                builder.Add(refIndex, transcriptBuilder.GetIntervalArray());
            }

            return (builder.GetCacheData(), numFilteredTranscripts);
        }
    }
}