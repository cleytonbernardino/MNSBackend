using Microsoft.EntityFrameworkCore;
using MMS.Domain.Entities;
using System.Text.Json;

namespace MMS.Infrastructure.DataAccess;

public class MmsDbContext(
    DbContextOptions dbContextOptions
    ) : DbContext(dbContextOptions)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<SubscriptionsPlan> SubscriptionsPlans { get; set; } 
    public DbSet<CompanySubscription> CompanySubscriptions { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ServiceDefinition> ServiceDefinitions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MmsDbContext).Assembly);
        modelBuilder.Entity<Company>()
            .HasOne(company => company.Manager)
            .WithOne()
            .HasForeignKey<Company>(company => company.ManagerId);
        modelBuilder.Entity<Company>()
            .Property(entity => entity.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<User>()
            .Property(entity => entity.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<SubscriptionsPlan>()
            .Property(plan => plan.Properties)
            .HasConversion(
                properties => JsonSerializer.Serialize(properties, (JsonSerializerOptions)null),
                properties => JsonSerializer.Deserialize<List<string>>(properties, (JsonSerializerOptions)null));
    }
}
