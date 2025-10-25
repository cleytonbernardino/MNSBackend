using Microsoft.Extensions.Logging;
using MMS.Application.Extensions;
using MMS.Application.Services.Encoders;
using MMS.Communication.Requests.Company;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.Company;
using MMS.Domain.Repositories.CompanySubscription;
using MMS.Domain.Repositories.SubscriptionPlan;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.CompanySubscription.RegisterAndUpdate;

public class RegisterCompanySubscriptionUseCase(
    ILoggedUser loggedUser,
    ILogger<RegisterCompanySubscriptionUseCase> logger,
    IIdEncoder idEncoder,
    ICompanySubscriptionWriteOnlyRepository repository,
    ICompanyReadOnlyRepository companyReadOnlyRepository,
    ISubscriptionPlanReadOnlyRepository planReadOnlyRepository,
    IUnitOfWork unitOfWork
    ) : IRegisterCompanySubscriptionUseCase
{
    #region Dependency Injection
    
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ILogger<RegisterCompanySubscriptionUseCase> _logger = logger;
    private readonly IIdEncoder _idEncoder = idEncoder;
    private readonly ICompanySubscriptionWriteOnlyRepository _repository = repository;
    private readonly ICompanyReadOnlyRepository _companyReadOnlyRepository = companyReadOnlyRepository;
    private readonly ISubscriptionPlanReadOnlyRepository _subscriptionPlanReadOnlyRepository = planReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    #endregion
    
    private async Task Validate(RequestRegisterCompanySubscription request)
    {
        RegisterCompanySubscriptionValidator validator = new();
        var result = await validator.ValidateAsync(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(err => err.ErrorMessage)
                .ToArray());

        bool exists = await _companyReadOnlyRepository.Exists(_idEncoder.Decode(request.CompanyId));
        if (!exists)
            throw new NotFoundException(ResourceMessagesException.COMPANY_NOT_FOUND);
        
        bool isShort = short.TryParse(_idEncoder.Decode(request.SubscriptionId).ToString(), out short id);
        if (!isShort)
            throw new ErrorOnValidationException([ResourceMessagesException.SUBSCRIPTION_DOES_NOT_EXIST]);
    
        exists = await _subscriptionPlanReadOnlyRepository.Exists(id);
        if (!exists)
            throw new NotFoundException(ResourceMessagesException.SUBSCRIPTION_DOES_NOT_EXIST);
    }
    
    public async Task Execute(RequestRegisterCompanySubscription request)
    {
        var loggedUser = await _loggedUser.User();
        if (loggedUser.IsAdmin == false)
        {
            _logger.LogCritical("Tentativa de registrar uma assinatura não altorizada");
            throw new NoPermissionException();
        }

        await Validate(request);

        var companySubscription = request.ToEntity();
        companySubscription.CompanyId = _idEncoder.Decode(request.CompanyId);
        companySubscription.SubscriptionPlanId = (short)_idEncoder.Decode(request.SubscriptionId);
        
        await _repository.RegisterCompanyPlan(companySubscription);
        await _unitOfWork.Commit();
    }
}
