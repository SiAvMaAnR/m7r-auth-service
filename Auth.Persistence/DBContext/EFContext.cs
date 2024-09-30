using Auth.Domain.Entities.RefreshTokens;
using Auth.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Auth.Persistence.DBContext;

public class EFContext : DbContext
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public EFContext(DbContextOptions<EFContext> options)
        : base(options)
    {
        // Database.EnsureDeleted();
        // Database.EnsureCreated();
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
}
