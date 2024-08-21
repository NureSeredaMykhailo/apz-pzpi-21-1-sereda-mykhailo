using EQueue.Predictor.Models;
using Microsoft.ML;

namespace EQueue.Predictor.Services
{
    internal class DeathPredictorService
    {
        private readonly PredictionEngine<DeathPredictInput, DeathPredictOutput> _predictionEngine;

        public DeathPredictorService(string modelPath)
        {
            var mlContext = new MLContext();

            ITransformer loadedModel;
            DataViewSchema inputSchema;

            using (var stream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                loadedModel = mlContext.Model.Load(stream, out inputSchema);
            }

            _predictionEngine = mlContext.Model.CreatePredictionEngine<DeathPredictInput, DeathPredictOutput>(loadedModel);
        }

        public DeathPredictOutput Predict(DeathPredictInput data)
        {
            return _predictionEngine.Predict(data);
        }
    }
}
