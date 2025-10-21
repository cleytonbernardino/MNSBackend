using Microsoft.Extensions.Logging;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.SubscriptionPlan;
using MMS.Domain.Services.Cache;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.SubscriptionPlan.Delete;

public class DeleteSubscriptionPlanUseCase(
    ILoggedUser loggedUser,
    ILogger<DeleteSubscriptionPlanUseCase> logger,
    ISubscriptionPlanUpdateOnlyRepository repository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService
    ) : IDeleteSubscriptionPlanUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ILogger<DeleteSubscriptionPlanUseCase> _logger = logger;
    private readonly ISubscriptionPlanUpdateOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICacheService _cacheService = cacheService;
    
    private const string CACHE_KEY = "SubscriptionPlans";
    
    public async Task Execute(short id)
    {
        var loggedUser = await _loggedUser.User();
        if (loggedUser.IsAdmin == false)
        {
            _logger.LogCritical($"Tentativa de apagar um plano não altorizado, feita pelo usuário: {loggedUser.UserIdentifier}");
            throw new NoPermissionException();
        }

        await _repository.Delete(id);
        await _unitOfWork.Commit();
        await _cacheService.DeleteCache(CACHE_KEY);
    }
}
