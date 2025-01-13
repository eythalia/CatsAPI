namespace CatImageApi.Domain.Database
{
    public class CatTag
    {
        public int CatsId { get; set; }
        public int TagsId { get; set; }
        public Cat Cat { get; set; } = new Cat();
        public Tag Tag { get; set; } = new Tag();
    }
}
