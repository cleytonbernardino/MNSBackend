using Microsoft.EntityFrameworkCore;
using MMS.Domain.Entities;

namespace MMS.Infrastructure.DataAccess;

public class MmsDbContext(
    DbContextOptions dbContextOptions
    ) : DbContext(dbContextOptions)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MmsDbContext).Assembly);
    }
}
