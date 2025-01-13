using CatImageApi.Domain.Database;

namespace CatImageApi.Application.Interfaces
{
    public interface IDbRepository
    {
        Task<List<string>> GetExistingCatIdsAsync(IEnumerable<string> catIds);
        Task AddCatsAsync(IEnumerable<Cat> cats);
        Task<Cat?> GetCatByIdAsync(int id);
        Task<List<Tag>> GetAndCreateTagsAsync(IEnumerable<string> tagNames);
        Task<List<Cat>> GetCatsWithPagingAsync(int page, int pageSize);
        Task<int> GetTotalCatCountAsync();
        Task<List<Cat>> GetCatsByTagWithPagingAsync(string tag, int page, int pageSize);
        Task<int> GetTotalCatCountByTagAsync(string tag);
    }
}
