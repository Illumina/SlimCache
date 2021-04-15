using System;
using System.Collections.Generic;
using System.Linq;
using SlimCache.Comparers;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace UnitTests.TestComparers
{
    internal sealed class TranscriptComparer : EqualityComparer<ITranscript>
    {
        public static readonly TranscriptComparer DefaultInstance = new();
        
        public override bool Equals(ITranscript x, ITranscript y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            
            return x.Chromosome.Index == y.Chromosome.Index                                                         &&
                   x.Start            == y.Start                                                                    &&
                   x.End              == y.End                                                                      &&
                   x.Id.WithVersion   == y.Id.WithVersion                                                           &&
                   x.BioType          == y.BioType                                                                  &&
                   x.IsCanonical      == y.IsCanonical                                                              &&
                   x.Source           == y.Source                                                                   &&
                   GeneComparer.DefaultInstance.Equals(x.Gene, y.Gene)                                              &&
                   x.TranscriptRegions.SequenceEqual(y.TranscriptRegions, TranscriptRegionComparer.DefaultInstance) &&
                   x.NumExons        == y.NumExons                                                                  &&
                   x.TotalExonLength == y.TotalExonLength                                                           &&
                   x.StartExonPhase  == y.StartExonPhase                                                            &&
                   x.SiftIndex       == y.SiftIndex                                                                 &&
                   x.PolyPhenIndex   == y.PolyPhenIndex                                                             &&
                   TranslationComparer.DefaultInstance.Equals(x.Translation, y.Translation)                         &&
                   x.MicroRnas.ArrayEquals(y.MicroRnas, IntervalComparer.DefaultInstance)                           &&
                   x.Selenocysteines.ArrayEquals(y.Selenocysteines, EqualityComparer<int>.Default)                  &&
                   x.RnaEdits.ArrayEquals(y.RnaEdits, RnaEditComparer.DefaultInstance);
        }

        public override int GetHashCode(ITranscript x)
        {
            var hashCode = new HashCode();
            hashCode.Add(x.Chromosome);
            hashCode.Add(x.Start);
            hashCode.Add(x.End);
            hashCode.Add(x.Id);
            hashCode.Add((int) x.BioType);
            hashCode.Add(x.IsCanonical);
            hashCode.Add((int) x.Source);
            hashCode.Add(x.Gene, GeneComparer.DefaultInstance);
            hashCode.Add(x.TranscriptRegions.GetArrayHashCode(TranscriptRegionComparer.DefaultInstance));
            hashCode.Add(x.NumExons);
            hashCode.Add(x.TotalExonLength);
            hashCode.Add(x.StartExonPhase);
            hashCode.Add(x.SiftIndex);
            hashCode.Add(x.PolyPhenIndex);
            hashCode.Add(x.Translation, TranslationComparer.DefaultInstance);
            hashCode.Add(x.MicroRnas.GetArrayHashCode(IntervalComparer.DefaultInstance));
            hashCode.Add(x.Selenocysteines);
            hashCode.Add(x.RnaEdits.GetArrayHashCode(RnaEditComparer.DefaultInstance));
            return hashCode.ToHashCode();
        }
    }
}