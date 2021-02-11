using VariantAnnotation.Caches.DataStructures;
using VariantAnnotation.Interface.AnnotatedPositions;

namespace UnitTests.Resources
{
    public static class RnaEdits
    {
        public static readonly IRnaEdit NM_033360_0 = new RnaEdit(3474, 3474, "G");
        public static readonly IRnaEdit NM_033360_1 = new RnaEdit(1147, 1147, "T");
        public static readonly IRnaEdit NM_033360_2 = new RnaEdit(675,  675,  "G");

        public static readonly IRnaEdit[] NM_033360 = {NM_033360_0, NM_033360_1, NM_033360_2};
    }
}