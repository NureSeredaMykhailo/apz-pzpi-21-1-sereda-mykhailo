namespace EQueue.Predictor.Models
{
    public class PredictOutput
    {
        public bool GotIncurableDamage { get; set; }
        public float DamageProbability { get; set; }

        public bool Survived { get; set; }
        public float SurvivedProbability { get; set; }

        public float TreatmentStartDelayMinutes { get; set; }
    }
}
