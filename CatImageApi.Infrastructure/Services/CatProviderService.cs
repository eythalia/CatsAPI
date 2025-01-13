using CatImageApi.Application.Interfaces;
using CatImageApi.Domain.ExternalAPI;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace CatImageApi.Infrastructure.Services
{
    public class CatProviderService : ICatProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public CatProviderService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TheCatAPI:ApiKey"];
        }

        public async Task<List<CatAPI>> FetchCatsAsync(int limit)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://api.thecatapi.com/v1/images/search?limit={limit}&has_breeds=true");
            request.Headers.Add("x-api-key", _apiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<CatAPI>>(content) ?? new List<CatAPI>();
        }

        public async Task<byte[]> FetchCatImageAsync(string imageUrl)
        {
            var response = await _httpClient.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
