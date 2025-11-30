using MMS.Domain.Enums;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.ServiceDefinition;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Entity = MMS.Domain.Entities;

namespace MMS.Application.UseCases.ServiceDefinition.Delete;

public class DeleteServiceDefinitionUseCase(
    ILoggedUser loggedUser,
    IServiceDefinitionUpdateOnlyRepository repository,
    IUnitOfWork unitOfWork
    ): IDeleteServiceDefinitionUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IServiceDefinitionUpdateOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.User();

        var service = await _repository.GetById(loggedUser, id);
        if (service is null)
            throw new NotFoundException(ResourceMessagesException.SERVICE_NOT_FOUND);
        
        CanDelete(loggedUser, service);
        _repository.Delete(service);

        await _unitOfWork.Commit();
    }

    private static void CanDelete(Entity.User loggedUser, Entity.ServiceDefinition serviceDefinition)
    {
        UserRolesEnum[] allowedRoles = [UserRolesEnum.MANAGER, UserRolesEnum.SUB_MANAGER];
        if (allowedRoles.Contains(loggedUser.Role))
            return;

        if (loggedUser.UserIdentifier != serviceDefinition.RegisteredBy)
            throw new NoPermissionException();
    }
}
