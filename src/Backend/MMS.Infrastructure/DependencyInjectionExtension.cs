using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MMS.Application.Services.Encoders;
using MMS.Application.Services.MessageQueue;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.Company;
using MMS.Domain.Repositories.Token;
using MMS.Domain.Repositories.User;
using MMS.Domain.Security.Cryptography;
using MMS.Domain.Security.Token;
using MMS.Domain.Services.LoggedUser;
using MMS.Infrastructure.DataAccess;
using MMS.Infrastructure.DataAccess.Repositories;
using MMS.Infrastructure.Extensions;
using MMS.Infrastructure.Security.Cryptography;
using MMS.Infrastructure.Security.Token.Access.Generate;
using MMS.Infrastructure.Security.Token.Access.Validator;
using MMS.Infrastructure.Security.Token.Refresh;
using MMS.Infrastructure.Services.LoggedUser;
using MMS.Infrastructure.Services.MessageQueue;
using System.Reflection;

namespace MMS.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        AddRepositories(service);
        AddEncryptors(service);
        AddJwtToken(service, configuration);
        //AddQueue(service);
        AddLoggedUser(service);

        if (configuration.IsUnitTestEnvironment())
            return;

        AddDbContext(service, configuration);
        AddFluentMigrator(service, configuration);
    }

    private static void AddRepositories(IServiceCollection service)
    {
        service.AddScoped<IUserReadOnlyRepository, UserRepository>();
        service.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        service.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        service.AddScoped<ICompanyWriteOnlyRepository, CompanyRepository>();
        service.AddScoped<ICompanyReadOnlyRepository, CompanyRepository>();
        service.AddScoped<IRefreshTokenRepository, TokenRepository>();
        service.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddEncryptors(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncrypter, Argon2Encripter>();
        services.AddScoped<IIdEncoder, SqidsIdEncoder>();
    }

    private static void AddJwtToken(IServiceCollection services, IConfiguration configuration)
    {
        string signingKey = configuration.GetValue<string>("Settings:JWT:SigningKey")!;
        string issueKey = configuration.GetValue<string>("Settings:JWT:Issuer")!;
        uint expirationTimeMinutes = configuration.GetValue<uint>("Settings:JWT:ExpiryInMinutes")!;
        uint refreshTokenExpiryDate = configuration.GetValue<uint>("Settings:JWT:RefreshTokenExpiryInDays");

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => options.TokenValidationParameters = JwtValidationParaments.ConfigureParameters(signingKey, issueKey));

        services.AddScoped<IRefreshTokenHandler>(options => 
            new JwtTokenRefreshHandler(
                refreshTokenExpiryDate,
                options.GetRequiredService<IAccessTokenValidator>(),
                options.GetRequiredService<IRefreshTokenRepository>(),
                options.GetRequiredService<IUnitOfWork>()
                ));
        services.AddScoped<IAccessTokenGenerator>(_ => 
            new JwtTokenGenerator(expirationTimeMinutes, signingKey, issueKey)
        );
        services.AddScoped<IAccessTokenValidator>(_ => 
            new JwtValidationParaments(signingKey, issueKey)
        );
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.ConnectionString();

        services.AddDbContext<MmsDbContext>(option =>
           option.UseMySQL(connectionString)
       );
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
            options
            .AddMySql5()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.Load("MMS.Infrastructure")).For.All()
        );
    }

    private static void AddQueue(IServiceCollection services) => services.AddSingleton<IMessageQueue, MessageQueue>();

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();
}
