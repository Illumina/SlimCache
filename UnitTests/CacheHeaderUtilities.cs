using System;
using CacheUtils.Utilities;
using Genome;
using VariantAnnotation.Interface.AnnotatedPositions;
using VariantAnnotation.IO.Caches;

namespace UnitTests
{
    public static class CacheHeaderUtilities
    {
        public static CacheHeader CreateHeader(Source source)
        {
            Header header       = HeaderUtilities.GetHeader(source, GenomeAssembly.GRCh38);
            var    customHeader = new TranscriptCacheCustomHeader(91, DateTime.Now.Ticks);
            return new CacheHeader(header, customHeader);
        }
    }
}