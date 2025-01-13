namespace CatImageApi.Domain.Database
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Created { get; set; } = DateTime.UtcNow;
        public ICollection<CatTag> CatTags { get; set; } = new List<CatTag>();
    }
}
