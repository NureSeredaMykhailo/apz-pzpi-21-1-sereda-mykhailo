using Microsoft.ML.Data;

namespace EQueue.Predictor.Models
{
    public class DeathPredictInput
    {
        [LoadColumn(0)]
        public long ClinicId { get; set; }

        [LoadColumn(1)]
        public long Trauma0TypeId { get; set; }

        [LoadColumn(2)]
        public long Trauma0PlaceId { get; set; }

        [LoadColumn(3)]
        public long Trauma1TypeId { get; set; }

        [LoadColumn(4)]
        public long Trauma1PlaceId { get; set; }

        [LoadColumn(5)]
        public long Trauma2TypeId { get; set; }

        [LoadColumn(6)]
        public long Trauma2PlaceId { get; set; }

        [LoadColumn(7)]
        public long Trauma3TypeId { get; set; }

        [LoadColumn(8)]
        public long Trauma3PlaceId { get; set; }

        [LoadColumn(9)]
        public float TreatmentStartDelayMinutes { get; set; }

        [LoadColumn(10)]
        public bool Survived { get; set; }
    }
}
