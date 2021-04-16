using System.IO;
using CacheUtils.Utilities;
using Compression.FileHandling;
using Genome;
using VariantAnnotation.Caches.DataStructures;
using VariantAnnotation.Interface.AnnotatedPositions;
using VariantAnnotation.IO.Caches;

namespace SlimCache
{
    public sealed class PredictionStaging
    {
        public readonly Prediction.Entry[] Lut;
        public readonly Prediction[][]     PredictionsPerRef;

        public PredictionStaging(Prediction.Entry[] lut, Prediction[][] predictionsPerRef)
        {
            Lut               = lut;
            PredictionsPerRef = predictionsPerRef;
        }

        public void Write(BlockStream blockStream, GenomeAssembly genomeAssembly)
        {
            using var writer = new BinaryWriter(blockStream);

            var indexEntries = new IndexEntry[PredictionsPerRef.Length];

            Header header       = HeaderUtilities.GetHeader(Source.None, genomeAssembly);
            var    customHeader = new PredictionCacheCustomHeader(indexEntries);

            var predictionHeader = new PredictionHeader(header, customHeader, Lut);
            blockStream.WriteHeader(predictionHeader.Write);

            WriteLookupTable(writer);
            blockStream.Flush();

            WritePredictions(blockStream, writer, indexEntries);
        }

        private void WriteLookupTable(BinaryWriter writer)
        {
            writer.Write(Lut.Length);
            foreach (Prediction.Entry entry in Lut) entry.Write(writer);
        }

        private void WritePredictions(BlockStream blockStream, BinaryWriter writer, IndexEntry[] indexEntries)
        {
            for (var refIndex = 0; refIndex < PredictionsPerRef.Length; refIndex++)
            {
                Prediction[] refPredictions = PredictionsPerRef[refIndex];

                (long fileOffset, _)              = blockStream.GetBlockPosition();
                indexEntries[refIndex].FileOffset = fileOffset;
                indexEntries[refIndex].Count      = refPredictions?.Length ?? 0;

                if (refPredictions != null)
                {
                    foreach (Prediction prediction in refPredictions) prediction.Write(writer);
                }

                blockStream.Flush();
            }
        }
    }
}