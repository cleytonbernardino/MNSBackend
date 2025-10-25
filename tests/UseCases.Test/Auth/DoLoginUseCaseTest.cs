using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Tokens;
using MMS.Application.UseCases.Auth.DoLogin;
using MMS.Communication.Requests.Auth;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.Auth;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var request = new RequestDoLogin
        {
            Email = user.Email,
            Password = user.Password
        };

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Invalid_Email_Credentials()
    {
        var user = UserBuilder.Build();

        var request = new RequestDoLogin
        {
            Email = "tes@gmail.com",
            Password = user.Password
        };

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldThrowAsync<InvalidLoginException>();
    }

    [Fact]
    public async Task Error_Invalid_Password_Credentials()
    {
        var user = UserBuilder.Build();

        var request = new RequestDoLogin
        {
            Email = user.Email,
            Password = $"{user.Password}123"
        };

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldThrowAsync<InvalidLoginException>();
    }

    [Fact]
    public async Task Error_Invalid_Email()
    {
        var user = UserBuilder.Build();

        var request = new RequestDoLogin
        {
            Email = "Fake",
            Password = user.Password
        };

        var useCase = CreateUseCase(user);

        async Task act() => await useCase.Execute(request);

        await act().ShouldThrowAsync<ErrorOnValidationException>();
    }

    private static DoLoginUseCase CreateUseCase(Entity.User user)
    {
        var repository = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = new RefreshTokenHandlerBuilder().GenerateTokenAndSave(user.UserIdentifier);

        repository.GetUserByEmailAndPassword(user);

        return new DoLoginUseCase(repository.Build(), refreshTokenGenerator.Build(),accessTokenGenerator);
    }

}
