using System.IO;
using IO;
using SlimCache;
using UnitTests.Resources;
using UnitTests.TestComparers;
using VariantAnnotation.Caches;
using Xunit;

namespace UnitTests
{
    public class CacheWriterTests
    {
        [Fact]
        public void Write_ExpectedResults()
        {
            TranscriptCacheData cacheData = CacheIoTests.GetCacheData();
            var                 sift      = new PredictionStaging(SIFT.Lut,     SIFT.PredictionsPerRef);
            var                 polyphen  = new PredictionStaging(PolyPhen.Lut, PolyPhen.PredictionsPerRef);

            string outputCachePrefix = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            string transcriptPath    = CacheConstants.TranscriptPath(outputCachePrefix);

            CacheWriter.Write(cacheData, sift, polyphen, outputCachePrefix);

            TranscriptCacheData newCacheData = LoadCacheData(transcriptPath);
            Assert.Equal(cacheData, newCacheData, TranscriptCacheDataComparer.DefaultInstance);
        }

        private static TranscriptCacheData LoadCacheData(string filePath) =>
            CacheLoader.LoadTranscripts(FileUtilities.GetReadStream(filePath), Chromosomes.RefIndexToChromosome);
    }
}