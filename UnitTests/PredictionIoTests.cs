using System.IO;
using System.IO.Compression;
using Compression.Algorithms;
using Compression.FileHandling;
using Genome;
using SlimCache;
using UnitTests.Comparers;
using UnitTests.TestComparers;
using VariantAnnotation.Caches.DataStructures;
using VariantAnnotation.IO.Caches;
using Xunit;

namespace UnitTests
{
    public class PredictionIoTests
    {
        private const int NumAminoAcids = 20;
        private static readonly int[] AminoAcidIndices = { 0, -1, 1, 2, 3, 4, 5, 6, 7, -1, 8, 9, 10, 11, -1, 12, 13, 14, 15, 16, -1, 17, 18, -1, 19, -1 };
        
        [Fact]
        public void ReadWrite_ExpectedResults()
        {
            (Prediction.Entry[] lut, Prediction[][] predictionsPerRef) = GetStaging();
            var staging = new PredictionStaging(lut, predictionsPerRef);

            (Prediction.Entry[] newLut, Prediction[][] newPredictionsPerRef) = WriteAndReadPredictions(staging);

            var comparer = PredictionEntryComparer.DefaultInstance;
            Assert.Equal(lut, newLut, comparer);

            int numRefSeqs = predictionsPerRef.Length;
            Assert.Equal(numRefSeqs, newPredictionsPerRef.Length);
            
            // ref index 0
            Prediction prediction = newPredictionsPerRef[0][0];
            Assert.Equal(lut[0], prediction.GetPrediction('K', 1), comparer);
            Assert.Equal(lut[1], prediction.GetPrediction('D', 2), comparer);
            Assert.Equal(lut[2], prediction.GetPrediction('P', 3), comparer);
            Assert.Equal(lut[3], prediction.GetPrediction('T', 4), comparer);
            
            Assert.NotEqual(lut[3], prediction.GetPrediction('P', 4), comparer);

            // ref index 1
            prediction = newPredictionsPerRef[1][0];
            
            Assert.Equal(lut[3], prediction.GetPrediction('A', 1), comparer);
            Assert.Equal(lut[2], prediction.GetPrediction('C', 2), comparer);
            Assert.Equal(lut[1], prediction.GetPrediction('M', 3), comparer);
            Assert.Equal(lut[0], prediction.GetPrediction('Y', 4), comparer);
            
            Assert.NotEqual(lut[1], prediction.GetPrediction('Y', 3), comparer);

            // ref index 2 = null
            Assert.Empty(newPredictionsPerRef[2]);
        }

        private static (Prediction.Entry[] Lut, Prediction[][] PredictionsPerRef) GetStaging()
        {
            const int numRefSeqs    = 3;
            const int numLutEntries = 4;

            var lut = new Prediction.Entry[numLutEntries];
        
            lut[0] = new Prediction.Entry(1.0, 0); // tolerated
            lut[1] = new Prediction.Entry(0.0, 1); // deleterious
            lut[2] = new Prediction.Entry(0.6, 2); // tolerated - low confidence
            lut[3] = new Prediction.Entry(0.3, 3); // deleterious - low confidence

            var predictionsPerRef = new Prediction[numRefSeqs][];
            
            // ref index 0
            var data = new byte[NumAminoAcids * 4];

            data[GetIndex('K', 1)] = 0;
            data[GetIndex('D', 2)] = 1;
            data[GetIndex('P', 3)] = 2;
            data[GetIndex('T', 4)] = 3;
            
            predictionsPerRef[0]    = new Prediction[1];
            predictionsPerRef[0][0] = new Prediction(data, lut);
            
            // ref index 1
            var data2 = new byte[NumAminoAcids * 4];

            data2[GetIndex('A', 1)] = 3;
            data2[GetIndex('C', 2)] = 2;
            data2[GetIndex('M', 3)] = 1;
            data2[GetIndex('Y', 4)] = 0;
            
            predictionsPerRef[1]    = new Prediction[1];
            predictionsPerRef[1][0] = new Prediction(data2, lut);

            // ref index 2 = null

            return (lut, predictionsPerRef);
        }
        
        private static int GetIndex(char newAminoAcid, int aaPosition)
        {
            
            int       asciiIndex    = char.ToUpper(newAminoAcid) - 'A';
            
            int aaIndex = AminoAcidIndices[asciiIndex];
            
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