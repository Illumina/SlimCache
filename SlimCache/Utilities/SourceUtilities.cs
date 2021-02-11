using System.IO;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace SlimCache.Utilities
{
    public static class SourceUtilities
    {
        public static Source GetSource(string source)
        {
            source = source.ToLower();
            if (source.StartsWith("ensembl")) return Source.Ensembl;
            if (source.StartsWith("refseq")) return Source.RefSeq;
            
            throw new InvalidDataException("Transcript source must be either Ensembl or RefSeq");
        }
    }
}