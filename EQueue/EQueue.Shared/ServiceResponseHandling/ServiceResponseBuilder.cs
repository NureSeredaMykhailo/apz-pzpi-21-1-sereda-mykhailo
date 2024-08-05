namespace EQueue.Shared.ServiceResponseHandling
{
    public class ServiceResponseBuilder
    {
        public static ServiceResponse Success() => new ServiceResponse { IsSuccess = true };

        public static ServiceResponse Failure(string errorMessage)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }

        public static ServiceResponse<TResult> Success<TResult>(TResult result)
        {
            return new ServiceResponse<TResult>
            {
                IsSuccess = true,
                Result = result
            };
        }

        public static ServiceResponse<TResult> Failure<TResult>(string errorMessage)
        {
            return new ServiceResponse<TResult>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
