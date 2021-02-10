using System;
using System.IO.Compression;
using CacheUtils.TranscriptCache;
using Compression.Algorithms;
using Compression.FileHandling;
using IO;
using VariantAnnotation.Caches;

namespace SlimCache
{
    public static class CacheWriter
    {
        public static void Write(TranscriptCacheData cacheData, PredictionStaging sift, PredictionStaging polyphen,
                                 string              outputCachePrefix)
        {
            string transcriptPath = CacheConstants.TranscriptPath(outputCachePrefix);
            string siftPath       = CacheConstants.SiftPath(outputCachePrefix);
            string polyPhenPath   = CacheConstants.PolyPhenPath(outputCachePrefix);

            var zstd = new Zstandard();

            using (var siftStream =
                new BlockStream(zstd, FileUtilities.GetCreateStream(siftPath), CompressionMode.Compress))
            {
                Console.Write("- writing SIFT prediction cache... ");
                sift.Write(siftStream, cacheData.Header.Assembly);
                Console.WriteLine("finished.");
            }
            
            using (var polyPhenStream = new BlockStream(zstd, FileUtilities.GetCreateStream(polyPhenPath),
                CompressionMode.Compress))
            {
                Console.Write("- writing PolyPhen prediction cache... ");
                polyphen.Write(polyPhenStream, cacheData.Header.Assembly);
                Console.WriteLine("finished.");
            }

            using (var writer =
                new TranscriptCacheWriter(FileUtilities.GetCreateStream(transcriptPath), cacheData.Header))
            {
                Console.Write("- writing transcript cache... ");
                writer.Write(cacheData);
                Console.WriteLine("finished.");
            }
        }
    }
}