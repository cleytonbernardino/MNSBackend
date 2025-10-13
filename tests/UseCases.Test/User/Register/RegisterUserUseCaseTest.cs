using CommonTestUtilities.Cache;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using Microsoft.Extensions.Logging.Abstractions;
using MMS.Application.UseCases.User.Register;
using MMS.Domain.Enums;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestRegisterUseBuilder.Build();

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Success_Admin_User_Created()
    {
        var user = UserBuilder.Build();
        user.IsAdmin = true;
        
        var request = RequestRegisterUseBuilder.Build();
        request.Role = (short)UserRolesEnum.ADMIN;

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldNotThrowAsync();
    }
    
    [Fact]
    public async Task Error_User_Cannot_Create_An_Admin()
    {
        var user = UserBuilder.Build();
        
        var request = RequestRegisterUseBuilder.Build();
        request.Role = (short)UserRolesEnum.ADMIN;

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldThrowAsync<NoPermissionException>();
    }
    
    [Fact]
    public async Task Error_First_Name_Empty()
    {
        var user = UserBuilder.Build();

        var request = RequestRegisterUseBuilder.Build();
        request.FirstName = string.Empty;

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
        exception.ErrorMessages.Single().ShouldBe(ResourceMessagesException.FIRST_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Validator()
    {
        var user = UserBuilder.Build();
        var request = RequestRegisterUseBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        async Task act() => await useCase.Execute(request);

        var exception = await act().ShouldThrowAsync<ErrorOnValidationException>();
        exception.ErrorMessages.Single().ShouldBe(ResourceMessagesException.EMAIL_IN_USE);
    }

    [Fact]
    public async Task Error_Very_Low_Permission()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.RH;

        var request = RequestRegisterUseBuilder.Build();
        request.Role = (short)UserRolesEnum.MANAGER;

        var useCase = CreateUseCase(user);
        async Task act() => await useCase.Execute(request);

        await act().ShouldThrowAsync<NoPermissionException>();
    }

    [Fact]
    public async Task Error_Without_Permission()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.EMPLOYEE;

        var request = RequestRegisterUseBuilder.Build();
        request.Role = (short)UserRolesEnum.MANAGER;

        var useCase = CreateUseCase(user);
        async Task act() => await useCase.Execute(request);

        await act().ShouldThrowAsync<NoPermissionException>();
    }

    private static RegisterUserUseCase CreateUseCase(Entity.User user, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        UserReadOnlyRepositoryBuilder readOnlyRepository = new();
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var cacheService = new CacheServiceBuilder().Build();
        var passwordEncrypter = PasswordEncrypterBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var logger = NullLogger<RegisterUserUseCase>.Instance;

        if (!string.IsNullOrWhiteSpace(email))
            readOnlyRepository.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(
           loggedUser, readOnlyRepository.Build(), writeOnlyRepository, cacheService,
           passwordEncrypter, unitOfWork, logger
        );
    }
}
