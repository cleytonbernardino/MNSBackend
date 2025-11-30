using MMS.Application.Extensions;
using MMS.Application.Services.Encoders;
using MMS.Communication.Requests.ServiceDefinition;
using MMS.Domain.Enums;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.ServiceDefinition;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Entity = MMS.Domain.Entities;

namespace MMS.Application.UseCases.ServiceDefinition.Update;

public class UpdateServicesDefinitionUseCase(
    ILoggedUser loggedUser,
    IServiceDefinitionUpdateOnlyRepository repository,
    IIdEncoder idEncoder,
    IUnitOfWork unitOfWork
    ) : IUpdateServicesDefinitionUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IServiceDefinitionUpdateOnlyRepository _repository = repository;
    private readonly IIdEncoder _idEncoder = idEncoder;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    private static void Validator(RequestUpdateServiceDefinition request)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
            throw new ErrorOnValidationException(errMessages: [ ResourceMessagesException.EMPTY_SERVICES_ID ]);
    }

    private static void CanUpdate(Entity.User user, Entity.ServiceDefinition service)
    {
        UserRolesEnum[] allowedRoles = [UserRolesEnum.MANAGER, UserRolesEnum.SUB_MANAGER];
        if (allowedRoles.Contains(user.Role))
            return;

        if (user.UserIdentifier != service.RegisteredBy)
            throw new NoPermissionException();
    }
    
    public async Task Execute(RequestUpdateServiceDefinition request)
    {
        var loggedUser = await _loggedUser.User();
        
        Validator(request);

        long id = _idEncoder.Decode(request.Id);
        var service = await _repository.GetById(loggedUser, id);

        if (service is null)
            throw new NotFoundException(ResourceMessagesException.SERVICE_NOT_FOUND);
        CanUpdate(loggedUser, service);
        
        service = service.Join(request);

        _repository.Update(service);
        
        await _unitOfWork.Commit();
    }
}
