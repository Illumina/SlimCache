using VariantAnnotation.Caches.DataStructures;

namespace UnitTests.Resources
{
    public static class SIFT
    {
        public static readonly Prediction.Entry[] Lut;
        public static readonly Prediction[][]     PredictionsPerRef;

        public static readonly Prediction NM_000546;
        public static readonly Prediction ENST00000610292;
        public static readonly Prediction NM_033360;
        public static readonly Prediction Unused;

        static SIFT()
        {
            const int numAminoAcids = 20;
            Lut = GetLut();

            var data = new byte[numAminoAcids * 4];
            data[PredictionIoTests.GetIndex('A', 1)] = 3;
            data[PredictionIoTests.GetIndex('C', 2)] = 2;
            data[PredictionIoTests.GetIndex('M', 3)] = 1;
            data[PredictionIoTests.GetIndex('Y', 4)] = 0;

            var data2 = new byte[numAminoAcids * 4];
            data2[PredictionIoTests.GetIndex('C', 1)] = 0;
            data2[PredictionIoTests.GetIndex('A', 2)] = 1;
            data2[PredictionIoTests.GetIndex('Y', 3)] = 2;
            data2[PredictionIoTests.GetIndex('M', 4)] = 3;

            var data3 = new byte[numAminoAcids * 4];
            data3[PredictionIoTests.GetIndex('G', 1)] = 3;
            data3[PredictionIoTests.GetIndex('R', 2)] = 2;
            data3[PredictionIoTests.GetIndex('S', 3)] = 1;
            data3[PredictionIoTests.GetIndex('A', 4)] = 0;
            
            var data4 = new byte[numAminoAcids * 4];
            data4[PredictionIoTests.GetIndex('M', 1)] = 0;
            data4[PredictionIoTests.GetIndex('B', 2)] = 2;
            data4[PredictionIoTests.GetIndex('D', 3)] = 2;
            data4[PredictionIoTests.GetIndex('H', 4)] = 1;

            NM_000546       = new Prediction(data,  Lut);
            ENST00000610292 = new Prediction(data2, Lut);
            NM_033360       = new Prediction(data3, Lut);
            Unused          = new Prediction(data4, Lut);
            
            PredictionsPerRef    = new Prediction[3][];
            PredictionsPerRef[1] = new[] {NM_033360};
            PredictionsPerRef[2] = new[] {Unused, NM_000546, ENST00000610292};
        }

        private static Prediction.Entry[] GetLut()
        {
            var entry  = new Prediction.Entry(1.0, 0); // tolerated
            var entry2 = new Prediction.Entry(0.0, 1); // deleterious
            var entry3 = new Prediction.Entry(0.6, 2); // tolerated - low confidence
            var entry4 = new Prediction.Entry(0.3, 3); // deleterious - low confidence

            return new[] {entry, entry2, entry3, entry4};
        }
    }
}