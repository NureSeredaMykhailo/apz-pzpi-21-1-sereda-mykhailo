using EQueue.Predictor.Models;

namespace EQueue.Predictor.Services
{
    public class PredictorService
    {
        private readonly DamagePredictorService _damagePredictorService;
        private readonly DeathPredictorService _deathPredictorService;

        public PredictorService()
        {
            _damagePredictorService = new DamagePredictorService("damagePredictModel.zip");
            _deathPredictorService = new DeathPredictorService("deathPredictModel.zip");
        }

        public PredictOutput Predict(DamagePredictInput damageInput, DeathPredictInput deathInput)
        {
            var damageOutput = _damagePredictorService.Predict(damageInput);
            var deathOutput = _deathPredictorService.Predict(deathInput);
            return new PredictOutput()
            {
                DamageProbability = damageOutput.Probability,
                GotIncurableDamage = damageOutput.GotIncurableDamage,
                Survived = deathOutput.Survived,
                SurvivedProbability = deathOutput.Probability,
                TreatmentStartDelayMinutes = damageInput.TreatmentStartDelayMinutes
            };
        }
    }
}
