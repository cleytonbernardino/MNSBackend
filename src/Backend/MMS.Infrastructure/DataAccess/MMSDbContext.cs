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
        modelBuilder.Entity<Company>()
            .HasOne(company => company.Manager)
            .WithOne()
            .HasForeignKey<Company>(company => company.ManagerId);
        modelBuilder.Entity<EntityBase>()
            .Property(entity => entity.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<EntityBase<short>>()
            .Property(entity => entity.Id)
            .ValueGeneratedOnAdd();
    }
}
