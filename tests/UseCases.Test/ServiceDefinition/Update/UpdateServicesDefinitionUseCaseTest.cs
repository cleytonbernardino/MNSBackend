using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.ServiceDefinition;
using CommonTestUtilities.Requests.ServicesDefinition;
using CommonTestUtilities.Services.LoggedUser;
using MMS.Application.UseCases.ServiceDefinition.Update;
using MMS.Domain.Enums;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;
using MMS.Exceptions;

namespace UseCases.Test.ServiceDefinition.Update;

public class UpdateServicesDefinitionUseCaseTest
{
    private readonly IdEncoderForTests _idEncoder = new();
    
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.EMPLOYEE;

        var service = ServiceDefinitionBuilder.Build(companyId: user.CompanyId, registeredBy: user.UserIdentifier);

        var request = RequestUpdateServiceDefinitionBuilder.Build(id: _idEncoder.Encode(service.Id));
        
        var useCase = CreateUseCase(user, service);
        async Task Act() => await useCase.Execute(request);

        await Act().ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Success_Administrators_Can_ByPass()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.MANAGER;

        var service = ServiceDefinitionBuilder.Build(companyId: user.CompanyId);

        var request = RequestUpdateServiceDefinitionBuilder.Build(id: _idEncoder.Encode(service.Id));
        
        var useCase = CreateUseCase(user, service);
        async Task Act() => await useCase.Execute(request);

        await Act().ShouldNotThrowAsync();
    }
    
    [Fact]
    public async Task Error_Validator()
    {
        var user = UserBuilder.Build();
        var service = ServiceDefinitionBuilder.Build(
            companyId: user.CompanyId, registeredBy: user.UserIdentifier);
        
        var request = RequestUpdateServiceDefinitionBuilder.Build(id: string.Empty);

        var useCase = CreateUseCase(user, service);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<ErrorOnValidationException>();
        errors.ErrorMessages.ShouldHaveSingleItem().ShouldBe(ResourceMessagesException.EMPTY_SERVICES_ID);
    }

    [Fact]
    public async Task Error_Service_Not_Found()
    {
        var user = UserBuilder.Build();
        var service = ServiceDefinitionBuilder.Build(
            companyId: user.CompanyId);

        var request = RequestUpdateServiceDefinitionBuilder.Build();

        var useCase = CreateUseCase(user, service);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<NotFoundException>();
        errors.Message.ShouldBe(ResourceMessagesException.SERVICE_NOT_FOUND);
    }
    
    [Fact]
    public async Task Error_Unauthorized_Employee()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.EMPLOYEE;
        
        var service = ServiceDefinitionBuilder.Build(
            companyId: user.CompanyId);

        var request = RequestUpdateServiceDefinitionBuilder.Build(id: _idEncoder.Encode(service.Id));

        var useCase = CreateUseCase(user, service);
        async Task Act() => await useCase.Execute(request);

        var errors = await Act().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }
    
    private static UpdateServicesDefinitionUseCase CreateUseCase(Entity.User user, Entity.ServiceDefinition service)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new ServiceDefinitionUpdateOnlyRepositoryBuilder()
            .GetById(user, service)
            .Build();
        var idEncoder = new IdEncoderBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        
        return new UpdateServicesDefinitionUseCase(loggedUser, repository, idEncoder, unitOfWork);
    }
}
