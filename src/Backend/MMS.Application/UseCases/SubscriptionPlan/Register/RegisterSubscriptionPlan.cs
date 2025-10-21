using Microsoft.Extensions.Logging;
using MMS.Application.Extensions;
using MMS.Communication.Requests.SubscriptionsPlans;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.SubscriptionPlan;
using MMS.Domain.Services.Cache;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.SubscriptionPlan.Register;

public class RegisterSubscriptionPlan(
    ILoggedUser loggedUser,
    ISubscriptionPlanWriteOnlyRepository repository,
    ILogger<RegisterSubscriptionPlan> logger,
    IUnitOfWork unitOfWork,
    ICacheService cacheService
    ) : IRegisterSubscriptionPlanUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ISubscriptionPlanWriteOnlyRepository _repository = repository;
    private readonly ILogger<RegisterSubscriptionPlan> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICacheService _cacheService = cacheService;

    private const string CACHE_KEY = "SubscriptionPlans";
    
    public async Task Execute(RequestRegisterSubscriptionPlan request)
    {
        var loggedUser = await _loggedUser.User();
        if (loggedUser.IsAdmin == false)
        {
            _logger.LogCritical($"Usuário: ${loggedUser.UserIdentifier}, tentou cadastrar um novo plano sem altorização");
            throw new NoPermissionException();
        }
        await Validator(request);
    
        await _repository.Register(request.ToEntity());
        await _unitOfWork.Commit();
        await _cacheService.DeleteCache(CACHE_KEY);
    }

    private static async Task Validator(RequestRegisterSubscriptionPlan request)
    {
        RegisterSubscriptionPlanValidator validator = new();

        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
            throw new ErrorOnValidationException(
                result.Errors.Select(err => err.ErrorMessage).ToArray());
    }
}
