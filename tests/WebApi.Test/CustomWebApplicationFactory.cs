using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MMS.Domain.Enums;
using MMS.Infrastructure.DataAccess;
using Entity = MMS.Domain.Entities;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    #region Standard Entities
    
    public Entity.User EmployeeUser { get; private set; } = null!;
    public Entity.User RHUser { get; private set; } = null!;
    public Entity.User SubManagerUser { get; private set; } = null!;
    public Entity.User ManagerUser { get; private set; } = null!;
    public Entity.User AdminUser { get; private set; } = null!;
    public string UserPassword { get; private set; } = string.Empty;
    public Entity.RefreshToken TokenRefresh { get; private set; } = null!;

    #endregion
    
    public T[] InjectInDatabase<T>(T[] entities) where T : Entity.EntityBase
    {
        using IServiceScope scope = Services.CreateScope();
        MmsDbContext dbContext = scope.ServiceProvider.GetRequiredService<MmsDbContext>();

        foreach (T entity in entities)
            entity.Id = 0;
        
        dbContext.AddRange(entities);

        dbContext.SaveChanges();
        return entities;
    }
    
    public T InjectInDatabase<T>(T entity) where T : Entity.EntityBase
    {
        T[] array = [entity];
        return InjectInDatabase(array)[0];
    }

    private void RegisterInitialUsers(MmsDbContext dbContext)
    {
        (ManagerUser, UserPassword) = UserBuilder.BuildWithPassword();

        EmployeeUser = UserBuilder.Build();
        EmployeeUser.Role = UserRolesEnum.EMPLOYEE;

        RHUser = UserBuilder.Build();
        RHUser.Role = UserRolesEnum.RH;

        SubManagerUser = UserBuilder.Build();
        SubManagerUser.Role = UserRolesEnum.SUB_MANAGER;

        AdminUser = UserBuilder.Build();
        AdminUser.IsAdmin = true;
        AdminUser.Role = UserRolesEnum.ADMIN;

        dbContext.Users.Add(EmployeeUser);
        dbContext.Users.Add(RHUser);
        dbContext.Users.Add(SubManagerUser);
        dbContext.Users.Add(ManagerUser);
        dbContext.Users.Add(AdminUser);
    }

    private void RegisterRefreshTokenFor(Entity.User user, MmsDbContext dbContext)
    {
        Entity.RefreshToken token = new()
        {
            UserIdentifier = user.UserIdentifier,
            ExpiryDate = DateTime.UtcNow.AddDays(1),
            Token = new Random().Next().ToString()
        };
        TokenRefresh = token;
        dbContext.RefreshTokens.Add(token);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                ServiceDescriptor? descriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MmsDbContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);

                ServiceProvider provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<MmsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                using IServiceScope scope = services.BuildServiceProvider().CreateScope();
                MmsDbContext dbContext = scope.ServiceProvider.GetRequiredService<MmsDbContext>();
                dbContext.Database.EnsureDeleted();

                RegisterInitialUsers(dbContext);
                RegisterRefreshTokenFor(ManagerUser, dbContext);
                dbContext.SaveChanges();
            });
    }
}
