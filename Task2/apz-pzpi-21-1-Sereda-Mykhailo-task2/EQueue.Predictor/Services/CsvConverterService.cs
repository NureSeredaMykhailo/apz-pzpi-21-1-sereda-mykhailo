using EQueue.Predictor.Models;
using System.Text;

public class CsvConverterService
{
    public static byte[] ConvertDamagePredictToCsv(List<DamagePredictInput> models)
    {
        var csvBuilder = new StringBuilder();

        csvBuilder.AppendLine("ClinicId,Trauma0TypeId,Trauma0PlaceId,Trauma1TypeId,Trauma1PlaceId,Trauma3TypeId,Trauma3PlaceId,Trauma4TypeId,Trauma4PlaceId,TreatmentStartDelayMinutes,GotIncurableDamage");

        foreach (var model in models)
        {
            csvBuilder.AppendLine($"{model.ClinicId}," +
                                  $"{model.Trauma0TypeId},{model.Trauma0PlaceId}," +
                                  $"{model.Trauma1TypeId},{model.Trauma1PlaceId}," +
                                  $"{model.Trauma2TypeId},{model.Trauma2PlaceId}," +
                                  $"{model.Trauma3TypeId},{model.Trauma3PlaceId}," +
                                  $"{model.TreatmentStartDelayMinutes},{model.GotIncurableDamage}");
        }

        return Encoding.UTF8.GetBytes(csvBuilder.ToString());
    }

    public static byte[] ConvertDeathPredictToCsv(List<DeathPredictInput> models)
    {
        var csvBuilder = new StringBuilder();

        csvBuilder.AppendLine("ClinicId,Trauma0TypeId,Trauma0PlaceId,Trauma1TypeId,Trauma1PlaceId,Trauma3TypeId,Trauma3PlaceId,Trauma4TypeId,Trauma4PlaceId,TreatmentStartDelayMinutes,Survived");

        foreach (var model in models)
        {
            csvBuilder.AppendLine($"{model.ClinicId}," +
                                  $"{model.Trauma0TypeId},{model.Trauma0PlaceId}," +
                                  $"{model.Trauma1TypeId},{model.Trauma1PlaceId}," +
                                  $"{model.Trauma2TypeId},{model.Trauma2PlaceId}," +
                                  $"{model.Trauma3TypeId},{model.Trauma3PlaceId}," +
                                  $"{model.TreatmentStartDelayMinutes},{model.Survived}");
        }

        return Encoding.UTF8.GetBytes(csvBuilder.ToString());
    }
}
