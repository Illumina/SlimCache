using System;
using System.Collections.Generic;
using System.IO;
using Intervals;
using IO;
using VariantAnnotation.Caches;
using VariantAnnotation.Caches.DataStructures;
using VariantAnnotation.Interface.AnnotatedPositions;
using VariantAnnotation.IO.Caches;

namespace SlimCache
{
    public static class PredictionFilter
    {
        public static (PredictionStaging Sift, PredictionStaging Polyphen) RemoveUnusedPredictions(
            string inputCachePrefix, TranscriptCacheData cacheData)
        {
            string siftPath     = CacheConstants.SiftPath(inputCachePrefix);
            string polyphenPath = CacheConstants.PolyPhenPath(inputCachePrefix);

            using Stream siftStream     = PersistentStreamUtils.GetReadStream(siftPath);
            using var    siftReader     = new PredictionCacheReader(siftStream, PredictionCacheReader.SiftDescriptions);
            using Stream polyphenStream = PersistentStreamUtils.GetReadStream(polyphenPath);
            using var polyphenReader =
                new PredictionCacheReader(polyphenStream, PredictionCacheReader.PolyphenDescriptions);

            var unusedPrediction = new Prediction(Array.Empty<byte>(), null);
            
            int numRefSeqs = cacheData.TranscriptIntervalArrays.Length;

            var siftPredictionsPerRef     = new Prediction[numRefSeqs][];
            var polyphenPredictionsPerRef = new Prediction[numRefSeqs][];
            
            Console.Write("- removing duplicate predictions... ");
            var numPredictionsFiltered = 0;
            
            for (ushort refIndex = 0; refIndex < numRefSeqs; refIndex++)
            {
                Prediction[] siftPredictions = siftReader.GetPredictions(refIndex);
                numPredictionsFiltered += MaskUnusedPredictions(cacheData.TranscriptIntervalArrays[refIndex],
                    t => t.SiftIndex, siftPredictions, unusedPrediction);

                Prediction[] polyphenPredictions = polyphenReader.GetPredictions(refIndex);
                numPredictionsFiltered += MaskUnusedPredictions(cacheData.TranscriptIntervalArrays[refIndex],
                    t => t.PolyPhenIndex, polyphenPredictions, unusedPrediction);

                siftPredictionsPerRef[refIndex]     = siftPredictions;
                polyphenPredictionsPerRef[refIndex] = polyphenPredictions;
            }

            var sift     = new PredictionStaging(siftReader.Header.LookupTable,     siftPredictionsPerRef);
            var polyphen = new PredictionStaging(polyphenReader.Header.LookupTable, polyphenPredictionsPerRef);
            
            Console.WriteLine($"{numPredictionsFiltered:N0} predictions removed.");

            return (sift, polyphen);
        }

        private static int MaskUnusedPredictions(IntervalArray<ITranscript> transcriptIntervalArray,
                                                 Func<ITranscript, int>     predictionFunc, Prediction[] predictions,
                                                 Prediction                 unusedPrediction)
        {
            if (transcriptIntervalArray == null) return 0;
            
            var usedPredictions = new HashSet<int>();

            foreach (Interval<ITranscript> interval in transcriptIntervalArray.Array)
            {
                int predictionIndex = predictionFunc(interval.Value);
                usedPredictions.Add(predictionIndex);
            }

            var numFiltered = 0;
            for (var predictionIndex = 0; predictionIndex < predictions.Length; predictionIndex++)
            {
                if (usedPredictions.Contains(predictionIndex)) continue;
                predictions[predictionIndex] = unusedPrediction;
                numFiltered++;
            }

            return numFiltered;
        }
    }
}