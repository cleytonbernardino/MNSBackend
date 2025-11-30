using MMS.Application.Extensions;
using MMS.Communication.Responses.ServiceDefinition;
using MMS.Domain.Enums;
using MMS.Domain.Repositories.ServiceDefinition;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Entity = MMS.Domain.Entities;

namespace MMS.Application.UseCases.ServiceDefinition.Get;

public class GetServicesDefinitionUseCase(
    ILoggedUser loggedUser,
    IServiceDefinitionReadOnlyRepository repository
    ): IGetServiceDefinitionUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IServiceDefinitionReadOnlyRepository _repository = repository;
    
    public async Task<ResponseGetServiceDefinition> Execute(long id)
    {
        var loggedUser = await _loggedUser.User();

        var service = await _repository.GetById(loggedUser, id);
        if (service is null)
            throw new NotFoundException(ResourceMessagesException.SERVICE_NOT_FOUND);

        CanSee(loggedUser, service);

        return service.ToResponse();
    }

    private static void CanSee(Entity.User loggedUser, Entity.ServiceDefinition serviceDefinition)
    {
        UserRolesEnum[] allowedRoles = [UserRolesEnum.MANAGER, UserRolesEnum.SUB_MANAGER];
        if (allowedRoles.Contains(loggedUser.Role))
            return;
        
        if (loggedUser.UserIdentifier != serviceDefinition.RegisteredBy)
            throw new NoPermissionException();
    }
}
