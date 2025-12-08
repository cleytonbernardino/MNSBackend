using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.ServiceDefinition;
using CommonTestUtilities.Services.LoggedUser;
using MMS.Application.UseCases.ServiceDefinition.List;
using MMS.Domain.Enums;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.ServiceDefinition.List;

public class ListServicesDefinitionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var services = ShortServicesDefinitionBuilder.BuildInBatch();
        
        var useCase = CreateUseCase(user, services);

        var result = await useCase.Execute();
        result.ServiceDefinitions.Length.ShouldBe(services.Length);
    }

    [Fact]
    public async Task Error_No_Permission()
    {
        var user = UserBuilder.Build();
        user.Role = UserRolesEnum.EMPLOYEE;
        
        var services = ShortServicesDefinitionBuilder.BuildInBatch();
        
        var useCase = CreateUseCase(user, services);

        var errors = await useCase.Execute().ShouldThrowAsync<NoPermissionException>();
        errors.Message.ShouldBe(ResourceMessagesException.NO_PERMISSION);
    }
    
    private static ListServicesDefinitionUseCase CreateUseCase(Entity.User user, Entity.ShortServiceDefinition[] services)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new ServiceDefinitionReadOnlyRepositoryBuilder().List(user, services).Build();

        return new ListServicesDefinitionUseCase(loggedUser, repository);
    }
}
