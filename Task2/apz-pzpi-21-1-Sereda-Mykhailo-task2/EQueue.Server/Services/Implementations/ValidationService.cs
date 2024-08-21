using EQueue.Server.Services.Interfaces;
using FluentValidation;
using EQueue.Shared.ServiceResponseHandling;

namespace EQueue.Server.Services.Implementations
{
    public class ValidationService : IValidationService
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ServiceResponse> ValidateAsync<T>(T item)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if (validator is null)
            {
                return ServiceResponseBuilder.Success();
            }

            var validationContext = new ValidationContext<T>(item);
            var validationResult = await validator.ValidateAsync(validationContext);

            return validationResult.IsValid ? ServiceResponseBuilder.Success() : ServiceResponseBuilder.Failure(validationResult.Errors[0].ErrorMessage);
        }
    }
}
