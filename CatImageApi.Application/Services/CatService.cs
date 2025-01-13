using CatImageApi.Application.Interfaces;
using CatImageApi.Application.Result;
using CatImageApi.Application.DTOs;
using CatImageApi.Domain.Database;
using CatImageApi.Domain.ExternalAPI;
using CatImageApi.Domain.Extensions;
using CatImageApi.Application.Mappeers;
using System.Data.Common;


namespace CatImageApi.Application.Services
{
    public class CatService : ICatService
    {
        private readonly ICatProviderService _apiRepository;
        private readonly IDbRepository _dbRepository;

        public CatService(ICatProviderService apiRepository, IDbRepository dbRepository)
        {
            _apiRepository = apiRepository;
            _dbRepository = dbRepository;
        }

        public async Task<Result<CatAdditionResponseDTO>> FetchAndStoreCatsAsync()
        {
            try
            {
                var apiCats = await _apiRepository.FetchCatsAsync(25);
                var catIds = apiCats.Select(c => c.CatId).ToList();
                var existingIds = await _dbRepository.GetExistingCatIdsAsync(catIds);
                var newCats = apiCats.Where(c => !existingIds.Contains(c.CatId)).ToList();
                
                if (!newCats.Any())
                    return Result<CatAdditionResponseDTO>.Failure("No cats found to add to the database.", 404);
                
                var allTemperaments = newCats
                    .SelectMany(cat => cat.Temperaments ?? Enumerable.Empty<string>())
                    .Select(t => t.Trim())
                    .Distinct()
                    .ToList();

                var tags = await _dbRepository.GetAndCreateTagsAsync(allTemperaments);
                (var catEntities,var failedCatsCnt) = await FetchCatEntitiesAsync(newCats, tags);

                if (catEntities.Any())
                {
                    await _dbRepository.AddCatsAsync(catEntities);
                    var response = Mapping.MapToCatAdditionResponse(catEntities,existingIds.Count,failedCatsCnt);
                    return Result<CatAdditionResponseDTO>.Success(response, 201);
                }

                return Result<CatAdditionResponseDTO>.Failure("Failed to store fetched cats.", 500);
            }
            catch (HttpRequestException ex)
            {
                return Result<CatAdditionResponseDTO>.Failure(ex.Message, (int)ex.StatusCode);
            }
            catch (DbException ex)
            {
                return Result<CatAdditionResponseDTO>.Failure(ex.Message, 500);
            }
            catch (Exception ex)
            {
                return Result<CatAdditionResponseDTO>.Failure(ex.Message, 500);
            }
            
        }

        
        public async Task<Result<CatResponseDTO?>> GetCatByIdAsync(int id)
        {
            try
            {
                var cat = await _dbRepository.GetCatByIdAsync(id);
                if (cat == null)
                    return Result<CatResponseDTO?>.Failure("Cat not found.", 404);

                var response = Mapping.MapToCatResponse(cat);
                
                return Result<CatResponseDTO?>.Success(response, 200);
            }
            catch (DbException ex) 
            {
                return Result<CatResponseDTO?>.Failure(ex.Message, 500);
            }
            catch (Exception ex)
            {
                return Result<CatResponseDTO?>.Failure(ex.Message, 500);
            }
            
        }

        public async Task<Result<PaginatedCatResponseDTO<CatResponseDTO>>> GetCatsAsync(int page, int pageSize, string? tag)
        {
            try
            {
                var response = new PaginatedCatResponseDTO<CatResponseDTO>();
                var totalItems = tag == null ? await _dbRepository.GetTotalCatCountAsync() : await _dbRepository.GetTotalCatCountByTagAsync(tag);

                if (totalItems == 0)
                    return Result<PaginatedCatResponseDTO<CatResponseDTO>>.Success(response, 200);

                if (totalItems <= page * pageSize - pageSize) 
                {
                    response = Mapping.MapToPaginatedCatResponse(new List<Cat>(), page, pageSize, totalItems);
                    return Result<PaginatedCatResponseDTO<CatResponseDTO>>.Success(response, 200);
                }
                 
                var cats = tag == null ? await _dbRepository.GetCatsWithPagingAsync(page, pageSize) : await _dbRepository.GetCatsByTagWithPagingAsync(tag, page, pageSize);
                response = Mapping.MapToPaginatedCatResponse(cats, page, pageSize, totalItems);
                
                return Result<PaginatedCatResponseDTO<CatResponseDTO>>.Success(response, 200);
            }
            catch (DbException ex)
            {
                return Result<PaginatedCatResponseDTO<CatResponseDTO>>.Failure(ex.Message, 500);
            }
            catch (Exception ex) 
            {
                return Result<PaginatedCatResponseDTO<CatResponseDTO>>.Failure(ex.Message, 500);
            }
           
        }

        private async Task<(List<Cat>,int)> FetchCatEntitiesAsync(IEnumerable<CatAPI> apiCats, List<Tag> tags)
        {
            List<string> failedCats = new List<string>();

            var tasks = apiCats.Select(async cat =>
            {
                try
                {
                    var imageUrl = cat.Url.ToString();
                    var imageBytes = await _apiRepository.FetchCatImageAsync(imageUrl);

                    var dbCat = new Cat
                    {
                        CatId = cat.CatId,
                        Width = cat.Width,
                        Height = cat.Height,
                        Image = imageBytes
                    };

                    dbCat.AddCatTags(cat, tags);

                    return dbCat;
                }
                catch
                {
                    failedCats.Add(cat.CatId);
                    return null; 
                }
            });

            
            return ((await Task.WhenAll(tasks)).Where(c => c != null).ToList(), failedCats.Count);
        }
    }
}
