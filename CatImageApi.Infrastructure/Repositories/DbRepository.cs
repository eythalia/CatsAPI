using CatImageApi.Application.Interfaces;
using CatImageApi.Domain.Database;
using CatImageApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CatImageApi.Infrastructure.Repositories
{
    public class DbRepository : IDbRepository
    {
        private readonly AppDbContext _context;

        public DbRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetExistingCatIdsAsync(IEnumerable<string> catIds)
        {
            return await _context.Cats
                .Where(c => catIds.Contains(c.CatId))
                .Select(c => c.CatId)
                .ToListAsync();
        }

        public async Task<List<Tag>> GetAndCreateTagsAsync(IEnumerable<string> tagNames)
        {
            if (!tagNames.Any())
            {
                return new List<Tag>();
            }

            var lowerTagNames = tagNames.Select(tag => tag.ToLower()).Distinct().ToList();

            var existingTags = await _context.Tags
                .Where(t => lowerTagNames.Contains(t.Name.ToLower()))
                .ToListAsync();

            var newTags = tagNames
                .Select(tag => tag.ToLower())
                .Except(existingTags.Select(t => t.Name))
                .Select(name => new Tag
                {
                    Name = name
                })
                .ToList();

            if (newTags.Any())
            {
                _context.Tags.AddRange(newTags);
                await _context.SaveChangesAsync();
            }

            return existingTags.Concat(newTags).ToList();
        }

        public async Task AddCatsAsync(IEnumerable<Cat> cats)
        {
            _context.Cats.AddRange(cats);
            await _context.SaveChangesAsync();
        }

        public async Task<Cat?> GetCatByIdAsync(int id)
        {
            return await _context.Cats
                .Include(c => c.CatTags)
                .ThenInclude(ct => ct.Tag)
                .FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task<List<Cat>> GetCatsWithPagingAsync(int page, int pageSize)
        {
            return await _context.Cats
                .Include(c => c.CatTags)
                .ThenInclude(ct => ct.Tag)
                .OrderBy(c => c.Id) 
                .Skip((page - 1) * pageSize) 
                .Take(pageSize) 
                .ToListAsync();
        }

        public async Task<int> GetTotalCatCountAsync()
        {
            return await _context.Cats.CountAsync();
        }

        public async Task<List<Cat>> GetCatsByTagWithPagingAsync(string tag, int page, int pageSize)
        {
            return await _context.Cats
                .Include(c => c.CatTags)
                .ThenInclude(ct => ct.Tag)
                .Where(c => c.CatTags.Any(ct => ct.Tag.Name == tag))
                .OrderBy(c => c.Id) 
                .Skip((page - 1) * pageSize) 
                .Take(pageSize) 
                .ToListAsync();
        }

        public async Task<int> GetTotalCatCountByTagAsync(string tag)
        {
            return await _context.Cats
                .Where(c => c.CatTags.Any(ct => ct.Tag.Name == tag)) 
                .CountAsync();
        }
    }
}
