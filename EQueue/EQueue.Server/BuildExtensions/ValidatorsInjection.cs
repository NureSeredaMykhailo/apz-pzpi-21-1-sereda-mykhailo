using EQueue.Shared.Dto;
using EQueue.Shared.Validators;
using FluentValidation;

namespace EQueue.Server.BuildExtensions
{
    internal static class ValidatorsInjection
    {
        internal static void AddValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<Credentials>, CredentialsValidator>();
        }
    }
}
