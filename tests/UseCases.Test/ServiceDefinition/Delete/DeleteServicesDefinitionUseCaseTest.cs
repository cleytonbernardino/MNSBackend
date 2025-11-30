using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.ServiceDefinition;
using CommonTestUtilities.Services.LoggedUser;
using MMS.Application.UseCases.ServiceDefinition.Delete;
using MMS.Domain.Enums;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.ServiceDefinition.Delete;

public class DeleteServicesDefinitionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.EMPLOYEE;
        
        var service = ServiceDefinitionBuilder.Build();
        service.RegisteredBy = user.UserIdentifier;
        
        var useCase = CreateUseCase(user, service);
        async Task Act() => await useCase.Execute(service.Id);

        await Act().ShouldNotThrowAsync();
    }
    
    [Fact]
    public async Task Success_Administrators_Can_ByPass()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.MANAGER;
        
        var service = ServiceDefinitionBuilder.Build();
        
        var useCase = CreateUseCase(user, service);
        async Task Act() => await useCase.Execute(service.Id);

        await Act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Service_Definition_Not_Found()
    {
        var user = UserBuilder.Build();
        var service = ServiceDefinitionBuilder.Build();

        var useCase = CreateUseCase(user, service);
        async Task Act() => await useCase.Execute((service.Id + 1));

        var errors = await Act().ShouldThrowAsync<NotFoundException>();
        errors.Message.ShouldBe(ResourceMessagesException.SERVICE_NOT_FOUND);
    }
    
    [Fact]
    public async Task Error_Unauthorized_Employee()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.EMPLOYEE;
        
        var service = ServiceDefinitionBuilder.Build();

        var useCase = CreateUseCase(user, service);
        async Task Act() => await useCase.Execute(service.Id);

        var errors = await Act().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }
    
    private static DeleteServiceDefinitionUseCase CreateUseCase(Entity.User user, Entity.ServiceDefinition service)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new ServiceDefinitionUpdateOnlyRepositoryBuilder().GetById(user, service);
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new DeleteServiceDefinitionUseCase(loggedUser, repository.Build(), unitOfWork);
    }
}
