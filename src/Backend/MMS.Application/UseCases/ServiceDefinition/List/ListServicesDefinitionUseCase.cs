using MMS.Application.Extensions;
using MMS.Communication.Responses.ServiceDefinition;
using MMS.Domain.Enums;
using MMS.Domain.Repositories.ServiceDefinition;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions.ExceptionsBase;
using Entity = MMS.Domain.Entities;

namespace MMS.Application.UseCases.ServiceDefinition.List;

public class ListServicesDefinitionUseCase(
    ILoggedUser loggedUser,
    IServiceDefinitionReadOnlyRepository repository
    ): IListServicesDefinitionUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IServiceDefinitionReadOnlyRepository _repository = repository;
    
    public async Task<ResponseShortServicesDefinition> Execute()
    {
        var loggedUser = await _loggedUser.User();
        CanSee(loggedUser);

        var services = await _repository.List(loggedUser);

        return new ResponseShortServicesDefinition { ServiceDefinitions = services.ToResponse() };
    }

    private static void CanSee(Entity.User loggedUser)
    {
        UserRolesEnum[] allowedRoles = [UserRolesEnum.MANAGER, UserRolesEnum.SUB_MANAGER];
        if (!allowedRoles.Contains(loggedUser.Role))
            throw new NoPermissionException();
    }
}
