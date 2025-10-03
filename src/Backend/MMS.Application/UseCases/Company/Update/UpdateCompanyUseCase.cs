using Microsoft.Extensions.Logging;
using MMS.Application.Extensions;
using MMS.Communication.Requests.Company;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.Company;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using MMS.Exceptions.LogMessages;

namespace MMS.Application.UseCases.Company.Update;

public class UpdateCompanyUseCase(
    ILoggedUser loggedUser,
    ICompanyUpdateOnlyRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateCompanyUseCase> logger
    ) : IUpdateCompanyUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ICompanyUpdateOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<UpdateCompanyUseCase> _logger = logger;
    
    public async Task Execute(RequestUpdateCompany request, long id)
    {
        var loggedUser = await _loggedUser.User();
        if (loggedUser.IsAdmin == false)
        {
            _logger.LogCritical(CriticalMessages.JWT_TOKEN_CHANGED);
            throw new NoPermissionException();
        }
        
        await Validated(request);
        
        var company = await _repository.GetById(id);
        if (company is null)
            throw new NotFoundException(ResourceMessagesException.COMPANY_NOT_FOUND);
        company = company.Join(request);
        
        _repository.Update(company);
        await _unitOfWork.Commit();
    }

    private static async Task Validated(RequestUpdateCompany request)
    {
        UpdateCompanyValidator validator = new();
        var result = await validator.ValidateAsync(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(
                result.Errors.Select(err => err.ErrorMessage).ToArray());
    }
}
