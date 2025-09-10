using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using MMS.Application.UseCases.User.Update.Password;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.User.Update;

public class UpdateUserPasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, string password) = UserBuilder.BuildWithPassword();
        
        var request = RequestUpdateUserPasswordBuilder.Build();
        request.OldPassword = password;
        
        var useCase = CreateUseCase(user, password);
        async Task act() => await useCase.Execute(request);
        await act().ShouldNotThrowAsync();
    }
    
    [Fact]
    public async Task Error_Current_Password_Incorrect()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserPasswordBuilder.Build();
        
        var useCase = CreateUseCase(user, "rightPassword");
        async Task act() => await useCase.Execute(request);
        
        var errors = await act().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(ResourceMessagesException.CURRENT_PASSWORD_INCORRECT);
    }
    
    [Fact]
    public async Task Error_Password_Empty()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserPasswordBuilder.Build();
        request.NewPassword = string.Empty;
        
        var useCase = CreateUseCase(user, request.OldPassword);
        async Task act() => await useCase.Execute(request);
        
        var errors = await act().ShouldThrowAsync<ErrorOnValidationException>();
        errors.ErrorMessages
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.PASSWORD_EMPTY);
    }
    
    private static UpdateUserPasswordUseCase CreateUseCase(Entity.User user, string password)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new UserUpdateOnlyRepositoryBuilder().GetUserByEmailAndPassword(user, password);
        var passwordEncrypt = PasswordEncrypterBuilder.Build();
        var unityOfWork = UnitOfWorkBuilder.Build();
        
        return new UpdateUserPasswordUseCase(loggedUser, repository.Build(), passwordEncrypt, unityOfWork);
    }
}
