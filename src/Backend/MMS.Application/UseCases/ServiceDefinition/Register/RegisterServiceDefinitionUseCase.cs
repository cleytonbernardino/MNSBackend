using MMS.Application.Extensions;
using MMS.Communication.Requests.ServiceDefinition;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.ServiceDefinition;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.ServiceDefinition.Register;

public class RegisterServicesUseCase(
    ILoggedUser loggedUser,
    IServiceDefinitionWriteOnlyRepository repository,
    IUnitOfWork unitOfWork
    ) : IRegisterServicesUseCase
{
    private readonly ILoggedUser _loggedUSer = loggedUser;
    private readonly IServiceDefinitionWriteOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private static async Task Validate(RequestRegisterServices request)
    {
        RegisterServicesValidator validator = new();

        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }

    public async Task Execute(RequestRegisterServices request)
    {
        var loggedUser = await _loggedUSer.User();

        await Validate(request);

        var entity = request.ToEntity();
        entity.CompanyId = loggedUser.CompanyId;
        entity.RegisteredBy = loggedUser.UserIdentifier;
        
        await _repository.Register(entity);
        await _unitOfWork.Commit();
    }
}
