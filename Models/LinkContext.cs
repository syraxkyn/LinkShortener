using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Models
{
    public class LinkContext:DbContext
    {
        public LinkContext(DbContextOptions<LinkContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Link>()
                .HasIndex(u => u.ShortUrl)
                .IsUnique();
            builder.Entity<Link>()
                .HasIndex(u => u.LongUrl)
                .IsUnique();
        }
        public DbSet<Link> Links { get; set; }
    }
}
