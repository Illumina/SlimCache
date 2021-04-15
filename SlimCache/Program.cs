using System;
using System.Collections.Generic;
using System.IO;
using CommandLine.Utilities;
using Genome;
using IO;
using SlimCache.Utilities;
using VariantAnnotation.Caches;
using VariantAnnotation.Interface.AnnotatedPositions;
using VariantAnnotation.Providers;

namespace SlimCache
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine(
                    "USAGE: {0} <cache prefix> <reference path> <output cache prefix> <desired transcript source>",
                    Path.GetFileName(Environment.GetCommandLineArgs()[0]));
                Environment.Exit(1);
            }

            string inputCachePrefix  = args[0];
            string referencePath     = args[1];
            string outputCachePrefix = args[2];
            string transcriptSource  = args[3];

            Source desiredTranscriptSource = SourceUtilities.GetSource(transcriptSource);

            Console.Write("- loading reference sequence... ");
            IDictionary<ushort, IChromosome> refIndexToChromosome =
                new ReferenceSequenceProvider(FileUtilities.GetReadStream(referencePath)).RefIndexToChromosome;
            Console.WriteLine("finished.");

            string              transcriptPath   = CacheConstants.TranscriptPath(inputCachePrefix);
            using FileStream    transcriptStream = FileUtilities.GetReadStream(transcriptPath);
            TranscriptCacheData cacheData        = CacheLoader.LoadTranscripts(transcriptStream, refIndexToChromosome);

            Console.Write("- filtering transcripts... ");
            (TranscriptCacheData filteredCacheData, int numFilteredTranscripts) = CacheFilter.FilterTranscripts(
                cacheData.Header, cacheData.TranscriptIntervalArrays, cacheData.RegulatoryRegionIntervalArrays,
                desiredTranscriptSource);
            Console.WriteLine($"{numFilteredTranscripts:N0} transcripts removed.");
            
            (PredictionStaging sift, PredictionStaging polyphen) =
                PredictionFilter.RemoveUnusedPredictions(inputCachePrefix, filteredCacheData);

            CacheWriter.Write(filteredCacheData, sift, polyphen, outputCachePrefix);

            Console.WriteLine(
                $"\nPeak memory usage: {MemoryUtilities.ToHumanReadable(MemoryUtilities.GetPeakMemoryUsage())}");
        }
    }
}