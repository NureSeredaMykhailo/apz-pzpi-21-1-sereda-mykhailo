using Blazored.LocalStorage;
using EQueue.Web.Services.Interfaces;
using EQueue.Shared.Dto;
using EQueue.Shared.ServiceResponseHandling;

namespace EQueue.Web.Security
{
    public class AuthenticationService
    {
        private readonly IHttpService _client;
        private readonly AuthStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(IHttpService client,
            AuthStateProvider authStateProvider,
            ILocalStorageService localStorage)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<ServiceResponse> LoginAsync(Credentials credentials)
        {
            var authResult = await _client.SendAsync<LoginResultDto>(HttpMethod.Post, "account/sign-in", credentials);

            if (authResult.IsSuccess is false)
            {
                return authResult;
            }

            await _localStorage.SetItemAsync("authToken", authResult.Result.Token);

            _authStateProvider.NotifyUserAuthentication(authResult.Result.Token);

            return ServiceResponseBuilder.Success();
        }

        public async Task<ServiceResponse> SignUpAsync(Credentials credentials)
        {
            var url = "account/create";
            var result = await _client.SendAsync(HttpMethod.Post, url, credentials);

            if (result.IsSuccess is false)
            {
                return result;
            }

            return ServiceResponseBuilder.Success();
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _authStateProvider.NotifyLogout();
        }
    }
}
