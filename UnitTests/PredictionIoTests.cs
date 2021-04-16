using System.IO;
using System.IO.Compression;
using Compression.Algorithms;
using Compression.FileHandling;
using Genome;
using SlimCache;
using UnitTests.Resources;
using UnitTests.TestComparers;
using VariantAnnotation.Caches.DataStructures;
using VariantAnnotation.IO.Caches;
using Xunit;

namespace UnitTests
{
    public class PredictionIoTests
    {
        private const int NumAminoAcids = 20;

        private static readonly int[] AminoAcidIndices =
            {0, -1, 1, 2, 3, 4, 5, 6, 7, -1, 8, 9, 10, 11, -1, 12, 13, 14, 15, 16, -1, 17, 18, -1, 19, -1};

        [Fact]
        public void ReadWrite_ExpectedResults()
        {
            var staging = new PredictionStaging(SIFT.Lut, SIFT.PredictionsPerRef);

            (Prediction.Entry[] newLut, Prediction[][] newPredictionsPerRef) = WriteAndReadPredictions(staging);

            var comparer = PredictionEntryComparer.DefaultInstance;
            Assert.Equal(SIFT.Lut, newLut, comparer);

            int numRefSeqs = SIFT.PredictionsPerRef.Length;
            Assert.Equal(numRefSeqs, newPredictionsPerRef.Length);

            // ref index 0 (chr1)
            Assert.Empty(newPredictionsPerRef[0]);

            // ref index 1 (chr12)
            Prediction prediction = newPredictionsPerRef[1][0];

            Assert.Equal(SIFT.Lut[3], prediction.GetPrediction('G',    1), comparer);
            Assert.Equal(SIFT.Lut[2], prediction.GetPrediction('R',    2), comparer);
            Assert.Equal(SIFT.Lut[1], prediction.GetPrediction('S',    3), comparer);
            Assert.Equal(SIFT.Lut[0], prediction.GetPrediction('A',    4), comparer);
            Assert.NotEqual(SIFT.Lut[1], prediction.GetPrediction('Y', 3), comparer);

            // ref index 2 (chr17)
            prediction = newPredictionsPerRef[2][1];
            Assert.Equal(SIFT.Lut[3], prediction.GetPrediction('A',    1), comparer);
            Assert.Equal(SIFT.Lut[2], prediction.GetPrediction('C',    2), comparer);
            Assert.Equal(SIFT.Lut[1], prediction.GetPrediction('M',    3), comparer);
            Assert.Equal(SIFT.Lut[0], prediction.GetPrediction('Y',    4), comparer);
            Assert.NotEqual(SIFT.Lut[0], prediction.GetPrediction('M', 3), comparer);

            prediction = newPredictionsPerRef[2][2];
            Assert.Equal(SIFT.Lut[0], prediction.GetPrediction('C',    1), comparer);
            Assert.Equal(SIFT.Lut[1], prediction.GetPrediction('A',    2), comparer);
            Assert.Equal(SIFT.Lut[2], prediction.GetPrediction('Y',    3), comparer);
            Assert.Equal(SIFT.Lut[3], prediction.GetPrediction('M',    4), comparer);
            Assert.NotEqual(SIFT.Lut[1], prediction.GetPrediction('P', 2), comparer);
        }

        public static int GetIndex(char newAminoAcid, int aaPosition)
        {
            int asciiIndex = char.ToUpper(newAminoAcid) - 'A';
            int aaIndex    = AminoAcidIndices[asciiIndex];
            return NumAminoAcids * (aaPosition - 1) + aaIndex;
        }

        private static (Prediction.Entry[] Lut, Prediction[][] PredictionsPerRef) WriteAndReadPredictions(
            PredictionStaging staging)
        {
            var zstd = new Zstandard();

            const GenomeAssembly assembly = GenomeAssembly.GRCh38;

            using var ms = new MemoryStream();

            using (var outputStream = new BlockStream(zstd, ms, CompressionMode.Compress, true))
            {
                staging.Write(outputStream, assembly);
            }

            ms.Position = 0;

            using var predictionCacheReader = new PredictionCacheReader(ms, PredictionCacheReader.SiftDescriptions);

            Prediction.Entry[] lut = predictionCacheReader.Header.LookupTable;

            int numRefSeqs        = predictionCacheReader.Header.Custom.Entries.Length;
            var predictionsPerRef = new Prediction[numRefSeqs][];

            for (ushort refIndex = 0; refIndex < numRefSeqs; refIndex++)
            {
                predictionsPerRef[refIndex] = predictionCacheReader.GetPredictions(refIndex);
            }

            return (lut, predictionsPerRef);
        }
    }
}