using System.IO;
using SlimCache;
using UnitTests.Resources;
using UnitTests.TestComparers;
using VariantAnnotation.Caches;
using VariantAnnotation.Caches.DataStructures;
using Xunit;

namespace UnitTests
{
    public class PredictionFilterTests
    {
        [Fact]
        public void RemoveUnusedPredictions_ExpectedResults()
        {
            TranscriptCacheData cacheData = CacheIoTests.GetCacheData();
            var                 sift      = new PredictionStaging(SIFT.Lut,     SIFT.PredictionsPerRef);
            var                 polyphen  = new PredictionStaging(PolyPhen.Lut, PolyPhen.PredictionsPerRef);

            Prediction unusedSift = sift.PredictionsPerRef[2][0];
            Assert.Equal(sift.Lut[1], unusedSift.GetPrediction('H', 4), PredictionEntryComparer.DefaultInstance);

            Prediction unusedPolyphen = polyphen.PredictionsPerRef[0][1];
            Assert.Equal(polyphen.Lut[3], unusedPolyphen.GetPrediction('Y', 4),
                PredictionEntryComparer.DefaultInstance);

            string outputCachePrefix = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            CacheWriter.Write(cacheData, sift, polyphen, outputCachePrefix);

            (PredictionStaging newSift, PredictionStaging newPolyphen) =
                PredictionFilter.RemoveUnusedPredictions(outputCachePrefix, cacheData);

            Assert.Null(newSift.PredictionsPerRef[2][0].GetPrediction('H', 4));
            Assert.Null(newPolyphen.PredictionsPerRef[0][1].GetPrediction('Y', 4));
        }
    }
}