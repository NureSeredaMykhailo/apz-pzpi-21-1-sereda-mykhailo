namespace EQueue.Shared.ServiceResponseHandling
{
    public class ServiceResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public ServiceResponse MapErrorResult()
        {
            return new ServiceResponse()
            {
                IsSuccess = false,
                ErrorMessage = ErrorMessage
            };
        }

        public ServiceResponse<TDestinationResult> MapErrorResult<TDestinationResult>()
        {
            return new ServiceResponse<TDestinationResult>()
            {
                IsSuccess = false,
                ErrorMessage = ErrorMessage
            };
        }
    }

    public class ServiceResponse<TResult> : ServiceResponse
    {
        public TResult Result { get; set; }
    }
}
