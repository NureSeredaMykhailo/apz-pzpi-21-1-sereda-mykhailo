using System.Collections;
using System.Net.Http.Headers;
using System.Text;
using EQueue.Shared.ServiceResponseHandling;
using EQueue.Web.Services.Interfaces;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EQueue.Web.Services.Implementations;

public class HttpService : IHttpService
{
    private const int TimeoutSeconds = 5;

    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public HttpService(IConfiguration configuration)
    {
        _httpClient = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(TimeoutSeconds)
        };
        _baseUrl = configuration["Api"];
    }

    public void SetAuthorizationHeader(string bearerToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
    }

    public void RemoveAuthorizationHeader()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<ServiceResponse<T>> SendAsync<T>(HttpMethod method, string endpoint, object payload = default)
    {
        var url = ConstructUrl(method, endpoint, payload);
        var request = CreateRequest(method, url, payload);
        return await SendRequestAsync<T>(request);
    }

    public async Task<ServiceResponse> SendAsync(HttpMethod method, string endpoint, object payload = default)
    {
        var url = ConstructUrl(method, endpoint, payload);
        var request = CreateRequest(method, url, payload);
        return await SendRequestAsync(request);
    }

    private string ConstructUrl(HttpMethod method, string endpoint, object payload)
    {
        var url = $"{_baseUrl}{endpoint}";

        if ((method == HttpMethod.Get || method == HttpMethod.Delete) && payload != null)
        {
            if (payload is string)
            {
                url += $"/{payload}";
                return url;
            }
            if (payload is not Dictionary<string, object> parameters)
            {
                throw new Exception("Parameters for GET and DELETE method should be in Dictionary<string, object>");
            }

            url += CreateQueryString(parameters);
        }

        return url;
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string url, object payload)
    {
        var request = new HttpRequestMessage(method, url);

        if (method != HttpMethod.Get && method != HttpMethod.Delete)
        {
            var json = JsonConvert.SerializeObject(payload);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return request;
    }

    private async Task<ServiceResponse<T>> SendRequestAsync<T>(HttpRequestMessage request)
    {
        try
        {
            var httpResponseMessage = await _httpClient.SendAsync(request);            

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var json = await httpResponseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(json);
                return ServiceResponseBuilder.Success<T>(result);
            }

            var error = await httpResponseMessage.Content.ReadAsStringAsync();
            return ServiceResponseBuilder.Failure<T>(error);
        }
        catch (TaskCanceledException)
        {
            return ServiceResponseBuilder.Failure<T>("Connection timed out.");
        }
        catch (Exception ex)
        {
            return ServiceResponseBuilder.Failure<T>("Uknown error");
        }
    }

    private async Task<ServiceResponse> SendRequestAsync(HttpRequestMessage request)
    {
        return await SendRequestAsync<object>(request);
    }

    private static string CreateQueryString(Dictionary<string, object> parameters)
    {
        var builder = new StringBuilder("?");

        foreach (var pair in parameters)
        {
            if (pair.Value is IList list)
            {
                foreach (var item in list)
                {
                    builder.Append($"{pair.Key}={item}&");
                }
            }
            else
            {
                builder.Append($"{pair.Key}={pair.Value}&");
            }
        }

        builder.Length--;
        return builder.ToString();
    }
}
