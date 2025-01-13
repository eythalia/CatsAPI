using CatImageApi.Application.DTOs;
using CatImageApi.Application.Result;

namespace CatImageApi.Application.Interfaces
{
    public interface ICatService
    {
        Task<Result<CatAdditionResponseDTO>> FetchAndStoreCatsAsync();
        Task<Result<CatResponseDTO?>> GetCatByIdAsync(int id);
        Task<Result<PaginatedCatResponseDTO<CatResponseDTO>>> GetCatsAsync(int page, int pageSize, string? tag);
    }
}
