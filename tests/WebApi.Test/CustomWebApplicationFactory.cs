using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MMS.Domain.Enums;
using MMS.Infrastructure.DataAccess;
using System.Security.Cryptography;
using Entity = MMS.Domain.Entities;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Entity.User EmployeeUser { get; private set; } 
    public Entity.User RHUser { get; private set; } 
    public Entity.User SubManagerUser { get; private set; } 
    public Entity.User ManagerUser { get; private set; }
    public Entity.User AdminUser { get; private set; }

    public int UserInDataBase { get; private set; } = 1;

    // Only manager User
    public string UserPassword { get; private set; } = string.Empty;
    public Entity.RefreshToken TokenRefresh { get; private set; } = null!;

    public Entity.User InjectUser(Entity.User user)
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MmsDbContext>();

        user.Id = ++UserInDataBase;
        dbContext.Add(user);
        dbContext.SaveChanges();
        return user;
    }

    private void RegisterInitialUsers(MmsDbContext dbContext)
    {
        (ManagerUser, UserPassword) = UserBuilder.BuildWithPassword();

        EmployeeUser = UserBuilder.Build();
        EmployeeUser.Id = ++UserInDataBase;
        EmployeeUser.Role = UserRolesEnum.EMPLOYEE;
        
        RHUser = UserBuilder.Build();
        RHUser.Id = ++UserInDataBase;
        RHUser.Role = UserRolesEnum.RH;
        
        SubManagerUser = UserBuilder.Build();
        SubManagerUser.Id = ++UserInDataBase;
        SubManagerUser.Role = UserRolesEnum.SUB_MANAGER;
        
        AdminUser = UserBuilder.Build();
        AdminUser.Id = ++UserInDataBase;
        AdminUser.IsAdmin = true;
        AdminUser.Role = UserRolesEnum.ADMIN;

        dbContext.Add(EmployeeUser);
        dbContext.Add(RHUser);
        dbContext.Add(SubManagerUser);
        dbContext.Add(ManagerUser);
        dbContext.Add(AdminUser);

        RegisterRefreshTokenFor(ManagerUser, dbContext);
        
        dbContext.SaveChanges();
    }
    
    private void RegisterRefreshTokenFor(Entity.User user, MmsDbContext dbContext)
    {
        Entity.RefreshToken token = new()
        {
            UserIdentifier = user.UserIdentifier,
            ExpiryDate = DateTime.UtcNow.AddDays(1),
            Token = RandomNumberGenerator.Create().ToString()
        };
        TokenRefresh = token;
        dbContext.RefreshTokens.Add(token);
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MmsDbContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<MmsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<MmsDbContext>();
                dbContext.Database.EnsureDeleted();

                RegisterInitialUsers(dbContext);
            });
    }
}
