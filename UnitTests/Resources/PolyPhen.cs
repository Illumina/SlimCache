using VariantAnnotation.Caches.DataStructures;

namespace UnitTests.Resources
{
    public static class PolyPhen
    {
        public static readonly Prediction.Entry[] Lut;
        public static readonly Prediction[][]     PredictionsPerRef;

        private static readonly Prediction NM_000546;
        private static readonly Prediction ENST00000610292;
        private static readonly Prediction NM_002524;
        private static readonly Prediction Unused;

        static PolyPhen()
        {
            const int numAminoAcids = 20;
            Lut = GetLut();

            var data = new byte[numAminoAcids * 4];
            data[PredictionIoTests.GetIndex('A', 1)] = 3;
            data[PredictionIoTests.GetIndex('C', 2)] = 2;
            data[PredictionIoTests.GetIndex('G', 3)] = 1;
            data[PredictionIoTests.GetIndex('T', 4)] = 0;

            var data2 = new byte[numAminoAcids * 4];
            data2[PredictionIoTests.GetIndex('R', 1)] = 0;
            data2[PredictionIoTests.GetIndex('Y', 2)] = 1;
            data2[PredictionIoTests.GetIndex('S', 3)] = 2;
            data2[PredictionIoTests.GetIndex('W', 4)] = 3;

            var data3 = new byte[numAminoAcids * 4];
            data3[PredictionIoTests.GetIndex('K', 1)] = 3;
            data3[PredictionIoTests.GetIndex('M', 2)] = 2;
            data3[PredictionIoTests.GetIndex('B', 3)] = 1;
            data3[PredictionIoTests.GetIndex('D', 4)] = 0;
            
            var data4 = new byte[numAminoAcids * 4];
            data4[PredictionIoTests.GetIndex('T', 1)] = 1;
            data4[PredictionIoTests.GetIndex('V', 2)] = 0;
            data4[PredictionIoTests.GetIndex('W', 3)] = 1;
            data4[PredictionIoTests.GetIndex('Y', 4)] = 3;

            NM_000546       = new Prediction(data,  Lut);
            ENST00000610292 = new Prediction(data2, Lut);
            NM_002524       = new Prediction(data3, Lut);
            Unused          = new Prediction(data4, Lut);

            PredictionsPerRef    = new Prediction[3][];
            PredictionsPerRef[0] = new[] {NM_002524, Unused};
            PredictionsPerRef[2] = new[] {ENST00000610292, NM_000546};
        }

        private static Prediction.Entry[] GetLut()
        {
            var entry  = new Prediction.Entry(0.9, 0); // tolerated
            var entry2 = new Prediction.Entry(0.1, 1); // deleterious
            var entry3 = new Prediction.Entry(0.5, 2); // tolerated - low confidence
            var entry4 = new Prediction.Entry(0.4, 3); // deleterious - low confidence

            return new[] {entry, entry2, entry3, entry4};
        }
    }
}