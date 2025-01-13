using System.ComponentModel.DataAnnotations;

namespace CatImageApi.API.DTOs
{
    public class GetCatsRequestDto
    {
        [Required(ErrorMessage = "Page is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
        public int page { get; set; }

        [Required(ErrorMessage = "PageSize is required.")]
        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
        public int pagesize { get; set; }

        [MaxLength(30, ErrorMessage = "Tag must be less than 30 characters.")]
        public string? tag { get; set; }
    }
}
