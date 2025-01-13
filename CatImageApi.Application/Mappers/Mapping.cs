using CatImageApi.Application.DTOs;
using CatImageApi.Domain.Database;

namespace CatImageApi.Application.Mappeers
{
    public static class Mapping
    {
        public static CatResponseDTO MapToCatResponse(Cat cat) 
        {
            return new CatResponseDTO
            {
                Id = cat.Id,
                CatId = cat.CatId,
                Width = cat.Width,
                Height = cat.Height,
                Image = cat.Image,
                Created = cat.Created,
                Tags = cat.CatTags.Select(ct => ct.Tag.Name).ToList()
            };
        }

        public static PaginatedCatResponseDTO<CatResponseDTO> MapToPaginatedCatResponse(List<Cat> cats,int page, int pageSize, int totalItems) 
        {
            return new PaginatedCatResponseDTO<CatResponseDTO>
            {
                Items = cats.Select(cat => new CatResponseDTO
                {
                    Id = cat.Id,
                    CatId = cat.CatId,
                    Width = cat.Width,
                    Height = cat.Height,
                    Image = cat.Image,
                    Created = cat.Created,
                    Tags = cat.CatTags.Select(ct => ct.Tag.Name).ToList()
                }).ToList(),
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                TotalItems = totalItems
            };

        }

        public static CatAdditionResponseDTO MapToCatAdditionResponse(List<Cat> catEntities,int existingCats, int failedCats) 
        {
            return new CatAdditionResponseDTO
            {
                Succeed = catEntities.Select(t => t.Id.ToString()).ToList(),
                TotalSucceed = catEntities.Count,
                TotalExisting = existingCats,
                TotalFailed = failedCats
            };
        }
    }
}
