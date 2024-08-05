using EQueue.Shared.ServiceResponseHandling;

namespace EQueue.Web.Services.Interfaces;

public interface IHttpService
{
    void SetAuthorizationHeader(string bearerToken);
    void RemoveAuthorizationHeader();
    Task<ServiceResponse<T>> SendAsync<T>(HttpMethod method, string endpoint, object payload = default);
    Task<ServiceResponse> SendAsync(HttpMethod method, string endpoint, object payload = default);
}