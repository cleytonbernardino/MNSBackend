using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MMS.Application.UseCases.Auth.DoLogin;
using MMS.Application.UseCases.Auth.Logout;
using MMS.Application.UseCases.Auth.RefreshToken;
using MMS.Application.UseCases.Company.List;
using MMS.Application.UseCases.Company.ListUsers;
using MMS.Application.UseCases.Company.Register;
using MMS.Application.UseCases.User.Delete;
using MMS.Application.UseCases.User.Register;
using MMS.Application.UseCases.User.Update;
using MMS.Application.UseCases.User.Update.Password;
using MMS.Application.UseCases.WhatsApp.ReceiverJson;
using Sqids;

namespace MMS.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection service, IConfiguration configuration)
    {
        AddUseCase(service);
        AddIdEncoder(service, configuration);
    }

    private static void AddUseCase(IServiceCollection service)
    {
        service.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        service.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        service.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
        service.AddScoped<ILogoutUseCase, LogoutUseCase>();
        service.AddScoped<IListCompanyUsersUseCase, ListCompanyUsersUseCase>();
        service.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        service.AddScoped<IUpdateUserPasswordUseCase, UpdateUserPasswordUseCase>();
        service.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
        service.AddScoped<IReceiverJsonUseCase, ReceiverJsonUseCase>();
        service.AddScoped<IRegisterCompanyUseCase, RegisterCompanyUseCase>();
        service.AddScoped<IListCompaniesUseCase, ListCompaniesUseCase>();
    }

    private static void AddIdEncoder(IServiceCollection services,  IConfiguration configuration)
    {
        SqidsEncoder<long> sqids = new(new SqidsOptions()
        {
            MinLength = 3,
            Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });
        services.AddSingleton(sqids);
    }
}
