namespace CatImageApi.Domain.Database
{
    public class Cat
    {
        public int Id { get; set; }
        public string CatId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Image { get; set; }
        public DateTime? Created { get; set; } = DateTime.UtcNow;
        public ICollection<CatTag> CatTags { get; set; } = new List<CatTag>();
    }
}
