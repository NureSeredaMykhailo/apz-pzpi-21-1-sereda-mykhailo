using Microsoft.ML.Data;

namespace EQueue.Predictor.Models
{
    public class DeathPredictOutput
    {
        [ColumnName("PredictedLabel")]
        public bool Survived { get; set; }
        public float Probability { get; set; }
    }
}
