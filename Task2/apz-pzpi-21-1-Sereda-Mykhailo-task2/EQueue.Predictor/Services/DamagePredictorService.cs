using EQueue.Predictor.Models;
using Microsoft.ML;

namespace EQueue.Predictor.Services
{
    internal class DamagePredictorService
    {
        private readonly PredictionEngine<DamagePredictInput, DamagePredictOutput> _predictionEngine;

        public DamagePredictorService(string modelPath)
        {
            var mlContext = new MLContext();

            ITransformer loadedModel;
            DataViewSchema inputSchema;

            using (var stream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                loadedModel = mlContext.Model.Load(stream, out inputSchema);
            }

            _predictionEngine = mlContext.Model.CreatePredictionEngine<DamagePredictInput, DamagePredictOutput>(loadedModel);
        }

        public DamagePredictOutput Predict(DamagePredictInput data)
        {
            return _predictionEngine.Predict(data);
        }
    }
}
