using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services.LoggedUser;
using MMS.Application.UseCases.User.Delete;
using MMS.Domain.Enums;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.User.Delete;

public class DeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        
        var useCase = CreateUseCase(user, 1);
        async Task act() => await useCase.Execute(1);

        await act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_User_Without_Permission()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.EMPLOYEE;

        var useCase = CreateUseCase(user, 1);
        async Task act() => await useCase.Execute(1);

        var errors = await act().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }
    
    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user, 1);
        async Task act() => await useCase.Execute(2);

        var errors = await act().ShouldThrowAsync<NotFoundException>();
        errors.Message.ShouldBe(ResourceMessagesException.USER_NOT_FOUND);
    }
    
    private DeleteUserUseCase CreateUseCase(Entity.User user, long userToDeleteId)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new UserUpdateOnlyRepositoryBuilder().GetById(userToDeleteId, user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new DeleteUserUseCase(loggedUser, repository.Build(), unitOfWork);
    }
}
