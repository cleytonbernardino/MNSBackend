using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.ServiceDefinition;
using CommonTestUtilities.Services.LoggedUser;
using MMS.Application.UseCases.ServiceDefinition.Get;
using MMS.Domain.Enums;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.ServiceDefinition.Get;

public class GetServicesDefinitionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.EMPLOYEE;

        var service = ServiceDefinitionBuilder.Build(
            registeredBy: user.UserIdentifier, companyId: user.CompanyId);
        
        var userCase = CreateUseCase(user, service);
        async Task Act() => await userCase.Execute(service.Id);

        await Act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Success_Administrators_Can_ByPass()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.MANAGER;

        var service = ServiceDefinitionBuilder.Build(companyId: user.CompanyId);
        
        var userCase = CreateUseCase(user, service);
        async Task Act() => await userCase.Execute(service.Id);

        await Act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Service_Not_Found()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.MANAGER;

        var service = ServiceDefinitionBuilder.Build(companyId: user.CompanyId);
        
        var userCase = CreateUseCase(user, service);
        async Task Act() => await userCase.Execute((service.Id + 1));

        var errors = await Act().ShouldThrowAsync<NotFoundException>();
        errors.Message.ShouldBe(ResourceMessagesException.SERVICE_NOT_FOUND);
    }

    [Fact]
    public async Task Error_Unauthorized_Employee()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.EMPLOYEE;

        var service = ServiceDefinitionBuilder.Build(companyId: user.CompanyId);
        
        var userCase = CreateUseCase(user, service);
        async Task Act() => await userCase.Execute(service.Id);

        var errors = await Act().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }
    
    private static GetServicesDefinitionUseCase CreateUseCase(Entity.User user, Entity.ServiceDefinition service)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new ServiceDefinitionReadOnlyRepositoryBuilder()
            .GetById(user, service)
            .Build();
        
        return new GetServicesDefinitionUseCase(loggedUser, repository);
    }
}
