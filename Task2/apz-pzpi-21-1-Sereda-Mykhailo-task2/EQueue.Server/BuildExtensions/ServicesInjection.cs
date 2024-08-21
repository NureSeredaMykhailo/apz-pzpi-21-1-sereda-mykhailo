using EQueue.Predictor.Services;
using EQueue.Server.Services.Implementations;
using EQueue.Server.Services.Interfaces;

namespace EQueue.Server.BuildExtensions
{
    internal static class ServicesInjection
    {
        internal static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenGenerator, JwtTokenGenerator>();
            services.AddTransient<IValidationService, ValidationService>();
            services.AddSingleton<PredictorService>();
        }
    }
}
