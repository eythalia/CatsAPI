using CatImageApi.Domain.Database;
using CatImageApi.Domain.ExternalAPI;


namespace CatImageApi.Domain.Extensions
{
    public static class CatExtensions
    {
        public static void AddCatTags(this Cat cat, CatAPI apiCat,List<Tag> tags)
        {
            var catTags = (apiCat.Temperaments ?? Enumerable.Empty<string>())
                         .SelectMany(temperament => tags
                         .Where(tag => string.Equals(temperament.Trim(), tag.Name, StringComparison.OrdinalIgnoreCase))
                         .Select(tag => new CatTag
                         {
                             Cat = cat,
                             Tag = tag
                         }))
                         .ToList();

            cat.CatTags = catTags;
        }
    }
}