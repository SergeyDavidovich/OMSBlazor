using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace OMSBlazor.Client.Services.DarkModeService
{
    public class ClientDarkModeService : IDarkModeService
    {
        private readonly HttpClient _httpClient;

        public ClientDarkModeService(NavigationManager navigationManager, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(navigationManager.BaseUri);
        }

        public async Task SetIsDarkMode(bool isDarkMode)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    isDarkMode
                }),
                Encoding.UTF8,
                "application/json");

            await _httpClient.PostAsync("api/darkMode", jsonContent);
        }

        public async Task<bool> GetIsDarkMode()
        {
            return await _httpClient.GetFromJsonAsync<bool>("api/darkMode");
        }
    }
}
