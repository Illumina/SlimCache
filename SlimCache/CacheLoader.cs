using System;
using IO;
using VariantAnnotation.Caches;
using VariantAnnotation.IO.Caches;
using VariantAnnotation.Providers;

namespace SlimCache
{
    public static class CacheLoader
    {
        public static TranscriptCacheData LoadTranscripts(string inputCachePrefix, string referencePath)
        {
            Console.Write("- loading reference sequence... ");
            var sequenceProvider = new ReferenceSequenceProvider(FileUtilities.GetReadStream(referencePath));
            Console.WriteLine("finished.");

            Console.Write("- loading cache... ");

            string transcriptPath = CacheConstants.TranscriptPath(inputCachePrefix);

            TranscriptCacheData cacheData;
            using (var reader = new TranscriptCacheReader(FileUtilities.GetReadStream(transcriptPath)))
            {
                cacheData = reader.Read(sequenceProvider.RefIndexToChromosome);
            }

            Console.WriteLine("finished.");
            return cacheData;
        }
    }
}