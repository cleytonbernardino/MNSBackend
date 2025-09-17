using MMS.Application.Extensions;
using MMS.Communication;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.Company;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.Company.Register;

public class RegisterCompanyUseCase(
    ILoggedUser loggedUser,
    ICompanyWriteOnlyRepository repository,
    IUnitOfWork unitOfWork
    ) : IRegisterCompanyUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ICompanyWriteOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(RequestRegisterCompany request)
    {
        var loggedUser = await _loggedUser.User();

        if (loggedUser.IsAdmin == false)
            throw new NoPermissionException();

        await Validator(request);

        var company = request.ToCompany();
        
        company.UpdatedOn = DateTime.UtcNow;
        company.Active = true;

        await _repository.RegisterCompany(company);
        await _unitOfWork.Commit();
    }

    private async Task Validator(RequestRegisterCompany request)
    {
        RegisterCompanyValidator validator = new();

        var result = await validator.ValidateAsync(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(err => err.ErrorMessage).ToArray());
    }
}
