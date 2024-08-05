using EQueue.Shared.ServiceResponseHandling;

namespace EQueue.Server.Services.Interfaces
{
    public interface IValidationService
    {
        public Task<ServiceResponse> ValidateAsync<T>(T item);
    }
}
