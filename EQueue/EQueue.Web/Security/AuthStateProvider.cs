using Blazored.LocalStorage;
using EQueue.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace EQueue.Web.Security
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private static AuthenticationState s_anonymous
            = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        private readonly ILocalStorageService _localStorage;

        private readonly IHttpService _client;

        public AuthStateProvider(ILocalStorageService localStorage, IHttpService client)
        {
            _localStorage = localStorage;
            _client = client;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return s_anonymous;
            }

            _client.SetAuthorizationHeader(token);

            return ConstructAuthenticationState(token);
        }

        public void NotifyUserAuthentication(string token)
        {
            _client.SetAuthorizationHeader(token);
            var state = ConstructAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
        }

        private AuthenticationState ConstructAuthenticationState(string token)
        {
            var claims = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims;
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwtAuthType"));
            return new AuthenticationState(claimsPrincipal);
        }

        public void NotifyLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(s_anonymous));
            _client.RemoveAuthorizationHeader();
        }
    }
}
