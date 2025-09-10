using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services.LoggedUser;
using MMS.Application.UseCases.Company.ListUsers;
using MMS.Communication;
using MMS.Domain.Enums;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.Company.ListUsers;

public class ListCompanyUsersUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        int numberOfUsers = 5;

        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user, numberOfUsers);
        async Task<ResponseListCompanyUser> act() => await useCase.Execute();

        var response = await act();
        response.Users
            .Count
            .ShouldBe(numberOfUsers);
    }

    [Fact]
    public async Task Error_User_Without_Permission()
    {
        var user = UserBuilder.Build();
        user.IsAdmin = false;
        user.Role = UserRolesEnum.EMPLOYEE;

        var useCase = CreateUseCase(user);
        async Task act() => await useCase.Execute();

        var errors = await act().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }

    private static ListCompanyUsersUseCase CreateUseCase(Entity.User user, int numberOfUsers = 5)
    {
        var idEncoder = new IdEncoderBuilder();
        idEncoder.Encoder();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new CompanyReadOnlyRepositoryBuilder().ListUsers(amount: numberOfUsers);

        return new ListCompanyUsersUseCase(idEncoder.Build(), loggedUser, repository.Build());
    }
}
