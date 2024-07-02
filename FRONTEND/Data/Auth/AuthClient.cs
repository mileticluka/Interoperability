using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using DAL.Model;

namespace FRONTEND.Data.Auth
{
    public class AuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthClient(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<TokenResponse> LoginAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5181/api/User/login", user);
            var tokenResponse = new TokenResponse();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                    tokenResponse.isSuccess = true;
                }
                catch (JsonException)
                {
                    tokenResponse.isSuccess = false;
                    tokenResponse.Token = null;
                }
            }
            else
            {
                tokenResponse.isSuccess = false;
                tokenResponse.Token = null;
            }

            return tokenResponse;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5181/api/User/register", user);
            return response.IsSuccessStatusCode;
        }

        public class TokenResponse
        {
            public bool isSuccess { get; set; }
            public string Token { get; set; }
        }
    }
}
