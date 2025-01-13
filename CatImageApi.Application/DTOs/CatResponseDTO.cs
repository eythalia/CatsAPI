

namespace CatImageApi.Application.DTOs
{
    public class CatResponseDTO
    {
        public int Id { get; set; }
        public string CatId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Image { get; set; }
        public DateTime? Created { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
