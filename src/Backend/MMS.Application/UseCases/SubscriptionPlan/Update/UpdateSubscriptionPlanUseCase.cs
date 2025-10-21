using Microsoft.Extensions.Logging;
using MMS.Application.Extensions;
using MMS.Application.Services.Encoders;
using MMS.Communication.Requests.SubscriptionsPlans;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.SubscriptionPlan;
using MMS.Domain.Services.Cache;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.SubscriptionPlan.Update;

public class UpdateSubscriptionPlanUseCase(
    ILoggedUser loggedUser,
    ISubscriptionPlanUpdateOnlyRepository repository,
    IIdEncoder idEncoder,
    ILogger<UpdateSubscriptionPlanUseCase> logger,
    IUnitOfWork unitOfWork,
    ICacheService cacheService
    ) : IUpdateSubscriptionPlanUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ISubscriptionPlanUpdateOnlyRepository _repository = repository;
    private readonly IIdEncoder _idEncoder = idEncoder;
    private readonly ILogger<UpdateSubscriptionPlanUseCase> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICacheService _cacheService = cacheService;

    private const string CACHE_KEY = "SubscriptionPlans";
    
    public async Task Execute(RequestUpdateSubscriptionPlan request)
    {
        var loggedUser = await _loggedUser.User();
        if (loggedUser.IsAdmin == false)
        {
            _logger.LogCritical($"Tentativa de atualizar um plano não altorizado, feita pelo usuário: {loggedUser.UserIdentifier}");
            throw new NoPermissionException();
        }

        var subscriptionPlan = await _repository.GetById(
            (short)_idEncoder.Decode(request.Id));
    
        if (subscriptionPlan is null)
            throw new NotFoundException(ResourceMessagesException.PLAN_NOT_FOUND);

        var newSubscriptionPlan = subscriptionPlan.Update(request);
        _repository.Update(newSubscriptionPlan);
    
        await _unitOfWork.Commit();
        await _cacheService.DeleteCache(CACHE_KEY);
    }
}
