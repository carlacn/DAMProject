using DAMProject.Shared.Auth;
using System.Net.Http.Json;

namespace DAMProject.Client.Auth
{
    public interface IAuthService
    {
        Task<AuthenticationStatus> GetAuthenticationStatus();
        Task Logout();
    }

    public class AuthService(HttpClient httpClient) : IAuthService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<AuthenticationStatus> GetAuthenticationStatus()
        {
            var response = await _httpClient.GetAsync("api/auth/status");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthenticationStatus>();
            }

            return new AuthenticationStatus { IsAuthenticated = false };
        }
        public async Task Logout() 
        {
            await _httpClient.PostAsync("api/auth/logout", null);
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}