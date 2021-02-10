using System;
using System.IO;
using CommandLine.Utilities;
using VariantAnnotation.Caches;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace SlimCache
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("USAGE: {0} <cache prefix> <reference path> <output cache prefix> <desired transcript source>",
                    Path.GetFileName(Environment.GetCommandLineArgs()[0]));
                Environment.Exit(1);
            }

            string inputCachePrefix  = args[0];
            string referencePath     = args[1];
            string outputCachePrefix = args[2];
            string transcriptSource  = args[3];
            
            Source desiredTranscriptSource = GetSource(transcriptSource);

            TranscriptCacheData cacheData = CacheLoader.LoadTranscripts(inputCachePrefix, referencePath);
            
            TranscriptCacheData filteredCacheData = cacheData.FilterTranscripts(desiredTranscriptSource);
            (PredictionStaging sift, PredictionStaging polyphen) =
                PredictionFilter.RemoveUnusedPredictions(inputCachePrefix, filteredCacheData);
            
            CacheWriter.Write(filteredCacheData, sift, polyphen, outputCachePrefix);
            
            Console.WriteLine($"\nPeak memory usage: {MemoryUtilities.ToHumanReadable(MemoryUtilities.GetPeakMemoryUsage())}");
        }

        private static Source GetSource(string source)
        {
            source = source.ToLower();
            if (source.StartsWith("ensembl")) return Source.Ensembl;
            if (source.StartsWith("refseq")) return Source.RefSeq;
            
            throw new InvalidDataException("Transcript source must be either Ensembl or RefSeq");
        }
    }
}