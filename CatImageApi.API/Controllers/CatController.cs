using Microsoft.AspNetCore.Mvc;
using CatImageApi.Application.Interfaces;
using CatImageApi.API.DTOs;
using CatImageApi.Application.DTOs;

namespace CatImageApi.API.Controllers
{
    [Route("api/cats")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly ICatService _catService;

        public CatController(ICatService catService)
        {
            _catService = catService;
        }

        [HttpPost("fetch")]
        [ProducesResponseType(typeof(CatAdditionResponseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> FetchCats()
        {
            var result = await _catService.FetchAndStoreCatsAsync();
            Object responseObj = result.IsSuccess ? new { data = result.Value } : new { error = result.Error };

            return StatusCode(result.StatusCode, responseObj);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CatResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCat(int id)
        {
            if (id <= 0)
                return BadRequest(new { error = "The id must be greater than 0." });

            var result = await _catService.GetCatByIdAsync(id);
            Object responseObj = result.IsSuccess ? new { data = result.Value } : new { error = result.Error };

            return StatusCode(result.StatusCode, responseObj);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedCatResponseDTO<CatResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCats([FromQuery] GetCatsRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
        
            var result = await _catService.GetCatsAsync(request.page, request.pagesize, request.tag);
            Object responseObj = result.IsSuccess ? new { data = result.Value } : new { error = result.Error };

            return StatusCode(result.StatusCode, responseObj);
        }
    }
}
