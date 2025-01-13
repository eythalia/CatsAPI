using Microsoft.EntityFrameworkCore;
using CatImageApi.Domain.Database;

namespace CatImageApi.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Cat> Cats { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CatTag>(entity =>
            {
                entity.HasKey(ct => new { ct.CatsId, ct.TagsId });

                entity.HasOne(ct => ct.Cat)
                      .WithMany(c => c.CatTags)
                      .HasForeignKey(ct => ct.CatsId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ct => ct.Tag)
                      .WithMany(t => t.CatTags)
                      .HasForeignKey(ct => ct.TagsId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
