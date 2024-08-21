using Microsoft.ML.Data;

namespace EQueue.Predictor.Models
{
    public class DamagePredictOutput
    {
        [ColumnName("PredictedLabel")]
        public bool GotIncurableDamage { get; set; }
        public float Probability { get; set; }
    }
}
