using EQueue.Predictor.Models;
using Microsoft.ML;

CreateDamagePredictModel();
CreateDeathPredictModel();

void CreateDamagePredictModel()
{
    var mlContext = new MLContext();

    IDataView dataView = mlContext.Data.LoadFromTextFile<DamagePredictInput>("damage-predict-input.csv", separatorChar: ',', hasHeader: true);

    var pipeline = mlContext.Transforms.Conversion.MapValueToKey(nameof(DamagePredictInput.Trauma0TypeId))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DamagePredictInput.Trauma0PlaceId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DamagePredictInput.Trauma1TypeId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DamagePredictInput.Trauma1PlaceId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DamagePredictInput.Trauma2TypeId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DamagePredictInput.Trauma2PlaceId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DamagePredictInput.Trauma3TypeId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DamagePredictInput.Trauma3PlaceId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DamagePredictInput.ClinicId)))

        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DamagePredictInput.Trauma0TypeId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DamagePredictInput.Trauma0PlaceId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DamagePredictInput.Trauma1TypeId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DamagePredictInput.Trauma1PlaceId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DamagePredictInput.Trauma2TypeId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DamagePredictInput.Trauma2PlaceId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DamagePredictInput.Trauma3TypeId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DamagePredictInput.Trauma3PlaceId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DamagePredictInput.ClinicId)))

        .Append(mlContext.Transforms.Concatenate("Features",
                                                 nameof(DamagePredictInput.Trauma0TypeId),
                                                 nameof(DamagePredictInput.Trauma0PlaceId),
                                                 nameof(DamagePredictInput.Trauma1TypeId),
                                                 nameof(DamagePredictInput.Trauma1PlaceId),
                                                 nameof(DamagePredictInput.Trauma2TypeId),
                                                 nameof(DamagePredictInput.Trauma2PlaceId),
                                                 nameof(DamagePredictInput.Trauma3TypeId),
                                                 nameof(DamagePredictInput.Trauma3PlaceId),
                                                 nameof(DamagePredictInput.TreatmentStartDelayMinutes),
                                                 nameof(DamagePredictInput.ClinicId)))

        .Append(mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: nameof(DamagePredictInput.GotIncurableDamage),
                                                                  featureColumnName: "Features"));


    var model = pipeline.Fit(dataView);

    string modelPath = "damagePredictModel.zip";
    mlContext.Model.Save(model, dataView.Schema, modelPath);
}

void CreateDeathPredictModel()
{
    var mlContext = new MLContext();

    IDataView dataView = mlContext.Data.LoadFromTextFile<DeathPredictInput>("death-predict-input.csv", separatorChar: ',', hasHeader: true);

    var pipeline = mlContext.Transforms.Conversion.MapValueToKey(nameof(DeathPredictInput.Trauma0TypeId))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DeathPredictInput.Trauma0PlaceId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DeathPredictInput.Trauma1TypeId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DeathPredictInput.Trauma1PlaceId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DeathPredictInput.Trauma2TypeId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DeathPredictInput.Trauma2PlaceId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DeathPredictInput.Trauma3TypeId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DeathPredictInput.Trauma3PlaceId)))
        .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(DeathPredictInput.ClinicId)))

        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DeathPredictInput.Trauma0TypeId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DeathPredictInput.Trauma0PlaceId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DeathPredictInput.Trauma1TypeId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DeathPredictInput.Trauma1PlaceId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DeathPredictInput.Trauma2TypeId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DeathPredictInput.Trauma2PlaceId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DeathPredictInput.Trauma3TypeId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DeathPredictInput.Trauma3PlaceId)))
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(nameof(DeathPredictInput.ClinicId)))

        .Append(mlContext.Transforms.Concatenate("Features",
                                                 nameof(DeathPredictInput.Trauma0TypeId),
                                                 nameof(DeathPredictInput.Trauma0PlaceId),
                                                 nameof(DeathPredictInput.Trauma1TypeId),
                                                 nameof(DeathPredictInput.Trauma1PlaceId),
                                                 nameof(DeathPredictInput.Trauma2TypeId),
                                                 nameof(DeathPredictInput.Trauma2PlaceId),
                                                 nameof(DeathPredictInput.Trauma3TypeId),
                                                 nameof(DeathPredictInput.Trauma3PlaceId),
                                                 nameof(DeathPredictInput.TreatmentStartDelayMinutes),
                                                 nameof(DeathPredictInput.ClinicId)))

        .Append(mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: nameof(DeathPredictInput.Survived),
                                                                  featureColumnName: "Features"));


    var model = pipeline.Fit(dataView);

    string modelPath = "deathPredictModel.zip";
    mlContext.Model.Save(model, dataView.Schema, modelPath);
}
