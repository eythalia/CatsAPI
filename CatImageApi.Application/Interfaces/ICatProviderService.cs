using CatImageApi.Domain.ExternalAPI;

namespace CatImageApi.Application.Interfaces
{
    public interface ICatProviderService
    {
        Task<List<CatAPI>> FetchCatsAsync(int limit = 25);
        Task<byte[]> FetchCatImageAsync(string imageUrl);
    }
}
