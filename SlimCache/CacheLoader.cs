using System;
using System.Collections.Generic;
using System.IO;
using Genome;
using VariantAnnotation.Caches;
using VariantAnnotation.IO.Caches;

namespace SlimCache
{
    public static class CacheLoader
    {
        public static TranscriptCacheData LoadTranscripts(Stream                           stream,
                                                          IDictionary<ushort, IChromosome> refIndexToChromosome)
        {
            TranscriptCacheData cacheData;
            Console.Write("- loading cache... ");

            using (var reader = new TranscriptCacheReader(stream))
            {
                cacheData = reader.Read(refIndexToChromosome);
            }

            Console.WriteLine("finished.");
            return cacheData;
        }
    }
}